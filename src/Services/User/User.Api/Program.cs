using Microsoft.EntityFrameworkCore;
using FastEndpoints;
using FastEndpoints.Swagger;
using User.Infrastructure.EFCore.Context;
using User.Core.Interfaces;
using User.Infrastructure.Repositories;
using User.Core.Services;
using SharedLibrary.Middleware;
using User.Infrastructure.Messaging;
using User.Core.Config;
using User.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UserDbContext>(c =>
{
    var connectionString = builder.Configuration.GetConnectionString("UserDb");
    c.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddScoped<IRepository, Repository>();

builder.Services.AddScoped<ICreateUserService, CreateUserService>();
builder.Services.AddScoped<IDeleteUserService, DeleteUserService>();
builder.Services.AddScoped<IGetUserService, GetUserService>();
builder.Services.AddScoped<IUpdateUserService, UpdateUserService>();

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddScoped<IRabbitMqConsumerService, RabbitMqConsumerService>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RestrictedAccessMiddleware>();

app.UseFastEndpoints();

app.UseHttpsRedirection();

if (Environment.GetEnvironmentVariable("MIGRATE_DATABASE") == "true")
{
    app.MigrateDatabase();
}

app.Run();
