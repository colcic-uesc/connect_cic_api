using FluentValidation;
using connect_cic_api.Domain;

namespace connect_cic_api.Services.Validators;
public class VacancyTypeValidator  : AbstractValidator<VacancyType>
{
    public VacancyTypeValidator()
   {
     RuleFor(vt => vt.Name)
         .NotEmpty().WithMessage("O campo 'Name' é obrigatório.");
   }
}
