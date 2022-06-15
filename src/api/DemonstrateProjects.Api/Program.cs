using DemonstrateProjects.Core.Interfaces;
using DemonstrateProjects.Core.Interfaces.Repositories;
using DemonstrateProjects.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using DemonstrateProjects.Infrastructure.Persistence.Repositories;
using DemonstrateProjects.Application.Services;
using DemonstrateProjects.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

/* Core and Infrastructure Layers */
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IPersonalReadKeyRepository, PersonalReadKeyRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// builder.Services.AddDbContext<AppDbContext>(opt => opt.UseNpgsql("Our ConnectionString!!"));

builder.Services.AddDbContext<AppDbContext>(opt => opt.UseInMemoryDatabase("database"));
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();

/* Application Layer */
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IPersonalReadKeyService, PersonalReadKeyService>();
builder.Services.AddScoped<IAuthService, AuthService>();

/* Authentication */
builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(opt => {
    opt.Events = new() {
        OnMessageReceived = async context => {
            var req = context.HttpContext.Request;
            var cookies = req.Cookies;

            if (cookies.TryGetValue("d_a", out var daValue))
                req.Headers.Authorization = "Bearer " + daValue;
            
            await Task.CompletedTask;
        }
    };
    opt.ClaimsIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Value;
    opt.Audience = builder.Configuration.GetSection("JwtConfig:Audience").Value;
    opt.TokenValidationParameters = new() {
        ValidIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Value,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:SecretSecureToken").Value))
    };
}); 

builder.Services.AddCors(x => {
    x.AddPolicy("CorsPolicy", y => {
        y.WithOrigins("http://localhost:4200").AllowCredentials().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);

var app = builder.Build();
app.UseRouting();
app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
