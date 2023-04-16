using anuR.Models;
using Microsoft.EntityFrameworkCore;

namespace anuR.Context;

public class MainContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Site> Sites { get; set; }
    
    public MainContext(DbContextOptions<MainContext> options) : base(options)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
    }

    public override int SaveChanges()
    {
        AddTimestamps();
        return base.SaveChanges();
    }

    public async Task<int> SaveChangesAsync()
    {
        AddTimestamps();
        return await base.SaveChangesAsync();
    }

    private void AddTimestamps()
    {
        var entities = ChangeTracker.Entries()
            .Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

        foreach (var entity in entities)
        {
            var now = DateTime.UtcNow; // current datetime

            if (entity.State == EntityState.Added) ((BaseEntity)entity.Entity).CreatedAt = now;
            ((BaseEntity)entity.Entity).UpdatedAt = now;
        }
    }
    
    protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<User>()
            .Property(u=>u.Id)
            .HasDefaultValueSql("uuid_generate_v4()");
    }
}