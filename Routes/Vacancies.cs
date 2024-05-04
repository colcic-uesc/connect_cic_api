using FluentValidation;
using FluentValidation.Results;
using connect_cic_api.Infra.Persistence;
using connect_cic_api.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace connect_cic_api.API.Endpoints;

public static class Vacancies
{
    public static void RegisterVacanciesEndpoint(this IEndpointRouteBuilder routes)
    {
        var vacanciesRoutes = routes.MapGroup("/vacancies");

        // GET
        vacanciesRoutes.MapGet("", (ConnectCICAPIContext context) =>
        {
            var vacancies = context.Vacancies.ToList();
            return Results.Ok(vacancies);
        });

        vacanciesRoutes.MapGet("/{id}", (ConnectCICAPIContext context, int id) =>
        {
            var vacancy = context.Vacancies.Find(id);
            if (vacancy == null)
                return Results.NotFound();
            return Results.Ok(vacancy);
        });

        vacanciesRoutes.MapGet("/{id}/students", (ConnectCICAPIContext context, int id) =>
        {
            var vacancy = context.Vacancies.Include(v => v.Students).FirstOrDefault(v => v.VacancyID == id);
            if (vacancy == null)
                return Results.NotFound();

            return Results.Ok(vacancy.Students?.ToList());
        });

        // POST
        vacanciesRoutes.MapPost("/{vacancyID}/students/{studentID}", (int vacancyID, int studentID, ConnectCICAPIContext context) =>
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
        vacanciesRoutes.MapPut("/{id}", (ConnectCICAPIContext context, Vacancy vacancy, int id) => {

            var selectedVacancy = context.Vacancies.FirstOrDefault(c => c.VacancyID == id);

            if (selectedVacancy == null) 
                return Results.NotFound();
            else{
                selectedVacancy.Value = vacancy.Value;
                selectedVacancy.StartDate = vacancy.StartDate;
                selectedVacancy.EndDate = vacancy.EndDate;
                selectedVacancy.Requirements = vacancy.Requirements;
                selectedVacancy.Description = vacancy.Description;
                selectedVacancy.ProjectTitle = vacancy.ProjectTitle;
                selectedVacancy.Status = vacancy.Status;
                context.SaveChanges();
                return Results.NoContent();
            }
        });


        // DELETE
        // /vacancies/id - deleta vaga especifica
        // /vacancies/vacancyID/students/studentID - aluno retira interesse na vaga
        vacanciesRoutes.MapDelete("/{id}", (ConnectCICAPIContext context, int id) => {
            var vacancy = context.Vacancies.FirstOrDefault(c => c.VacancyID == id);
            if (vacancy == null)
                return Results.NotFound();
            else{
                context.Vacancies.Remove(vacancy);
                context.SaveChanges();
                return Results.NoContent();
            }
        });

        vacanciesRoutes.MapDelete("{vacancyID}/students/{studentID}", (int vacancyID, int studentID, ConnectCICAPIContext context) =>
        {
            var vacancy = context.Vacancies.Include(v => v.Students).FirstOrDefault(v => v.VacancyID == vacancyID);
            var student = vacancy?.Students?.FirstOrDefault(s => s.StudentID == studentID);

            if (vacancy == null || student == null)
                return Results.NotFound();

            vacancy.Students?.Remove(student);
            context.SaveChanges();
            return Results.NoContent();
        });
    }
}
