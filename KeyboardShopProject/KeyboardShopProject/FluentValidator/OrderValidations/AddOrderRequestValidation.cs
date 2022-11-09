using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator.OrderValidations
{
    public class AddOrderRequestValidation : AbstractValidator<AddOrderRequest>
    {
        public AddOrderRequestValidation()
        {
            RuleFor(o => o.ClientID).GreaterThan(0).WithMessage("Enter valid client ID");
            RuleFor(o => o.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Date cannot be in the past");
        }
    }
}
