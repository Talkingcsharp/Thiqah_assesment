using Microsoft.EntityFrameworkCore;
using Thiqah.Shared.Exceptions;
using Thiqah.Users.DataAccess.Users;
using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.Application.Users
{
    public sealed class UsersQuery : IUsersQuery
    {
        private readonly IUserQueryRepository _repository;

        public UsersQuery(IUserQueryRepository repository)
        {
            _repository = repository;
        }

        public async Task<User> GetUser(int id)
        {
            User? user = await _repository.UsersQueryable.FirstOrDefaultAsync(w => w.Id == id);

            if(user is null)
            {
                throw new DataNotFoundException();
            }

            return user;
        }

        public async Task<List<User>> ListUsers()
        {
            return await _repository.UsersQueryable.ToListAsync();
        }
    }
}
