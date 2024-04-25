using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints
{
    public static class Professores
    {
        public static void RegisterProfessoresEndpoint(this IEndpointRouteBuilder routes)
        {
            var professoresRoutes = routes.MapGroup("professores");

            // GET: /professores - Lista todos os professores
            professoresRoutes.MapGet("", async (ConnectCICAPIContext context) =>
            {
                var professores = await context.Professores.ToListAsync();
                return Results.Ok(professores);
            });

            // GET: /professores/{id} - Retorna um professor específico
            professoresRoutes.MapGet("/{id}", async (int id, ConnectCICAPIContext context) =>
            {
                var professor = await context.Professores.FindAsync(id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(professor);
            });

            // POST: /professores - Cadastra um novo professor
            professoresRoutes.MapPost("", async (Professor professor, ConnectCICAPIContext context) =>
            {
                context.Professores.Add(professor);
                await context.SaveChangesAsync();
                return Results.Created($"/professores/{professor.ProfessorID}", professor);
            });

            // PUT: /professores/{id} - Atualiza um professor existente
            professoresRoutes.MapPut("/{id}", async (int id, Professor professorData, ConnectCICAPIContext context) =>
            {
                var professor = await context.Professores.FindAsync(id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                // Atualiza os dados do professor com os novos dados
                professor.Nome = professorData.Nome;
                professor.Email = professorData.Email;
                professor.Cargo = professorData.Cargo;

                await context.SaveChangesAsync();
                return Results.NoContent();
            });

            // DELETE: /professores/{id} - Deleta um professor
            professoresRoutes.MapDelete("/{id}", async (int id, ConnectCICAPIContext context) =>
            {
                var professor = await context.Professores.FindAsync(id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                context.Professores.Remove(professor);
                await context.SaveChangesAsync();
                return Results.NoContent();
            });
        }
    }
}
