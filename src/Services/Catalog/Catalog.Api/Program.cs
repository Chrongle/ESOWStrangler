using Catalog.Core.Entities;
using Catalog.Core.Interfaces;
using Catalog.Core.Services;
using Catalog.Infrastructure.EFCore.Context;
using Catalog.Infrastructure.Repository;
using Catalog.Infrastructure.Cache;
using SharedLibrary.Middleware;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Catalog.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CatalogDbContext>(c =>
{
    var connectionString = builder.Configuration.GetConnectionString("CatalogDb");
    c.UseSqlServer(connectionString, sqlOptions => sqlOptions.EnableRetryOnFailure());
});

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("RedisConnection");
});

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

builder.Services.AddScoped<IRepository<CatalogItem>, Repository<CatalogItem>>();
builder.Services.AddScoped<IRepository<CatalogBrand>, Repository<CatalogBrand>>();
builder.Services.AddScoped<IRepository<CatalogType>, Repository<CatalogType>>();
builder.Services.AddScoped<ICacheService, RedisCacheService>();
builder.Services.AddScoped<IGetCatalogItemService, GetCatalogItemService>();
builder.Services.AddScoped<IGetCatalogService<CatalogBrand>, GetCatalogBrandService>();
builder.Services.AddScoped<IGetCatalogService<CatalogType>, GetCatalogTypeService>();
builder.Services.AddScoped<IUpdateCatalogService, UpdateCatalogItemService>();

var app = builder.Build();

app.UseMiddleware<RestrictedAccessMiddleware>();

app.UseFastEndpoints();

app.UseSwaggerGen();

if (Environment.GetEnvironmentVariable("MIGRATE_DATABASE") == "true")
{
    app.MigrateDatabase();
}

app.Run();

public partial class Program { }
