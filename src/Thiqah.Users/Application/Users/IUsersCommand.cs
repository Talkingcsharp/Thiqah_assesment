using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.Application.Users;

public interface IUsersCommand
{
    Task<int> CreateUser(User input);
    Task IncreaseUserOrders(int userId);
}
