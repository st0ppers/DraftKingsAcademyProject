using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator.KeyboardValidations
{
    public class AddKeyboardRequestValidation : AbstractValidator<AddKeyboardRequest>
    {
        public AddKeyboardRequestValidation()
        {
            RuleFor(k => k.Model).NotEmpty().MinimumLength(3).MaximumLength(50).WithMessage("Enter correct value for model");
            RuleFor(k => k.Quantity).GreaterThan(0).WithMessage("Quantity cannot be negative number");
            RuleFor(k => k.Price).GreaterThan(0).WithMessage("Price cannot be negative number");
            RuleFor(k => k.Size).NotEmpty().WithMessage("Size cannot be empty");
            RuleFor(k => k.Color).NotEmpty().WithMessage("Color cannot be empty");
        }
    }
}
