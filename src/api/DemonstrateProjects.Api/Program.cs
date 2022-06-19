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
    opt.RequireHttpsMetadata = true;
    opt.SaveToken = true;
    opt.TokenValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:SecretSecureToken").Value)),
        ValidIssuer = builder.Configuration.GetSection("JwtConfig:Issuer").Value,
        ValidAudience = builder.Configuration.GetSection("JwtConfig:Audience").Value
    };
});

builder.Services.AddCors(x => {
    /* Default policy for our hosted webapp! */
    x.AddPolicy("WebPolicy", opt => {
        opt.WithOrigins(builder.Configuration.GetSection("CorsConfig:WebUrl").Value)
            .AllowAnyHeader()
                .AllowAnyMethod()
                    .AllowCredentials();
    });

    /* Default policy for anyone who wanna get projects using keys! */
    x.AddPolicy("ApiPolicy", opt => {
        opt.AllowAnyOrigin().WithMethods("POST", "OPTIONS").WithHeaders("Content-Type");
    });
});

builder.Services.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);

var app = builder.Build();
app.UseRouting();
app.UseHttpsRedirection();

app.UseCors("WebPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
