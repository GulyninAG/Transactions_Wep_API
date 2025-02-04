using FluentValidation;
using Transactions_Web_API.Domain;

namespace Transactions_Web_API.Application.Validators
{
  public class TransactionValidator : AbstractValidator<Transaction>
  {
    public TransactionValidator()
    {
      RuleFor(x => x.Id).NotEmpty().WithMessage("ID не может быть пустым.");
      RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Сумма должна быть положительной.");
      RuleFor(x => x.TransactionDate)
          .LessThanOrEqualTo(DateTime.Now)
          .WithMessage("Дата транзакции не может быть в будущем.");
    }
  }
}
