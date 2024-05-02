using FluentValidation;
using connect_cic_api.Domain;

namespace connect_cic_api.Services.Validators;

public class StudentValidator : AbstractValidator<Student>
{
    public StudentValidator()
   {
     RuleFor(s => s.Name)
         .NotEmpty().WithMessage("O campo 'Nome' é obrigatório.")
         .MinimumLength(6).WithMessage("O campo 'Nome' deve ter no mínimo 6 caracteres.");
   
     RuleFor(s => s.Email)
         .NotEmpty().WithMessage("O campo 'Email' é obrigatório.")
         .EmailAddress().WithMessage("O campo 'Email' deve ser um e-mail válido.");

     RuleFor(s => s. Course)
         .NotEmpty().WithMessage("O campo 'Curso' é obrigatório.");

     RuleFor(s => s. CRAA)
         .LessThanOrEqualTo(10).WithMessage("O campo 'CRAA' deve ser menor igual a 10.")
         .GreaterThanOrEqualTo(0).WithMessage("O campo 'CRAA' deve ser maior igual a 0.");
    
     RuleFor(s => s. Status)
         .NotEmpty().WithMessage("O campo 'Status' é obrigatório.");
            
   }
}
