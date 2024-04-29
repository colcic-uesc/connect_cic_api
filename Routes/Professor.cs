using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints
{
    public static class Professores
    {
        public static void RegisterProfessoresEndpoint(this IEndpointRouteBuilder routes)
        {
            var professoresRoutes = routes.MapGroup("/professores");

            // GETs
            // /professores - Lista todos os professores
            // /professores/{id} - Retorna um professor específico
            // /professores/{id}/vagas - lista as vagas de um professor
            professoresRoutes.MapGet("", (ConnectCICAPIContext context) =>
            {
                var professores = context.Professores.ToList();
                return Results.Ok(professores);
            });

            professoresRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) =>
            {
                var professor = context.Professores.FirstOrDefault(p => p.ProfessorID == id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(professor);
            });
            professoresRoutes.MapGet("/{id}/vagas", (ConnectCICAPIContext context, int id) => {
                var professor = context.Professores.FirstOrDefault(p => p.ProfessorID == id);

                if (professor == null){
                    return Results.NotFound();
                }else{
                    var vagas = context.Vagas.Where(v => v.ProfessorID == id).ToList();
                    return Results.Ok(vagas);
                }
            });

            // POSTs
            // /professores - Cadastra um novo professor
            // /professors/id/vagas - cadastra vaga para determinado professor
            professoresRoutes.MapPost("", (Professor professor, ConnectCICAPIContext context) =>
            {
                context.Professores.Add(professor);
                context.SaveChanges();
                return Results.Created($"/professores/{professor.ProfessorID}", professor);
            });

            professoresRoutes.MapPost("/{id}/vagas", (ConnectCICAPIContext context, Vaga vaga, int id) => {
                vaga.ProfessorID = id;
                context.Vagas.Add(vaga);
                context.SaveChanges();
                return Results.Created($"/vagas/{vaga.VagaID}", vaga);
            });

            // PUTs
            // /professores/{id} - Atualiza um professor existente
            professoresRoutes.MapPut("/{id}",  (int id, Professor professorData, ConnectCICAPIContext context) =>
            {
                var professor = context.Professores.FirstOrDefault(p => p.ProfessorID == id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                // Atualiza os dados do professor com os novos dados
                professor.Nome = professorData.Nome;
                professor.EmailContato = professorData.EmailContato;
                professor.Departamento = professorData.Departamento;


                context.SaveChanges();
                return Results.NoContent();
            });


            // DELETE: /professores/{id} - Deleta um professor
            professoresRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
            {
                var professor = context.Professores.FirstOrDefault(p => p.ProfessorID == id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                context.Professores.Remove(professor);
                context.SaveChanges();
                return Results.NoContent();
            });
        }
    }
}
