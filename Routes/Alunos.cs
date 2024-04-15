using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;

namespace connect_cic_api.API.Endpoints;

public static class Alunos
{
    public static void RegisterAlunosEndpoint(this IEndpointRouteBuilder routes){
        
        var AlunosRoutes = routes.MapGroup("/vagas/{id}/Alunos");
        
        AlunosRoutes.MapGet("", (ConnectCICAPIContext context) => context.Alunos.ToList());

        AlunosRoutes.MapPost("", (Aluno alunosrout, ConnectCICAPIContext context) =>
        
        {
         context.Alunos.Add(alunosrout);
         context.SaveChanges();
         return alunosrout;
        });
    }
}