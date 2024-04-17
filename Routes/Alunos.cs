using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;

namespace connect_cic_api.API.Endpoints;

public static class Alunos
{
    public static void RegisterAlunosEndpoint(this IEndpointRouteBuilder routes){
        
        var AlunosRoutes = routes.MapGroup("/vagas/{id}/Alunos");
        //Gets
        //  /alunos - lista alunos
        //  /alunos/id - um aluno especifico
        AlunosRoutes.MapGet("", (ConnectCICAPIContext context) => context.Alunos.ToList());

        // Posts
        // /alunos - cadastra aluno
        // /vagas/id/alunos - aluno se candidata a vaga
        AlunosRoutes.MapPost("", (Aluno alunosrout, ConnectCICAPIContext context) =>
        
        {
         context.Alunos.Add(alunosrout);
         context.SaveChanges();
         return alunosrout;
        });
    

        // Puts
        // /alunos/id - atualiza aluno
         AlunosRoutes.MapPut("/{id}", (int id, Aluno alunosrout, ConnectCICAPIContext context) =>
        
        {
         
         var AlunosToUpdate = context.Alunos.FirstOrDefault(c => c.AlunoID == id);

         AlunosToUpdate.Nome = alunosrout.Nome; //colocar o if
         context.SaveChanges();
         return AlunosToUpdate;
        });

        // Deletes
        // /alunos/id - deleta aluno
        // /vagas/idVaga/alunos/idAluno - aluno retira interesse na vaga
        AlunosRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>

        {
         var AlunosToDelete = context.Alunos.FirstOrDefault(c => c.AlunoID == id);
         context.Alunos.Remove(AlunosToDelete);
         context.SaveChanges();
         return AlunosToDelete;
        });

    }
}