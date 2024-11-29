using Basket.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace Basket.Api.Extensions;

public static class MigrationManager
{
  public static WebApplication MigrateDatabase(this WebApplication webApp)
  {
    using (var scope = webApp.Services.CreateScope())
    using (var appContext = scope.ServiceProvider.GetRequiredService<CustomerBasketDbContext>())
      try
      {
        if (!appContext.Database.CanConnect())
        {
          appContext.Database.Migrate();
        }
      }
      catch (Exception ex)
      {
        throw new Exception("An error occurred while migrating the database.", ex);
      }
    return webApp;
  }
}