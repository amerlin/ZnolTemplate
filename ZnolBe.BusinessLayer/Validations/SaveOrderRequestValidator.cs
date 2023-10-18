using FluentValidation;
using ZnolBe.Shared.Models.Requests;

namespace ZnolBe.BusinessLayer.Validations;
public class SaveOrderRequestValidator: AbstractValidator<SaveOrderRequest>
{
    public SaveOrderRequestValidator()
    {
        RuleFor(o => o.TotalPrice).GreaterThan(0).WithMessage("Total price must be greated 0.");
    }
}
