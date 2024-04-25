using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints;

public static class Alunos
{
    public static void RegisterAlunosEndpoint(this IEndpointRouteBuilder routes){
        
        var AlunosRoutes = routes.MapGroup("alunos");
        var VagasRoutes = routes.MapGroup("vagas/{id}");
        //Gets
        //  /alunos - lista alunos  ok
        AlunosRoutes.MapGet("", (ConnectCICAPIContext context) => context.Alunos.ToList());
        //  /alunos/id - um aluno especifico
        AlunosRoutes.MapGet("/id", (ConnectCICAPIContext context, int id) => context.Alunos.FirstOrDefault(c => c.AlunoID == id));

        // Posts
        // /alunos - cadastra aluno ok
        AlunosRoutes.MapPost("", (Aluno aluno, ConnectCICAPIContext context) =>
        {
            context.Alunos.Add(aluno);
            context.SaveChangesAsync();
            return aluno;
        });

        // /vagas/id/alunos - aluno se candidata a vaga
        VagasRoutes.MapPost("", (Aluno alunosrout, int id, ConnectCICAPIContext context) =>
        {
            var vagaSelecionada = context.Vagas.Include(v => v.Alunos).FirstOrDefault(c => c.VagaID == id);

            if (vagaSelecionada == null) 
                return Results.NotFound();
            
            if (vagaSelecionada.Alunos == null)
                vagaSelecionada.Alunos = new List<Aluno>();


            context.Alunos.Add(alunosrout);
            context.SaveChanges();
            return Results.Created($"/vagas/{id}/alunos", alunosrout);
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

        // /vagas/idVaga/alunos/idAluno - aluno retira interesse na vaga
        VagasRoutes.MapDelete("alunos/{idAluno}", (int id, int idAluno, ConnectCICAPIContext context) =>
        {
            var vagaSelecionada = context.Vagas.Include(v => v.Alunos).FirstOrDefault()
        });

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

 