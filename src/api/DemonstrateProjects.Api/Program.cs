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
    x.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false; // False only for development mode!
    x.SaveToken = true;
    x.TokenValidationParameters = new()
    {
        ValidAudience = builder.Configuration.GetSection("JwtConfig:Audience").Value,
        ValidIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Value,
        ValidateAudience = true,
        ValidateIssuer = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:SecretSecureToken").Value))
    };
    x.Events = new JwtBearerEvents()
    {
        OnMessageReceived = async y => {
            y.HttpContext.Request.Headers.Authorization = "Bearer " + y.Token;
            await Task.CompletedTask;
        }
    };
});

builder.Services.AddCors(x => {
    x.AddPolicy("CorsPolicy", y => {
        y.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});

builder.Services.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("CorsPolicy");

app.MapControllers();

app.Run();
