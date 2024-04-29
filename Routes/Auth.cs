using Microsoft.AspNetCore.Mvc;
using connect_cic_api.API.ViewModels;
using connect_cic_api.Application.Services;
using connect_cic_api.Services.DTO;

namespace connect_cic_api.API.Endpoints;


public static class Auth
{
   public static void RegisterAuthEndpoints(this IEndpointRouteBuilder routes)
   {
      var authRoutes = routes.MapGroup("/auth");
      authRoutes.MapPost("/login", (
         [FromBody] UserPostDTO authuser,
         ILoginService login)  =>
         {
            var result = login.Authenticate(authuser);

            if (result is not null)
               return Results.Ok(result);
            else
               return Results.BadRequest("Usuário ou senha inválidos");
         });
   }
}
