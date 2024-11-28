using Basket.Api.Endpoints;
using Basket.Api.Extensions;
using Basket.Core.Entities;
using Basket.Core.Interfaces;
using Basket.Core.Services;
using Basket.Infrastructure.EFCore.Context;
using Basket.Infrastructure.Repositories;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CustomerBasketDbContext>(c =>
{
    var connectionString = builder.Configuration.GetConnectionString("BasketDb");
    c.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.AddScoped<IRepository<CustomerBasket>, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();

builder.Services.AddFastEndpoints();
builder.Services.AddSwaggerDocument();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseFastEndpoints();

if (Environment.GetEnvironmentVariable("MIGRATE_DATABASE") == "true")
{
    app.MigrateDatabase();
}

app.Run();
