using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator.ShoppingCartValidations
{
    public class ShoppingCartRequestValidation : AbstractValidator<ShoppingCartRequest>
    {
        public ShoppingCartRequestValidation()
        {
            RuleFor(x => x.ClientId).GreaterThan(0).WithMessage("Enter valid client ID");
            RuleFor(x => x.KeyboardId).GreaterThan(0).WithMessage("Enter valid keyboard ID");
        }
    }
}
