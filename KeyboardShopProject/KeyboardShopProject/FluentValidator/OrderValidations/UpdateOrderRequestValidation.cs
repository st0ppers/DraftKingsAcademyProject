using FluentValidation;
using Keyboard.Models.Requests;

namespace Keyboard.ShopProject.FluentValidator.OrderValidations
{
    public class UpdateOrderRequestValidation : AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderRequestValidation()
        {
            RuleFor(o => o.OrderID).NotNull().WithMessage("Enter valid id");
            RuleFor(o => o.ClientID).GreaterThan(0).WithMessage("Enter valid client ID");
            RuleFor(o => o.Keyboards).NotEmpty().WithMessage("Keyboards cannot be empty");
            RuleFor(o => o.Date).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Date cannot be in the past");
        }
    }
}
