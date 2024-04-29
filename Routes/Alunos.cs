using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints;

public static class Alunos
{
    public static void RegisterAlunosEndpoint(this IEndpointRouteBuilder routes){
        
        var AlunosRoutes = routes.MapGroup("/alunos");
        // Gets
        //  /alunos - lista alunos  ok
        //  /alunos/id - um aluno especifico
        AlunosRoutes.MapGet("", (ConnectCICAPIContext context) => context.Alunos.ToList());
        AlunosRoutes.MapGet("/{id}", (ConnectCICAPIContext context, int id) => context.Alunos.FirstOrDefault(c => c.AlunoID == id));

        // Posts
        // /alunos - cadastra aluno ok
        AlunosRoutes.MapPost("", (Aluno aluno, ConnectCICAPIContext context) =>
        {
            context.Alunos.Add(aluno);
            context.SaveChangesAsync();
            return aluno;
        });
    

        // Puts
        // /alunos/id - atualiza aluno ok
        AlunosRoutes.MapPut("/{id}", (int id, Aluno alunosrout, ConnectCICAPIContext context) => 
        {
         
            var AlunosToUpdate = context.Alunos.FirstOrDefault(c => c.AlunoID == id);

            if(AlunosToUpdate is not null){
            
                AlunosToUpdate.Nome = alunosrout.Nome; // editar o if
                AlunosToUpdate.EmailContato = alunosrout.EmailContato;
                AlunosToUpdate.Curso = alunosrout.Curso;
                AlunosToUpdate.CRAA = alunosrout.CRAA;
                AlunosToUpdate.Status = alunosrout.Status;
            
                context.SaveChanges();
            }

            return AlunosToUpdate;
        });

        // Deletes
        // /alunos/id - deleta aluno ok
        AlunosRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var AlunosToDelete = context.Alunos.FirstOrDefault(c => c.AlunoID == id);

            if (AlunosToDelete == null)
                return Results.NotFound();
            else{
                context.Alunos.Remove(AlunosToDelete);
                context.SaveChanges();
                return Results.NoContent();
            }
        });

    }
}

 