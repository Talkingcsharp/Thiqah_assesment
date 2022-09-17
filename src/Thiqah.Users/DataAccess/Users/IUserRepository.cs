using Microsoft.EntityFrameworkCore;
using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.DataAccess.Users
{
    public interface IUserRepository
    {
        DbSet<User> Users { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
