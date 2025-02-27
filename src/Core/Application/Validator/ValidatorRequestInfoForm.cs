using Application.Errors;
using Application.Interfaces;
using Application.Models;
using FluentValidation;

namespace Application.Services;

public class ValidatorRequestInfoForm : AbstractValidator<RequestInfoForm>
{
    public ValidatorRequestInfoForm()
    {
        //если ОКПО не пустое, то его длина должна быть
        //не меньше 8 и не больше 14 символов
        RuleFor(r => r.Okpo)
            .MinimumLength(8)
            .WithMessage("{PropertyName} должно быть не меньше 8 знаков!")
            .MaximumLength(14)
            .WithMessage("{PropertyName} должно быть не больше 14 знаков!")
            .Must(r => int.TryParse(r, out int okpo) && okpo > 0)
            .WithMessage("{PropertyName} должно быть числом больше 0!")
            .WithName("ОКПО")
            .When(r => !string.IsNullOrWhiteSpace(r.Okpo));
        
        //если ИНН не пустое, то его длина должна быть
        //не меньше 10 и не больше 12 символов
        RuleFor(r => r.Inn)
            .MinimumLength(8)
            .WithMessage("{PropertyName} должен быть не меньше 10 знаков!")
            .MaximumLength(14)
            .WithMessage("{PropertyName} должен быть не больше 12 знаков!")
            .Must(r => int.TryParse(r, out int inn) && inn > 0)
            .WithMessage("{PropertyName} должен быть числом больше 0!")
            .WithName("ИНН")
            .When(r => !string.IsNullOrWhiteSpace(r.Inn));
        
        //если ОГРН/ОГРНИП не пустое, то его длина должна быть
        //не меньше 13 и не больше 15 символов
        RuleFor(r => r.Ogrn)
            .MinimumLength(8)
            .WithMessage("{PropertyName} должен быть не меньше 13 знаков!")
            .MaximumLength(14)
            .WithMessage("{PropertyName} должен быть не больше 15 знаков!")
            .Must(r => int.TryParse(r, out int ogrn) && ogrn > 0)
            .WithMessage("{PropertyName} должен быть числом больше 0!")
            .WithName("ИНН")
            .When(r => !string.IsNullOrWhiteSpace(r.Ogrn));
    }
}