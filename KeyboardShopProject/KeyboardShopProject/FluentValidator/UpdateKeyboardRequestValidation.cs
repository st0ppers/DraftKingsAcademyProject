using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator
{
    public class UpdateKeyboardRequestValidation : AbstractValidator<UpdateKeyboardRequest>
    {
        public UpdateKeyboardRequestValidation()
        {
            RuleFor(k => k.KeyboardID).NotNull().GreaterThan(0).WithMessage("Enter correct value for ID");

        }
    }
}
