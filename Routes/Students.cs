using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints;

public static class Students
{
    public static void RegisterStudentsEndpoint(this IEndpointRouteBuilder routes)
    {
        var studentsRoutes = routes.MapGroup("/students");

        studentsRoutes.MapGet("", (ConnectCICAPIContext context) =>
        {
            var students = context.Students.ToList();
            return Results.Ok(students);
        });

        studentsRoutes.MapGet("/{id}", (ConnectCICAPIContext context, int id) =>
        {
            var student = context.Students.Find(id);
            if (student == null)
                return Results.NotFound();
            return Results.Ok(student);
        });

        studentsRoutes.MapPost("", (
        IValidator<Student> validator,
        [FromBody] Student student,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = validator.Validate(student);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            context.Students.Add(student);
            context.SaveChanges();
            return Results.Created($"/students/{student.UserID}", student);
        });

        studentsRoutes.MapPut("/{id}", (int id, Student updatedStudent, IValidator<Student> validator, ConnectCICAPIContext context) =>
        {
            var studentToUpdate = context.Students.Find(id);

            if (studentToUpdate == null)
            {
                return Results.NotFound();
            }

            studentToUpdate.Name = updatedStudent.Name;
            studentToUpdate.Email = updatedStudent.Email;
            studentToUpdate.Course = updatedStudent.Course;
            studentToUpdate.CRAA = updatedStudent.CRAA;
            studentToUpdate.Status = updatedStudent.Status;

            ValidationResult validationResult = validator.Validate(studentToUpdate);
            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            context.SaveChanges();
            return Results.Ok(studentToUpdate);
        });

        studentsRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var studentToDelete = context.Students.Find(id);

            if (studentToDelete == null)
            {
                return Results.NotFound();
            }

            context.Students.Remove(studentToDelete);
            context.SaveChanges();
            return Results.NoContent();
        });
    }
}
