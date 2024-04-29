using connect_cic_api.Infra.Persistence;
using connect_cic_api.Domain;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints;


public static class Vagas
{
    public static void RegisterVagasEndpoint(this IEndpointRouteBuilder routes){
        var VagasRoutes = routes.MapGroup("/vagas");

        // GET
        // /vagas - lista vagas
        // /vagas/id - uma vaga especificas
        // /vagas/id/alunos - lista alunos que estão interessados na vaga
        VagasRoutes.MapGet("", (ConnectCICAPIContext context) => Results.Ok(context.Vagas.ToList()));
        VagasRoutes.MapGet("/{id}", (ConnectCICAPIContext context, int id) => context.Vagas.FirstOrDefault(v => v.VagaID == id));
        VagasRoutes.MapGet("/{id}/alunos", (ConnectCICAPIContext context, int id) => {
            var vaga = context.Vagas.Include(v => v.Alunos).FirstOrDefault(v => v.VagaID == id);
            if (vaga == null)
                return Results.NotFound();
            
            return Results.Ok(vaga.Alunos?.ToList());
        });

        // POST
        // /vagas/idVaga/alunos/idAluno - aluno se candidata a vaga
        VagasRoutes.MapPost("/{idVaga}/alunos/{idAluno}", (int idVaga, int idAluno, ConnectCICAPIContext context) =>
        {
            var vagaSelecionada = context.Vagas.Include(v => v.Alunos).FirstOrDefault(c => c.VagaID == idVaga);
            var alunosSelecionado = context.Alunos.FirstOrDefault(a => a.AlunoID == idAluno);

            if (vagaSelecionada == null || alunosSelecionado == null) 
                return Results.NotFound();
            
            if (vagaSelecionada.Alunos == null)
                vagaSelecionada.Alunos = new List<Aluno>();


            vagaSelecionada.Alunos.Add(alunosSelecionado);
            context.SaveChanges();
            return Results.Created($"/vagas/{idVaga}/alunos", alunosSelecionado);
        });


        // PUT
        // /vagas/id - atualiza vaga especifica
        VagasRoutes.MapPut("/{id}", (ConnectCICAPIContext context, Vaga vaga, int id) => {

            var vagaSelecionada = context.Vagas.FirstOrDefault(c => c.VagaID == id);

            if (vagaSelecionada == null) 
                return Results.NotFound();
            else{
                vagaSelecionada.Valor = vaga.Valor;
                vagaSelecionada.DataInicio = vaga.DataInicio;
                vagaSelecionada.DataFim = vaga.DataFim;
                vagaSelecionada.Requisitos = vaga.Requisitos;
                vagaSelecionada.Descricao = vaga.Descricao;
                vagaSelecionada.TituloProjeto = vaga.TituloProjeto;
                vagaSelecionada.Status = vaga.Status;
                context.SaveChanges();
                return Results.NoContent();
            }
        });


        // DELETE
        // vagas/id - deleta vaga especifica
        // /vagas/idVaga/alunos/idAluno - aluno retira interesse na vaga
        VagasRoutes.MapDelete("/{id}", (ConnectCICAPIContext context, int id) => {
            var vaga = context.Vagas.FirstOrDefault(c => c.VagaID == id);
            if (vaga == null)
                return Results.NotFound();
            else{
                context.Vagas.Remove(vaga);
                context.SaveChanges();
                return Results.NoContent();
            }
        });

        VagasRoutes.MapDelete("{idVaga}/alunos/{idAluno}", (int idVaga, int idAluno, ConnectCICAPIContext context) =>
        {
            var vaga = context.Vagas.Include(v => v.Alunos).FirstOrDefault(c => c.VagaID == idVaga);
            var aluno = vaga?.Alunos?.FirstOrDefault(a => a.AlunoID == idAluno);

            if (vaga == null || aluno == null)
                return Results.NotFound();
            else{
                vaga.Alunos?.Remove(aluno);
                context.SaveChanges();
                return Results.NoContent();
            }

        });
    }
}
