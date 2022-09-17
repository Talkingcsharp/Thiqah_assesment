using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.DataAccess.Users
{
    public interface IUserQueryRepository
    {
        IQueryable<User> UsersQueryable { get; }
    }
}
