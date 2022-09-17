using Microsoft.EntityFrameworkCore;
using Thiqah.Shared.Context;
using Thiqah.Shared.Exceptions;
using Thiqah.Shared.Validation;
using Thiqah.Users.DataAccess.Users;
using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.Application.Users
{
    public sealed class UsersCommand : IUsersCommand
    {
        private readonly IUserRepository _userRepository;
        private readonly ActiveContext _axtiveContext;
        private readonly FluentValidator _fluentValidator;

        public UsersCommand(IUserRepository userRepository, ActiveContext axtiveContext, FluentValidator fluentValidator)
        {
            _userRepository = userRepository;
            _axtiveContext = axtiveContext;
            _fluentValidator = fluentValidator;
        }

        public async Task<int> CreateUser(User input)
        {
            _fluentValidator.Validate(input);

            await _userRepository.Users.AddAsync(input);
            await _userRepository.SaveChangesAsync();
            return input.Id;
        }

        public async Task IncreaseUserOrders(int userId)
        {
            User? user = await _userRepository.Users.FirstOrDefaultAsync(x => x.Id == userId);

            if(user is null)
            {
                throw new DataNotFoundException();
            }

            user.OrdersCount++;
            await _userRepository.SaveChangesAsync();
        }
    }
}
