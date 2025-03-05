using Application.Models.SearchEmployees;
using FluentValidation;

namespace Application.Services;

public class ValidatorRequestSearchEmployees : AbstractValidator<RequestSearchEmployees>
{
    public ValidatorRequestSearchEmployees()
    {
        //если ОКУД не пустой, то его длина должна быть
        //не меньше 6 и не больше 7 символов
        RuleFor(r => r.Okud)
            .MinimumLength(6)
            .WithMessage("{PropertyName} должно быть не меньше 6 знаков!")
            .MaximumLength(7)
            .WithMessage("{PropertyName} должно быть не больше 7 знаков!")
            .Must(r => int.TryParse(r, out int okud) && okud > 0)
            .WithMessage("{PropertyName} должно быть числом больше 0!")
            .WithName("ОКУД")
            .When(r => !string.IsNullOrWhiteSpace(r.Okud));
    }
}