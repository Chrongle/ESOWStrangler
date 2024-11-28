using System.Text;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Web.Gateway.Api.Middelwares;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var jwtSerttings = builder.Configuration.GetSection("JWTSettings");
builder.Services.AddAuthentication().AddJwtBearer(opts =>
  {
      opts.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
      {
          ValidateIssuer = true,
          ValidateAudience = true,
          ValidateLifetime = true,
          ValidateIssuerSigningKey = true,
          ValidIssuer = jwtSerttings["validIssuer"],
          ValidAudience = jwtSerttings["validAudience"],
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
            .GetBytes(jwtSerttings.GetSection("securityKey").Value))
      };
  });

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseMiddleware<InterceptionMiddelware>();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();
