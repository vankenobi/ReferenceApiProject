using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Reference.Api.Models;

namespace Reference.Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
            Database.EnsureCreated();
            Database.Migrate();
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {}

        public DbSet<User> Users { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetCreatedAt();
            SetUpdatedAt();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetCreatedAt()
        {
            var entitiesAdded = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Added)
                .Select(x => x.Entity)
                .OfType<BaseEntity>();

            foreach (var entity in entitiesAdded)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
        }

        private void SetUpdatedAt()
        {
            var entitiesModified = ChangeTracker.Entries()
                .Where(x => x.State == EntityState.Modified)
                .Select(x => x.Entity)
                .OfType<BaseEntity>();

            foreach (var entity in entitiesModified)
            {
                entity.UpdatedAt = DateTime.UtcNow;

            }
        }
    }
}

