using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator
{
    public class AddKeyboardRequestValidation : AbstractValidator<AddKeyboardRequest>
    {
        public AddKeyboardRequestValidation()
        {
            RuleFor(k => k.Model).NotEmpty().MinimumLength(3).MaximumLength(50).WithMessage("Enter correct value for model");
            RuleFor(k => k.Quantity).NotNull().GreaterThan(0).WithMessage("Enter correct value for quantity");
            RuleFor(k => k.Price).NotNull().GreaterThan(0).WithMessage("Enter correct value for price");
            RuleFor(k => k.Size).NotEmpty().WithMessage("Size cannot be empty");
            RuleFor(k => k.Color).NotEmpty().WithMessage("Color cannot be empty");
        }
    }
}
