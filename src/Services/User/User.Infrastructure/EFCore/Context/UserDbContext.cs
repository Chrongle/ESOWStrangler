using User.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace User.Infrastructure.EFCore.Context;

public partial class UserDbContext : DbContext
{
  public UserDbContext()
  {
  }

  public UserDbContext(DbContextOptions<UserDbContext> options)
      : base(options)
  {
  }

  public virtual DbSet<UserEntity> Users { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<UserEntity>(entity =>
    {
      entity.HasKey(x => x.Id);
      entity.Property(x => x.FirstName).IsRequired();
      entity.Property(x => x.LastName).IsRequired();
      entity.Property(x => x.Address).IsRequired();
      entity.Property(x => x.ZipCode).IsRequired();
      entity.Property(x => x.City).IsRequired();
      entity.Property(x => x.Country).IsRequired();
      entity.Property(x => x.Mobile).IsRequired();
      entity.Property(x => x.Email).IsRequired();

      entity.ToTable("Users");
    });

    OnModelCreatingPartial(modelBuilder);
  }

  protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
  {
    if (!optionsBuilder.IsConfigured)
    {
      optionsBuilder.UseSqlServer("Server=localhost,1436;Database=UserDb;User Id=sa;Password=Super.secretpa55word;TrustServerCertificate=True;");
    }
  }

  partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
