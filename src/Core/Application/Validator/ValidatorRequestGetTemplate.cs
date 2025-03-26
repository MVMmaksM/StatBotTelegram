using Application.Models;
using FluentValidation;

namespace Application.Services;

public class ValidatorRequestGetTemplate : AbstractValidator<RequestGetTemplate>
{
    public ValidatorRequestGetTemplate()
    {
        RuleFor(r => r.Filter.Okud)
            .Must(o => int.TryParse(o, out int okud) && okud > 0)
            .WithMessage("{PropertyName} должно быть числом больше 0!")
            .MinimumLength(6)
            .WithMessage("{PropertyName} должно быть не меньше 6 знаков!")
            .MaximumLength(7)
            .WithMessage("{PropertyName} должно быть не больше 7 знаков!")
            .WithName("ОКУД")
            .When(r => !string.IsNullOrWhiteSpace(r.Filter.Okud));
           
    }
}