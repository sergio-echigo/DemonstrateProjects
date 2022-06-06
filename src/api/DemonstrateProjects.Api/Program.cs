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
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false; // False only for development mode!
    x.SaveToken = true;
    x.TokenValidationParameters = new()
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JwtConfig:SecretSecureToken").Value))
    };
});

builder.Services.AddControllers(opt => opt.SuppressAsyncSuffixInActionNames = false);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
