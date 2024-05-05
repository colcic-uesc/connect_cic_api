﻿using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using connect_cic_api.Domain;
namespace connect_cic_api.API.Endpoints;
using connect_cic_api.Infra.Persistence;
using connect_cic_api.Services.DTO;

public static class Users
{
    public static void RegisterUsersEndpoint (this IEndpointRouteBuilder routes){
        var UsersRoutes = routes.MapGroup("/users");

        // GETs
        // /users - lista usuarios
        // /users/id - um usuario especifico
        // /users/admins - lista usuarios admin
        // /users/admins/id - um usuario admin especifico
        // /users/students - lista usuarios estudantes
        // /users/students/id - um usuario estudante especifico
        // /users/professors - lista usuarios professores
        // /users/professors/id - um usuario professor especifico
        UsersRoutes.MapGet("", (ConnectCICAPIContext context) => Results.Ok(context.Users.ToList())).RequireAuthorization("AdminOnly");
        UsersRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) => Results.Ok(context.Users.Find(id))).RequireAuthorization("AdminOnly");
        UsersRoutes.MapGet("/admins", (ConnectCICAPIContext context) => Results.Ok(context.Users.Where(u => u.Rules == UserRules.Admin).ToList())).RequireAuthorization("AdminOnly");
        UsersRoutes.MapGet("/admins/{id}", (int id, ConnectCICAPIContext context) => Results.Ok(context.Users.FirstOrDefault(u => u.UserID == id && u.Rules == UserRules.Admin))).RequireAuthorization("AdminOnly");
        UsersRoutes.MapGet("/students", (ConnectCICAPIContext context) => Results.Ok(context.Users.Where(u => u.Rules == UserRules.Student).ToList())).RequireAuthorization("AdminOnly").RequireAuthorization("AdminOnly");
        UsersRoutes.MapGet("/students/{id}", (int id, ConnectCICAPIContext context) => Results.Ok(context.Users.FirstOrDefault(u => u.UserID == id && u.Rules == UserRules.Student))).RequireAuthorization("AdminOrStudent");
        UsersRoutes.MapGet("/professors", (ConnectCICAPIContext context) => Results.Ok(context.Users.Where(u => u.Rules == UserRules.Professor).ToList())).RequireAuthorization("AdminOnly");
        UsersRoutes.MapGet("/professors/{id}", (int id, ConnectCICAPIContext context) => Results.Ok(context.Users.FirstOrDefault(u => u.UserID == id && u.Rules == UserRules.Professor))).RequireAuthorization("AdminOrProfessor");

        // POSTs
        // /users/admins - cadastra usuario admin
        // /users/students - cadastra usuario estudante
        // /users/professors - cadastra usuario professor
        UsersRoutes.MapPost("/admins", async (
        IValidator<UserPostDTO> validator,
        [FromBody] UserPostDTO userPost,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(userPost);

            if (!validationResult.IsValid)
            {
               return Results.ValidationProblem(validationResult.ToDictionary());
            }

            if (userPost.Rules != UserRules.Admin){
                return Results.BadRequest("Usuario deve ser um admin");
            }

            var user = new User(userPost.Login, userPost.Password, userPost.Rules);

            context.Users.Add(user);
            context.SaveChanges();
            return Results.Created($"/{user.UserID}",user);
        }).RequireAuthorization("AdminOnly");


        UsersRoutes.MapPost("/students", async (
        IValidator<UserPostDTO> validator,
        [FromBody] UserPostDTO userPost,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(userPost);

            if (!validationResult.IsValid)
            {
               return Results.ValidationProblem(validationResult.ToDictionary());
            }

            if (userPost.Rules != UserRules.Student){
                return Results.BadRequest("Usuario deve ser um estudante");
            }

            var Student = context.Students.FirstOrDefault(s => s.StudentID == userPost.StudentID);

            if (Student == null){
                return Results.NotFound("Estudante não encontrado");
            }

            var user = new User(userPost.Login, userPost.Password, userPost.Rules);
            user.StudentID = userPost.StudentID;
    

            context.Users.Add(user);
            context.SaveChanges();
            return Results.Created($"/{user.UserID}",user);
        });


        UsersRoutes.MapPost("/profesors", async (
        IValidator<UserPostDTO> validator,
        [FromBody] UserPostDTO userPost,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(userPost);

            if (!validationResult.IsValid)
            {
               return Results.ValidationProblem(validationResult.ToDictionary());
            }

            if (userPost.Rules != UserRules.Professor){
                return Results.BadRequest("Usuario deve ser um professor");
            }

            var Professor = context.Professors.FirstOrDefault(s => s.ProfessorID == userPost.ProfessorID);

            if (Professor == null){
                return Results.NotFound("Professor não encontrado");
            }

            var user = new User(userPost.Login, userPost.Password, userPost.Rules);
            user.ProfessorID = Professor.ProfessorID;

            context.Users.Add(user);
            context.SaveChanges();
            return Results.Created($"/{user.UserID}",user);
        });

        // PUTs
        // /users/id - atualiza usuario
        // /users/professors/id - atualiza usuario professor
        // /users/students/id - atualiza usuario estudante
        UsersRoutes.MapPut("/{id}", (
        int id,
        [FromBody] UserPostDTO user,
        ConnectCICAPIContext context) =>
        {
            var UserToUpdate = context.Users.FirstOrDefault(u => u.UserID == id);

            if (UserToUpdate == null)
            {
               return Results.NotFound();
            }

            if (UserToUpdate.Rules != user.Rules){
                return Results.BadRequest("Não é possivel trocar o tipo de usuario");
            }

            UserToUpdate.Update(user.Login, user.Password, user.Rules);
            context.SaveChanges();
            return Results.Ok(UserToUpdate);
        }).RequireAuthorization("AdminOnly");

        UsersRoutes.MapPut("/professors/{id}", (
        int id,
        [FromBody] UserPostDTO user,
        ConnectCICAPIContext context) =>
        {
            var Professor = context.Professors.FirstOrDefault(p => p.ProfessorID == id);

            if (Professor == null)
            {
               return Results.NotFound();
            }


            var UserToUpdate = context.Users.FirstOrDefault(u => u.UserID == Professor.UserID);

            if (UserToUpdate == null)
            {
               return Results.NotFound();
            }
            if (UserToUpdate.Rules != user.Rules){
                return Results.BadRequest("Não é possivel trocar o tipo de usuario");
            }

            UserToUpdate.Update(user.Login, user.Password, user.Rules);
            context.SaveChanges();
            return Results.Ok(UserToUpdate);
        }).RequireAuthorization("AdminOrProfessor");

        UsersRoutes.MapPut("/students/{id}", (
        int id,
        [FromBody] UserPostDTO user,
        ConnectCICAPIContext context) =>
        {
            var Student = context.Students.FirstOrDefault(s => s.StudentID == id);

            if (Student == null)
            {
               return Results.NotFound();
            }

            var UserToUpdate = context.Users.FirstOrDefault(u => u.UserID == Student.UserID);

            if (UserToUpdate == null)
            {
               return Results.NotFound();
            }
            if (UserToUpdate.Rules != user.Rules){
                return Results.BadRequest("Não é possivel trocar o tipo de usuario");
            }

            UserToUpdate.Update(user.Login, user.Password, user.Rules);
            context.SaveChanges();
            return Results.Ok(UserToUpdate);
        }).RequireAuthorization("AdminOrStudent");

        // DELETEs
        // /users/id - deleta usuario
        UsersRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var UserToDelete = context.Users.FirstOrDefault(u => u.UserID == id);
            
            if (UserToDelete == null)
            {
                return Results.NotFound();
            }

            context.Users.Remove(UserToDelete);
            context.SaveChanges();
            return Results.Ok(UserToDelete);
        }).RequireAuthorization("AdminOnly");
    }   

}

