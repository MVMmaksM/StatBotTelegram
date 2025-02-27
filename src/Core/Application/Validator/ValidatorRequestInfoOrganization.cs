using Application.Errors;
using Application.Interfaces;
using Application.Models;
using FluentValidation;

namespace Application.Services;

public class ValidatorRequestInfoOrganization : AbstractValidator<RequestInfoForm>
{
    public ValidatorRequestInfoOrganization()
    {
        //если ОКПО не пустое, то его длина должна быть
        //не меньше 8 символов
        RuleFor(r => r.Okpo)
            .MinimumLength(8)
            .WithMessage("{PropertyName} должно быть не меньше 8 знаков!")
            .MaximumLength(14)
            .WithMessage("{PropertyName} должно быть не больше 14 знаков!")
            .Must(r => int.TryParse(r, out int okpo) && okpo > 0)
            .WithMessage("{PropertyName} должно быть числом больше 0!")
            .WithName("ОКПО")
            .When(r => !string.IsNullOrWhiteSpace(r.Okpo));
    }
}