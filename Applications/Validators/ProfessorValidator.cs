using FluentValidation;
using connect_cic_api.Domain;

namespace connect_cic_api.Services.Validators;
public class ProfessorValidator : AbstractValidator<Professor>
{
   public ProfessorValidator()
   {
     RuleFor(p => p.Name)
         .NotEmpty().WithMessage("O campo 'Nome' é obrigatório.")
         .MinimumLength(6).WithMessage("O campo 'Nome' deve ter no mínimo 6 caracteres.");
   
     RuleFor(p => p.Email)
         .NotEmpty().WithMessage("O campo 'Email' é obrigatório.")
         .EmailAddress().WithMessage("O campo 'Email' deve ser um e-mail válido.");

     RuleFor(p => p.Department)
         .NotEmpty().WithMessage("O campo 'Department' é obrigatório.");
        
   }
}
