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
            var vacancy = context.Vacancies.Include(v => v.Students).FirstOrDefault(v => v.VacancyID == vacancyID);
            var student = context.Students.Find(studentID);

            if (vacancy == null || student == null)
                return Results.NotFound();

            vacancy.Students ??= new List<Student>();
            vacancy.Students.Add(student);
            context.SaveChanges();
            return Results.Created($"/vacancies/{vacancyID}/students", student);
        });

        // PUT
        vacanciesRoutes.MapPut("/{id}", (ConnectCICAPIContext context, Vacancy updatedVacancy, int id) =>
        {
            var vacancy = context.Vacancies.Find(id);

            if (vacancy == null)
                return Results.NotFound();

            vacancy.Value = updatedVacancy.Value;
            vacancy.StartDate = updatedVacancy.StartDate;
            vacancy.EndDate = updatedVacancy.EndDate;
            vacancy.Requirements = updatedVacancy.Requirements;
            vacancy.Description = updatedVacancy.Description;
            vacancy.ProjectTitle = updatedVacancy.ProjectTitle;
            vacancy.Status = updatedVacancy.Status;
            context.SaveChanges();
            return Results.NoContent();
        });

        // DELETE
        vacanciesRoutes.MapDelete("/{id}", (ConnectCICAPIContext context, int id) =>
        {
            var vacancy = context.Vacancies.Find(id);
            if (vacancy == null)
                return Results.NotFound();

            context.Vacancies.Remove(vacancy);
            context.SaveChanges();
            return Results.NoContent();
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
