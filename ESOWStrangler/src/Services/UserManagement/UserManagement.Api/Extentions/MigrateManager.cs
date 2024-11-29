using UserManagement.Infrastructure.EFCore.Context;
using Microsoft.EntityFrameworkCore;

namespace UserManagement.Api.Extensions;

public static class MigrationManager
{
  public static WebApplication MigrateDatabase(this WebApplication webApp)
  {
    using (var scope = webApp.Services.CreateScope())
    using (var appContext = scope.ServiceProvider.GetRequiredService<UserManagementDbContext>())
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