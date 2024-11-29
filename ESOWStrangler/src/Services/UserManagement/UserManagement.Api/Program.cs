using System.Text;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using UserManagement.Api.Extensions;
using UserManagement.Api.JwtFeatures;
using UserManagement.Core.Config;
using UserManagement.Core.Entities;
using UserManagement.Core.Interfaces;
using UserManagement.Infrastructure.EFCore.Context;
using UserManagement.Infrastructure.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddDbContext<UserManagementDbContext>(opts =>
    opts.UseSqlServer(builder.Configuration.GetConnectionString("UserManagementDb")));

builder.Services.AddIdentity<User, IdentityRole>()
  .AddEntityFrameworkStores<UserManagementDbContext>();

var jwtSettings = builder.Configuration.GetSection("JWTSettings");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opts =>
{
    opts.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["validIssuer"],
        ValidAudience = jwtSettings["validAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["securityKey"]))
    };
});

builder.Services.AddAuthorization();
builder.Services.AddSingleton<JwtHandler>();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddScoped<IPublisherService, RabbitMqPublishService>();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.UseFastEndpoints();
app.UseSwaggerGen();

if (Environment.GetEnvironmentVariable("MIGRATE_DATABASE") == "true")
{
    app.MigrateDatabase();
}

app.Run();