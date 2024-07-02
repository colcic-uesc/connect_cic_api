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

        # region GETs
        // /students - lista estudantes
        studentsRoutes.MapGet("", (ConnectCICAPIContext context) =>
        {
            var students = context.Students.ToList();
            return Results.Ok(students);
        }).RequireAuthorization("CanViewStudents");

        // /students/id - um estudante especifico
        studentsRoutes.MapGet("/{id}", (ConnectCICAPIContext context, int id) =>
        {
            var student = context.Students.Find(id);
            if (student == null)
                return Results.NotFound();
            return Results.Ok(student);
        }).RequireAuthorization("CanViewStudentDetails"); 

        // /students/id/vacancies - lista de vagas de um estudante
        studentsRoutes.MapGet("/{id}/vacancies", (ConnectCICAPIContext context, int id) =>
        {
            var student = context.Students
                .Include(s => s.Vacancies)
                .FirstOrDefault(s => s.StudentID == id);

            if (student == null)
                return Results.NotFound();

            return Results.Ok(student.Vacancies);
        }).RequireAuthorization("CanViewStudentDetails");
        # endregion

        # region POSTs
        // /students - registra um estudante
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
        # endregion

        # region PUTs
        // /students/id - atualiza um estudante
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
        }).RequireAuthorization("CanModifyStudent");
        # endregion


        # region DELETEs
        // /students/id - deleta um estudante
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
        }).RequireAuthorization("CanModifyStudent");
        # endregion
    }
}
