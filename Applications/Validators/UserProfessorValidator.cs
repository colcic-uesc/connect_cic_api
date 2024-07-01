using FluentValidation;
using connect_cic_api.Services.DTO;

namespace connect_cic_api.Services.Validators;

public class UserProfessorValidator : AbstractValidator<UserProfessorPostDTO>{
    public UserProfessorValidator(){
        RuleFor(u => u.Login)
            .NotEmpty().WithMessage("O campo 'Login' é obrigatório.")
            .EmailAddress().WithMessage("O campo 'Login' deve ser um e-mail válido");
      
        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("O campo 'Password' é obrigatório.")
            .MinimumLength(6).WithMessage("O campo 'Password' deve ter no mínimo 6 caracteres")
            .MaximumLength(16).WithMessage("Sua senha não pode exceder 16 caracteres.")
            .Matches(@"[A-Z]+").WithMessage("Sua senha deve ter pelo menos uma letra maiúscula.")
            .Matches(@"[a-z]+").WithMessage("Sua senha deve ter pelo menos uma letra minuscula.")
            .Matches(@"[0-9]+").WithMessage("Sua senha deve ter pelo menos um número.");

        RuleFor(u => u.ProfessorID)
            .NotEmpty().WithMessage("O campo 'ProfessorID' é obrigatório.");
         
   }

}
