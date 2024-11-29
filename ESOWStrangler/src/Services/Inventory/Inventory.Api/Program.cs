using Inventory.Api.Extensions;
using Inventory.Core.Config;
using Inventory.Core.Interfaces;
using Inventory.Core.Services;
using Inventory.Infrastructure.EFCore.Context;
using Inventory.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryDbContext>(config =>
{
    var connectionString = builder.Configuration.GetConnectionString("InventoryDb");
    config.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.Configure<RabbitMqSettings>(builder.Configuration.GetSection("RabbitMqSettings"));
builder.Services.AddScoped<IPublisherService, RabbitMqPublishService>();
builder.Services.AddScoped<IConsumerService, RabbitMqConsumerService>();
builder.Services.AddHostedService<RabbitMqConsumerService>();

builder.Services.AddScoped<IInventoryService, InventoryService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

if (Environment.GetEnvironmentVariable("MIGRATE_DATABASE") == "true")
{
    app.MigrateDatabase();
}

app.Run();