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

        # region GETs
        // GETs
        // /users - lista usuarios
        UsersRoutes.MapGet("", (ConnectCICAPIContext context) => Results.Ok(context.Users.ToList()))
            .RequireAuthorization("AdminOnly");
        // /users/id - um usuario especifico
        UsersRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) => Results.Ok(context.Users.Find(id)))
            .RequireAuthorization("AdminOnly");
        // /users/admins - lista usuarios admin
        UsersRoutes.MapGet("/admins", (ConnectCICAPIContext context) => Results.Ok(context.Users.Where(u => u.Rules == UserRules.Admin).ToList()))
            .RequireAuthorization("AdminOnly");
        // /users/admins/id - um usuario admin especifico
        UsersRoutes.MapGet("/admins/{id}", (int id, ConnectCICAPIContext context) => Results.Ok(context.Users.FirstOrDefault(u => u.UserID == id && u.Rules == UserRules.Admin)))
            .RequireAuthorization("AdminOnly");
        // /users/students - lista usuarios estudantes
        UsersRoutes.MapGet("/students", (ConnectCICAPIContext context) => Results.Ok(context.Users.Where(u => u.Rules == UserRules.Student).ToList()))
            .RequireAuthorization("AdminOnly");
        // /users/students/id - um usuario estudante especifico
        UsersRoutes.MapGet("/students/{id}", (int id, ConnectCICAPIContext context) => {
            var UserSearch = context.Users.FirstOrDefault(u => u.StudentID == id && u.Rules == UserRules.Student);

            if (UserSearch == null){
                return Results.NotFound();
            }else{
                return Results.Ok(UserSearch);
            }
        }).RequireAuthorization("AdminOrStudent");
        // /users/professors - lista usuarios professores
        UsersRoutes.MapGet("/professors", (ConnectCICAPIContext context) => Results.Ok(context.Users.Where(u => u.Rules == UserRules.Professor).ToList()))
            .RequireAuthorization("AdminOnly");
        // /users/professors/id - um usuario professor especifico
        UsersRoutes.MapGet("/professors/{id}", (int id, ConnectCICAPIContext context) =>{ 
            var UserSearch = context.Users.FirstOrDefault(u => u.ProfessorID == id && u.Rules == UserRules.Professor);

            if (UserSearch == null){
                return Results.NotFound();
            }else{
                return Results.Ok(UserSearch);
            }
        }).RequireAuthorization("AdminOrProfessor");
        # endregion

        # region POSTs
        // /users/admins - cadastra usuario admin
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


            var userSearch = context.Users.FirstOrDefault(u => u.Login == userPost.Login);
            if (userSearch != null){
                return Results.BadRequest("Login já cadastrado");
            }
            
            var user = new User(userPost.Login, userPost.Password, UserRules.Admin);

            context.Users.Add(user);
            context.SaveChanges();
            return Results.Created($"/{user.UserID}",user);
        }).RequireAuthorization("AdminOnly");


        // /users/students - cadastra usuario estudante
        UsersRoutes.MapPost("/students", async (
        IValidator<UserStudentPostDTO> validator,
        [FromBody] UserStudentPostDTO userPost,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(userPost);

            if (!validationResult.IsValid)
            {
               return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var Student = context.Students.FirstOrDefault(s => s.StudentID == userPost.StudentID);

            if (Student == null){
                return Results.NotFound("Estudante não encontrado");
            }

            // verifica se o estudante já possui um usuario
            var userSearch = context.Users.FirstOrDefault(u => u.StudentID == userPost.StudentID);
            if (userSearch != null){
                return Results.BadRequest("Estudante já cadastrado.");
            }

            // Verifica se login já existe
            userSearch = context.Users.FirstOrDefault(u => u.Login == userPost.Login);
            if (userSearch != null){
                return Results.BadRequest("Login já cadastrado");
            }

            using var transaction = context.Database.BeginTransaction();

            try{
                var user = new User(userPost.Login, userPost.Password, UserRules.Student);
                user.StudentID = userPost.StudentID;
                context.Users.Add(user);
                context.SaveChanges();

                Student.UserID = user.UserID;
                context.SaveChanges();

                transaction.Commit();
                return Results.Created($"/{user.UserID}",user);
            }catch (Exception e){
                transaction.Rollback();
                return Results.BadRequest(e.Message);
            }
        });


        // /users/professors - cadastra usuario professor
        UsersRoutes.MapPost("/professors", async (
        IValidator<UserProfessorPostDTO> validator,
        [FromBody] UserProfessorPostDTO userPost,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(userPost);

            if (!validationResult.IsValid)
            {
               return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var Professor = context.Professors.FirstOrDefault(s => s.ProfessorID == userPost.ProfessorID);

            if (Professor == null){
                return Results.NotFound("Professor não encontrado");
            }

            // verifica se o estudante já possui um usuario
            var userSearch = context.Users.FirstOrDefault(u => u.ProfessorID == userPost.ProfessorID);
            if (userSearch != null){
                return Results.BadRequest("Professor já cadastrado.");
            }

            userSearch = context.Users.FirstOrDefault(u => u.Login == userPost.Login);
            if (userSearch != null){
                return Results.BadRequest("Usuario já existe");
            }

            using var transaction = context.Database.BeginTransaction();

            try{
                var user = new User(userPost.Login, userPost.Password, UserRules.Professor);
                user.ProfessorID = Professor.ProfessorID;
                context.Users.Add(user);
                context.SaveChanges();

                Professor.UserID = user.UserID;
                context.SaveChanges();

                transaction.Commit();
                return Results.Created($"/{user.UserID}",user);
            }catch (Exception e){
                transaction.Rollback();
                return Results.BadRequest(e.Message);
            }  
        });
        #endregion

        # region PUTs
        // /users/id - atualiza usuario
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

            var userSearch = context.Users.FirstOrDefault(u => u.Login == user.Login);
            if (userSearch != null){
                return Results.BadRequest("Login já cadastrado");
            }


            UserToUpdate.Update(user.Login, user.Password, UserToUpdate.Rules);
            context.SaveChanges();
            return Results.Ok(UserToUpdate);
        }).RequireAuthorization("AdminOnly");

        // /users/professors/id - atualiza usuario professor
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


            var UserToUpdate = context.Users.FirstOrDefault(u => u.ProfessorID == Professor.ProfessorID);

            if (UserToUpdate == null)
            {
               return Results.NotFound();
            }

            var userSearch = context.Users.FirstOrDefault(u => u.Login == user.Login);
            if (userSearch != null){
                return Results.BadRequest("Login já cadastrado");
            }

            if (UserToUpdate.Rules != UserRules.Professor){
                return Results.BadRequest("Não é possivel trocar o tipo de usuario");
            }

            UserToUpdate.Update(user.Login, user.Password, UserRules.Professor);
            context.SaveChanges();
            return Results.Ok(UserToUpdate);
        }).RequireAuthorization("AdminOrProfessor");

        // /users/students/id - atualiza usuario estudante
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

            var UserToUpdate = context.Users.FirstOrDefault(u => u.StudentID == Student.StudentID);

            if (UserToUpdate == null)
            {
               return Results.NotFound();
            }

            var userSearch = context.Users.FirstOrDefault(u => u.Login == user.Login);
            if (userSearch != null){
                return Results.BadRequest("Login já cadastrado");
            }

            if (UserToUpdate.Rules != UserRules.Student){
                return Results.BadRequest("Não é possivel trocar o tipo de usuario");
            }

            UserToUpdate.Update(user.Login, user.Password, UserRules.Student);
            context.SaveChanges();
            return Results.Ok(UserToUpdate);
        }).RequireAuthorization("AdminOrStudent");
        # endregion

        # region DELETEs
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
        # endregion
    }   

}

