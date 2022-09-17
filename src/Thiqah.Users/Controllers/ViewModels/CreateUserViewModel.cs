using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.Controllers.ViewModels
{
    public sealed  class CreateUserViewModel
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsMale { get; set; }
        public int Age { get; set; }

        public User ToUser()
        {
            return new User
            {
                UserName = UserName,
                Name = Name,
                Email = Email,
                IsMale = IsMale,
                Age = Age
            };
        }
    }
}
