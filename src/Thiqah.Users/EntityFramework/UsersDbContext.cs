using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Thiqah.Shared.Context;
using Thiqah.Shared.Domain;
using Thiqah.Users.DataAccess.Users;
using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.EntityFramework
{
    public sealed class UsersDbContext : DbContext, IUserRepository, IUserQueryRepository
    {
        private readonly ActiveContext _activeContext;

        public UsersDbContext(DbContextOptions options, ActiveContext activeContext) : base(options)
        {
            _activeContext = activeContext;
        }

        public DbSet<User> Users => Set<User>();
        public IQueryable<User> UsersQueryable => Users.AsQueryable();


        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);


        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var item in ChangeTracker.Entries())
            {
                if (item.State == EntityState.Added)
                {
                    SetEntityAdded(item.Entity, _activeContext);
                }

                if (item.State == EntityState.Modified)
                {
                    SetEntityModified(item.Entity, _activeContext, item.OriginalValues);
                }

                if (item.State == EntityState.Deleted)
                {
                    item.State = EntityState.Modified;
                    SetEntityDeleted(item.Entity, _activeContext, item.OriginalValues);
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }


        private static void SetEntityAdded(object entity, ActiveContext activeContext)
        {
            var myEntity = entity as BaseDomain;
            if (myEntity is null) return;
            myEntity.DateCreated = DateTime.UtcNow;
            myEntity.UserCreated = activeContext.UserId;
            myEntity.IsDeleted = false;
        }

        private static void SetEntityModified(object entity, ActiveContext activeContext, PropertyValues originalValues)
        {
            var myEntity = entity as BaseDomain;
            if (myEntity is null) return;

            myEntity.DateUpdated = DateTime.UtcNow;
            myEntity.UserUpdated = activeContext.UserId;
            myEntity.DateCreated = originalValues.GetValue<DateTime?>(nameof(BaseDomain.DateCreated)) ?? myEntity.DateCreated;
            myEntity.UserCreated = originalValues.GetValue<int?>(nameof(BaseDomain.UserCreated)) ?? myEntity.UserCreated;
            myEntity.IsDeleted = false;
        }

        private static void SetEntityDeleted(object entity, ActiveContext activeContext, PropertyValues originalValues)
        {
            var myEntity = entity as BaseDomain;
            if (myEntity is null) return;

            myEntity.DateUpdated = DateTime.UtcNow;
            myEntity.UserUpdated = activeContext.UserId;
            myEntity.DateCreated = originalValues.GetValue<DateTime?>(nameof(BaseDomain.DateCreated)) ?? myEntity.DateCreated;
            myEntity.UserCreated = originalValues.GetValue<int?>(nameof(BaseDomain.UserCreated)) ?? myEntity.UserCreated;
            myEntity.IsDeleted = true;
        }
    }
}
