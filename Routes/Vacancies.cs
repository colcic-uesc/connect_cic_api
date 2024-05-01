using connect_cic_api.Infra.Persistence;
using connect_cic_api.Domain;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints;


public static class Vacancies
{
    public static void RegisterVacanciesEndpoint(this IEndpointRouteBuilder routes){
        var VacanciesRoutes = routes.MapGroup("/vacancies");

        // GET
        // /vacancies - lista vagas
        // /vacancies/id - uma vaga especificas
        // /vacancies/id/students - lista alunos que estão interessados na vaga
        VacanciesRoutes.MapGet("", (ConnectCICAPIContext context) => Results.Ok(context.Vacancies.ToList()));
        VacanciesRoutes.MapGet("/{id}", (ConnectCICAPIContext context, int id) => context.Vacancies.FirstOrDefault(v => v.VacancyID == id));
        VacanciesRoutes.MapGet("/{id}/students", (ConnectCICAPIContext context, int id) => {
            var vacancy = context.Vacancies.Include(v => v.Students).FirstOrDefault(v => v.VacancyID == id);
            if (vacancy == null)
                return Results.NotFound();
            
            return Results.Ok(vacancy.Students?.ToList());
        });

        // POST
        // /vacancies/vacancyID/students/studentID - aluno se candidata a vaga
        VacanciesRoutes.MapPost("/{vacancyID}/students/{studentID}", (int vacancyID, int studentID, ConnectCICAPIContext context) =>
        {
            var selectedVacancy = context.Vacancies.Include(v => v.Students).FirstOrDefault(c => c.VacancyID == vacancyID);
            var selectedStudents = context.Students.FirstOrDefault(a => a.StudentID == studentID);

            if (selectedVacancy == null || selectedStudents == null) 
                return Results.NotFound();
            
            if (selectedVacancy.Students == null)
                selectedVacancy.Students = new List<Student>();


            selectedVacancy.Students.Add(selectedStudents);
            context.SaveChanges();
            return Results.Created($"/vacancies/{vacancyID}/students", selectedStudents);
        });


        // PUT
        // /vacancies/id - atualiza vaga especifica
        VacanciesRoutes.MapPut("/{id}", (ConnectCICAPIContext context, Vacancy vacancy, int id) => {

            var selectedVacancy = context.Vacancies.FirstOrDefault(c => c.VacancyID == id);

            if (selectedVacancy == null) 
                return Results.NotFound();
            else{
                selectedVacancy.Value = vacancy.Value;
                selectedVacancy.DataInicio = vacancy.StartDate;
                selectedVacancy.DataFim = vacancy.EndDate;
                selectedVacancy.Requisitos = vacancy.Requirements;
                selectedVacancy.Descricao = vacancy.Description;
                selectedVacancy.TituloProjeto = vacancy.ProjectTitle;
                selectedVacancy.Status = vacancy.Status;
                context.SaveChanges();
                return Results.NoContent();
            }
        });


        // DELETE
        // vacancies/id - deleta vaga especifica
        // /vacancies/vacancyID/students/studentID - aluno retira interesse na vaga
        VacanciesRoutes.MapDelete("/{id}", (ConnectCICAPIContext context, int id) => {
            var vacancy = context.Vacancies.FirstOrDefault(c => c.VacancyID == id);
            if (vacancy == null)
                return Results.NotFound();
            else{
                context.Vacancies.Remove(vacancy);
                context.SaveChanges();
                return Results.NoContent();
            }
        });

        VacanciesRoutes.MapDelete("{vacancyID}/students/{studentID}", (int vacancyID, int studentID, ConnectCICAPIContext context) =>
        {
            var vacancy = context.Vacancies.Include(v => v.Students).FirstOrDefault(c => c.VacancyID == vacancyID);
            var student = vacancy?.Students?.FirstOrDefault(a => a.StudentID == studentID);

            if (vacancy == null || student == null)
                return Results.NotFound();
            else{
                vacancy.Students?.Remove(student);
                context.SaveChanges();
                return Results.NoContent();
            }

        });
    }
}
