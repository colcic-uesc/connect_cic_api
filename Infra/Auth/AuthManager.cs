using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace connect_cic_api.Application.Services;
public interface IAuthManager
{
   public string GenerateJwtToken(string email, string role, string id);
}
public class AuthManager : IAuthManager
{


   public string GenerateJwtToken(string userName, string role, string id)
   {
      var issuer = "connect_cic_api"; //emissor do token
      var audience = "Common"; //destinatário do token
      var key = "Sei que. Voce vai querer ser. Uma de nos!"; //chave secreta do token
      //cria uma chave utilizando criptografia simétrica
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
      //cria as credenciais do token
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      

      var claims = new Claim[] { };

      if (id != null)
      {
         claims = new[]
         {
            new Claim("userName", userName),
            new Claim(ClaimTypes.Role, role),
            new Claim("id", id),
         };
      }
      else{
         claims = new[]
         {
            new Claim("userName", userName),
            new Claim(ClaimTypes.Role, role),
         };
      }

      var token = new JwtSecurityToken( //cria o token
         issuer: issuer, //emissor do token
         audience: audience, //destinatário do token
         claims: claims, //informações do usuário
         expires: DateTime.Now.AddMinutes(30), //tempo de expiração do token
         signingCredentials: credentials); //credenciais do token
      

      var tokenHandler = new JwtSecurityTokenHandler(); //cria um manipulador de token

      var stringToken = tokenHandler.WriteToken(token);

      return stringToken;
   }
}
