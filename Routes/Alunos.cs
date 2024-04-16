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
    

    
         AlunosRoutes.MapPut("/{id}", (int id, Aluno alunosrout, ConnectCICAPIContext context) =>
        
        {
         
         var AlunosToUpdate = context.Alunos.FirstOrDefault(c => c.AlunoID == id);

         AlunosToUpdate.Nome = alunosrout.Nome;
         context.SaveChanges();
         return AlunosToUpdate;
        });

        AlunosRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>

        {
         var AlunosToDelete = context.Alunos.FirstOrDefault(c => c.AlunoID == id);
         context.Alunos.Remove(AlunosToDelete);
         context.SaveChanges();
         return AlunosToDelete;
        });

    }
}