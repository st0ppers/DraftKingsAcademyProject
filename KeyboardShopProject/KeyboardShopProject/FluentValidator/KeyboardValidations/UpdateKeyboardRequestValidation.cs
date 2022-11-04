using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator.KeyboardValidations
{
    public class UpdateKeyboardRequestValidation : AbstractValidator<UpdateKeyboardRequest>
    {
        public UpdateKeyboardRequestValidation()
        {
            RuleFor(k => k.KeyboardID).GreaterThan(0).WithMessage("Enter correct value for ID");
            RuleFor(k => k.Model).NotEmpty().MinimumLength(3).MaximumLength(50).WithMessage("Enter correct value for model");
            RuleFor(k => k.Color).NotEmpty().WithMessage("Color cannot be empty");
            RuleFor(k => k.Price).GreaterThan(0).WithMessage("Price cannot be negative");
            RuleFor(k => k.Quantity).GreaterThan(0).WithMessage("Quantity cannot be negative");
            RuleFor(k => k.Size).NotEmpty().WithMessage("Size cannot be empty");

        }
    }
}
