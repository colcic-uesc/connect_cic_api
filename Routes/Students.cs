using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using connect_cic_api.Domain;
using connect_cic_api.Infra.Persistence;
using Microsoft.EntityFrameworkCore;

namespace connect_cic_api.API.Endpoints;

public static class Students
{
    public static void RegisterStudentsEndpoint(this IEndpointRouteBuilder routes){
        
        var StudentsRoutes = routes.MapGroup("/students");
        // Gets
        //  /students - lista alunos  ok
        //  /students/id - um aluno especifico
        StudentsRoutes.MapGet("", (ConnectCICAPIContext context) => context.Students.ToList());
        StudentsRoutes.MapGet("/{id}", (ConnectCICAPIContext context, int id) => context.Students.FirstOrDefault(c => c.StudentID == id));

        // Posts
        // /students - cadastra aluno ok
        StudentsRoutes.MapPost("", async (
        IValidator<Student> validator,
        [FromBody] Student student, 
        ConnectCICAPIContext context) =>
        {
            
            ValidationResult validationResult = await validator.ValidateAsync(student);

            if (!validationResult.IsValid)
            {
               return Results.ValidationProblem(validationResult.ToDictionary());
            }

            context.Students.Add(student);
            context.SaveChanges();
            return Results.Created($"/{student.UserID}", student);
        });
    

        // Puts
        // /students/id - atualiza aluno ok
        StudentsRoutes.MapPut("/{id}", (int id, Student studentsrout, ConnectCICAPIContext context) => 
        {
         
            var StudentsToUpdate = context.Students.FirstOrDefault(c => c.StudentID == id);

            if(StudentsToUpdate is not null){
            
                StudentsToUpdate.Name = studentsrout.Name; // editar o if
                StudentsToUpdate.Email = studentsrout.Email;
                StudentsToUpdate.Course = studentsrout.Course;
                StudentsToUpdate.CRAA = studentsrout.CRAA;
                StudentsToUpdate.Status = studentsrout.Status;
            
                context.SaveChanges();
            }

            return StudentsToUpdate;
        });

        // Deletes
        // /students/id - deleta aluno ok
        StudentsRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var StudentsToDelete = context.Students.FirstOrDefault(c => c.StudentID == id);

            if (StudentsToDelete == null)
                return Results.NotFound();
            else{
                context.Students.Remove(StudentsToDelete);
                context.SaveChanges();
                return Results.NoContent();
            }
        });

    }
}

 