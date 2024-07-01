using Microsoft.AspNetCore.Mvc;
using connect_cic_api.Application.Services;
using connect_cic_api.Services.DTO;
using connect_cic_api.Infra.Persistence;
using connect_cic_api.Domain;

namespace connect_cic_api.API.Endpoints;

public static class Auth
{
   public static void RegisterAuthEndpoints(this IEndpointRouteBuilder routes)
   {
      var authRoutes = routes.MapGroup("/auth");
      authRoutes.MapPost("/login", (
         [FromBody] UserPostDTO authuser,
         ILoginService login,
         ConnectCICAPIContext context
         )  =>
         {
            var userSearch = context.Users.FirstOrDefault(u => u.Login == authuser.Login);

            if (userSearch is null)
               return Results.NotFound("Usuário não encontrado");


            var userRules = userSearch.Rules;
            var studentID = userSearch?.StudentID;
            var professorID = userSearch?.ProfessorID;

            
            if (userRules == UserRules.Admin){
               var result = login.Authenticate(authuser, userRules, null, null);
               if (result is not null)
                  return Results.Ok(result);
               else
                  return Results.BadRequest("Usuário ou senha inválidos");
            }else if (userRules == UserRules.Student){
               var result = login.Authenticate(authuser, userRules, studentID, null);
               if (result is not null)
                  return Results.Ok(result);
               else
                  return Results.BadRequest("Usuário ou senha inválidos");
            }else if (userRules == UserRules.Professor){
               var result = login.Authenticate(authuser, userRules, null, professorID);
               if (result is not null)
                  return Results.Ok(result);
               else
                  return Results.BadRequest("Usuário ou senha inválidos");
            }else{
               return Results.BadRequest("Usuário ou senha inválidos");
            }
         });
   }
}
