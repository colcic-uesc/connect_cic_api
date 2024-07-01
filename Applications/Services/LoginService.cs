using System.Data;
using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;
using connect_cic_api.Services.DTO;

namespace connect_cic_api.Application.Services;

public interface ILoginService
{
   UserAuthDTO? Authenticate(UserPostDTO authUser, UserRules rules, int? studentID, int? professorID);
}
public class LoginService : ILoginService
{
   private readonly IAuthManager _authManager;
   private readonly ConnectCICAPIContext _context;
   public LoginService(IAuthManager authManager, ConnectCICAPIContext context)
   {
      _authManager = authManager;
      _context = context;
   }
   public UserAuthDTO? Authenticate(UserPostDTO authUser, UserRules rules, int? studentID, int? professorID)
   {
      // caso usuario nao for admin, studentId ou professorId devem ser informados, mas nao ambos
      if (rules != UserRules.Admin && ((studentID is null && professorID is null) || (studentID is not null && professorID is not null))){
         return null;
      }

      // caso usurario seja admin, studentId e professorId devem ser nulos
      if (rules == UserRules.Admin && (studentID is not null || professorID is not null)){
         return null;
      }

      // caso usuario seja student, studentId deve ser informado
      if (rules == UserRules.Student && studentID is null){
         return null;
      }

      // caso usuario seja professor, professorId deve ser informado
      if (rules == UserRules.Professor && professorID is null){
         return null;
      }

      var _passHashed = Utils.ComputeSha256Hash(authUser.Password);

      var user = _context.Users.FirstOrDefault(u => u.Login == authUser.Login && u.Password == _passHashed);

      if (user is null)
         return null;

      string _token;
      if (professorID is not null)
         _token = _authManager.GenerateJwtToken(user.Login!, user.Rules.ToString(), professorID.ToString());
      else if (studentID is not null)
         _token = _authManager.GenerateJwtToken(user.Login!, user.Rules.ToString(), studentID.ToString());
      else
         _token = _authManager.GenerateJwtToken(user.Login!, user.Rules.ToString(), null);

      
      return new UserAuthDTO(user.Login, _token);

   }
}