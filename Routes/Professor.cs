using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using connect_cic_api.Domain;
using connect_cic_api.Services.DTO;
using connect_cic_api.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints
{
    public static class Professors
    {
        public static void RegisterProfessorsEndpoint(this IEndpointRouteBuilder routes)
        {
            var ProfessorsRoutes = routes.MapGroup("/professors");

            # region GETs
            // /professors - Lista todos os professores
            ProfessorsRoutes.MapGet("", (ConnectCICAPIContext context) =>
            {
                var professors = context.Professors.ToList();
                return Results.Ok(professors);
            });

            // /professors/{id} - Retorna um professor específico
            ProfessorsRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) =>
            {
                var professor = context.Professors.FirstOrDefault(p => p.ProfessorID == id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(professor);
            });

            // /professors/{id}/vagas - lista as vagas de um professor
            ProfessorsRoutes.MapGet("/{id}/vacancies", (ConnectCICAPIContext context, int id) => {
                var professor = context.Professors.FirstOrDefault(p => p.ProfessorID == id);

                if (professor == null){
                    return Results.NotFound();
                }else{
                    var vacancies = context.Vacancies.Where(v => v.ProfessorID == id).ToList();
                    return Results.Ok(vacancies);
                }
            });
            # endregion

            # region POSTs
            // /professors - Cadastra um novo professor
            ProfessorsRoutes.MapPost("", async (
            [FromBody] Professor professor, 
            IValidator<Professor> validator,
            ConnectCICAPIContext context) =>
            {
                ValidationResult validationResult = await validator.ValidateAsync(professor);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                context.Professors.Add(professor);
                context.SaveChanges();
                return Results.Created($"/professors/{professor.ProfessorID}", professor);
            });

            // /professors/id/vacancies - cadastra vaga para determinado professor
            ProfessorsRoutes.MapPost("/{id}/vacancies", async (
            IValidator<Vacancy> validator,
            ConnectCICAPIContext context, 
            [FromBody] Vacancy vacancy, 
            int id) => {
               
                ValidationResult validationResult = await validator.ValidateAsync(vacancy);

                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }

                vacancy.ProfessorID = id;
                context.Vacancies.Add(vacancy);
                context.SaveChanges();
                return Results.Created($"/vacancies/{vacancy.VacancyID}", vacancy);
            }).RequireAuthorization("CanAddVacancy");
            # endregion

            # region PUTs
            // /professors/{id} - Atualiza um professor existente
            ProfessorsRoutes.MapPut("/{id}",  (int id, Professor professorData, ConnectCICAPIContext context) =>
            {
                var professor = context.Professors.FirstOrDefault(p => p.ProfessorID == id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                // Atualiza os dados do professor com os novos dados
                professor.Name = professorData.Name;
                professor.Email = professorData.Email;
                professor.Department = professorData.Department;


                context.SaveChanges();
                return Results.NoContent();
            }).RequireAuthorization("CanModifyProfessor");
            # endregion

            # region DELETEs
            // /professors/{id} - Deleta um professor
            ProfessorsRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
            {
                var professor = context.Professors.FirstOrDefault(p => p.ProfessorID == id);

                if (professor == null)
                {
                    return Results.NotFound();
                }

                context.Professors.Remove(professor);
                context.SaveChanges();
                return Results.NoContent();
            }).RequireAuthorization("CanModifyProfessor");
            #endregion
        }
    }
}
