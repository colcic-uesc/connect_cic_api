﻿using connect_cic_api.Domain;
namespace connect_cic_api.API.Endpoints;
using connect_cic_api.Infra.Persistence;

public static class Usuarios
{
    public static void RegisterUsuariosEndpoint (this IEndpointRouteBuilder routes){
        var usuariosRoutes = routes.MapGroup("/usuarios");

        usuariosRoutes.MapGet("", (ConnectCICAPIContext context) => context.Usuarios.ToList());

        usuariosRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) => context.Usuarios.FirstOrDefault(u => u.UsuarioID == id));

        usuariosRoutes.MapPost("", (Usuario usuario,ConnectCICAPIContext context) =>
        {
            context.Usuarios.Add(usuario);
            context.SaveChanges();
            return usuario;
        });

        usuariosRoutes.MapPut("/{id}", (int id, Usuario usuario, ConnectCICAPIContext context) =>
        {
            var usuarioToUpdate = context.Usuarios.FirstOrDefault(u => u.UsuarioID == id);
            usuarioToUpdate.Email = usuario.Email;
            usuarioToUpdate.Senha = usuario.Senha;
            usuarioToUpdate.Permissao = usuario.Permissao;
            context.SaveChanges();
            return usuarioToUpdate;
        });

        usuariosRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var usuarioToDelete = context.Usuarios.FirstOrDefault(u => u.UsuarioID == id);
            context.Usuarios.Remove(usuarioToDelete);
            context.SaveChanges();
            return usuarioToDelete;
        });
    }   

}

