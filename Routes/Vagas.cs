using connect_cic_api.Infra.Persistence;
using connect_cic_api.Domain;

namespace connect_cic_api.API.Endpoints;


public static class Vagas
{
    public static void RegisterVagasEndpoint(this IEndpointRouteBuilder routes){
        var VagasRoutes = routes.MapGroup("/vagas");
        var ProfessorVagas = routes.MapGroup("/professors/{id}/vagas");

        // GET
        // /professores/id/vagas - lista as vagas de um professor
        // /vagas - lista vagas
        // /vagas/id - uma vaga especificas
        // /vagas/id/alunos - lista alunos que estão interessados na vaga
        VagasRoutes.MapGet("", (ConnectCICAPIContext context) => context.Vagas.ToList());
        VagasRoutes.MapGet("/{id}", (ConnectCICAPIContext context, int id) => context.Vagas.FirstOrDefault(v => v.VagaID == id));
        VagasRoutes.MapGet("/{id}/alunos", (ConnectCICAPIContext context, int id) => context.Vagas.FirstOrDefault(v => v.VagaID == id)?.Alunos?.ToList());

        // POST
        // /professors/id/vagas - cadastra vaga para determinado professor
        ProfessorVagas.MapPost("", (ConnectCICAPIContext context, Vaga vaga, int id) => {
            vaga.ProfessorID = id;
            context.Vagas.Add(vaga);
            context.SaveChanges();
            return Results.Created($"/vagas/{vaga.VagaID}", vaga);
        });

        // PUT
        // /professors/idProf/vagas/idVaga - atualiza vaga para determinado professor
        ProfessorVagas.MapPut("/{id}", (ConnectCICAPIContext context, Vaga vaga, int id) => {

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
        // /professors/idProf/vagas/idVaga - deleta vaga para determinado professor
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
    }
}
