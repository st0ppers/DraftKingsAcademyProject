using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator.ClientValidations
{
    public class UpdaterClientRequestValidation : AbstractValidator<UpdateClientRequest>
    {
        public UpdaterClientRequestValidation()
        {
            RuleFor(c => c.FullName).NotEmpty().Matches("[A-Za-z]").MinimumLength(5).MaximumLength(50).WithMessage("Enter valid name");
            RuleFor(c => c.Address).NotEmpty().Matches("^[a-zA-Z0-9_.-]*$").MinimumLength(5).MaximumLength(50)
                .WithMessage("Enter valid address");
            RuleFor(c => c.Age).GreaterThan(0).WithMessage("Age cannot be negative number");
            RuleFor(c => c.ClientID).NotNull().GreaterThan(0).WithMessage("Enter valid ID");
        }
    }
}
