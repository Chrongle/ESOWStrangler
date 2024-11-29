using FastEndpoints;
using FastEndpoints.Swagger;
using Inventory.Api.Extensions;
using Microsoft.EntityFrameworkCore;
using Order.Core.Config;
using Order.Core.Interfaces;
using Order.Core.Services;
using Order.Infrastructure.EFCore.Context;
using Order.Infrastructure.Messaging;
using Order.Infrastructure.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddDbContext<OrderDbContext>(config =>
{
    var connectionString = builder.Configuration.GetConnectionString("OrderDb");
    config.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddScoped<IPublisherService, RabbitMqPublishService>();
builder.Services.AddScoped<IConsumerService, RabbitMqConsumerService>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

builder.Services.AddScoped<IRepository, Repository>();
builder.Services.AddScoped<ICustomerOrderService, CustomerOrderService>();

var app = builder.Build();

app.UseFastEndpoints();
app.UseSwaggerGen();

if (Environment.GetEnvironmentVariable("MIGRATE_DATABASE") == "true")
{
    app.MigrateDatabase();
}

app.Run();