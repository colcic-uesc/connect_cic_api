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
        UsersRoutes.MapGet("", (ConnectCICAPIContext context) => context.Users.ToList());

        UsersRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) => context.Users.FirstOrDefault(u => u.UserID == id));

        // POSTs
        // /users - cadastra usuario
        UsersRoutes.MapPost("", async (
        IValidator<UserPostDTO> validator,
        [FromBody] UserPostDTO userPost,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(userPost);

            if (!validationResult.IsValid)
            {
               return Results.ValidationProblem(validationResult.ToDictionary());
            }

            var user = new User(userPost.Login, userPost.Password, userPost.Rules);

            context.Users.Add(user);
            context.SaveChanges();
            return Results.Created($"/{user.UserID}",user);
        });

        // PUTs
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

            UserToUpdate.Update(user.Login, user.Password, user.Rules);
            context.SaveChanges();
            return Results.Ok(UserToUpdate);
        });

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
        });
    }   

}

