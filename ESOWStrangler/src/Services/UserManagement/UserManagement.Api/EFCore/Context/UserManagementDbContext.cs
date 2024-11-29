using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UserManagement.Core.Entities;

namespace UserManagement.Infrastructure.EFCore.Context;

public class UserManagementDbContext : IdentityDbContext<User>
{
    public UserManagementDbContext(DbContextOptions options) : base(options)
    {
    }

    protected UserManagementDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
