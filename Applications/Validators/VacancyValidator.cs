using FluentValidation;
using connect_cic_api.Domain;

namespace connect_cic_api.Services.Validators;
public class VacancyValidator : AbstractValidator<Vacancy>
{
     public VacancyValidator()
   {
        RuleFor(v => v.Value)
            .GreaterThanOrEqualTo(0).WithMessage("O campo 'Value' deve ser maior igual a 0.");
   
     RuleFor(v => v.StartDate)
         .NotEmpty().WithMessage("O campo 'Data' inicial é obrigatório.");
    
     RuleFor(v => v.StartDate.Year)
         .GreaterThanOrEqualTo(2000).WithMessage("A data deve ser após os anos 2000.");
   
     RuleFor(v => v.EndDate)
         .NotEmpty().WithMessage("O campo 'Data' final é obrigatório.")
         .GreaterThan(v => v.StartDate).WithMessage("A data final não pode começar antes da data final.");

     RuleFor(v => v.ProjectTitle)
         .NotEmpty().WithMessage("O campo 'Título do Projeto' é obrigatório.")
         .MinimumLength(10).WithMessage("O campo 'Título do Projeto' deve ter no mínimo 10 caracteres.");
        
      RuleFor(v => v.Status)
         .NotEmpty().WithMessage("O campo 'Status' é obrigatório.");

      RuleFor(v => v.VacancyTypeID)
         .NotEmpty().WithMessage("O campo 'VacancyTypeID' é obrigatório.");
            
   }
}
