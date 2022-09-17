using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.Application.Users;

public interface IUsersQuery
{
    Task<List<User>> ListUsers();
    Task<User> GetUser(int id);
}
