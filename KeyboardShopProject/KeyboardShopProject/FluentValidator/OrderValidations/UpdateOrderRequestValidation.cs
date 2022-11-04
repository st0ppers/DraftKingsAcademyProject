using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator.OrderValidations
{
    public class UpdateOrderRequestValidation : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidation()
        {
            RuleFor(o => o.OrderID).GreaterThan(0).WithMessage("Enter valid Order ID");
            RuleFor(o => o.ClientID).GreaterThan(0).WithMessage("Enter valid client ID");
            RuleFor(o => o.KeyboardID).GreaterThan(0).WithMessage("Enter valid keyboard ID");
            RuleFor(o => o.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Date cannot be in the past");
        }
    }
}
