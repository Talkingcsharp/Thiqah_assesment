using FluentValidation;
using Thiqah.Shared.Domain;
using Thiqah.Shared.Validation;

namespace Thiqah.Users.Domain.Users
{
    public sealed class User : BaseDomain, IValidationModel<User>
    {
        public AbstractValidator<User> Validator => new UserValidator();

        public string? UserName { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public bool IsMale { get; set; }
        public int Age { get; set; }
        public int OrdersCount { get; set; }
    }

    public sealed class UserValidator: AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(x => x.UserName).NotNull().MinimumLength(2).MaximumLength(100);
            RuleFor(x=>x.Name).NotNull().MinimumLength(2).MaximumLength(100);
            RuleFor(x => x.Email).EmailAddress().NotNull();
            RuleFor(x => x.Age).GreaterThanOrEqualTo(10).LessThanOrEqualTo(100);
            RuleFor(x => x.OrdersCount).GreaterThanOrEqualTo(0);
        }
    }
}
