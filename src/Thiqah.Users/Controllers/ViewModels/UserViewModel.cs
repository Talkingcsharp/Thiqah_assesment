using Thiqah.Users.Domain.Users;

namespace Thiqah.Users.Controllers.ViewModels
{
    public sealed class UserViewModel
    {
        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsMale { get; set; }
        public int Age { get; set; }
        public int OrdersCount { get; set; }
        public int Id { get; set; }

        public static UserViewModel FromUser(User input)
        {
            return new UserViewModel
            {
                Age = input.Age,
                Email = input.Email,
                IsMale = input.IsMale,
                Name = input.Name,
                OrdersCount = input.OrdersCount,
                UserName = input.UserName,
                Id = input.Id
            };
        }
    }
}
