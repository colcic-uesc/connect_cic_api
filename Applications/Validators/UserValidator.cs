using FluentValidation;
using connect_cic_api.Services.DTO;

namespace using FluentValidation;
using connect_cic_api.Services.DTO;

namespace connect_cic_api.Services.Validators;

public class UserPostValidator : AbstractValidator<UserPostDTO>
{
   public UserPostValidator()
   {
      RuleFor(u => u.Login)
         .NotEmpty().WithMessage("O campo Login é obrigatório")
         .EmailAddress().WithMessage("O campo Login deve ser um e-mail válido");
      
      RuleFor(u => u.Password)
         .NotEmpty().WithMessage("O campo Senha é obrigatório")
         .MinimumLength(6).WithMessage("O campo Senha deve ter no mínimo 6 caracteres");

      RuleFor(u => u.Rules)
         .NotEmpty().WithMessage("O campo Regras é obrigatório");
         
   }
}.Services.Validators;

public class UserPostValidator : AbstractValidator<UserPostDTO>
{
   public UserPostValidator()
   {
      RuleFor(u => u.Login)
         .NotEmpty().WithMessage("O campo Login é obrigatório")
         .EmailAddress().WithMessage("O campo Login deve ser um e-mail válido");
      
      RuleFor(u => u.Password)
         .NotEmpty().WithMessage("O campo Senha é obrigatório")
         .MinimumLength(6).WithMessage("O campo Senha deve ter no mínimo 6 caracteres");

      RuleFor(u => u.Rules)
         .NotEmpty().WithMessage("O campo Regras é obrigatório");
         
   }
}
