using connect_cic_api.Domain;
namespace connect_cic_api.API.Endpoints;
using connect_cic_api.Infra.Persistence;

public static class TipoVagas
{
    public static void RegisterTipoVagasEndpoint (this IEndpointRouteBuilder routes){
        var tipoVagasRoutes = routes.MapGroup("/tipos-vaga");

        // Gets
        // /tipo-vagas - lista tipos de vagas
        // /tipo-vagas/id - um tipo de vaga especifico
        tipoVagasRoutes.MapGet("", (ConnectCICAPIContext context) => context.TipoVagas.ToList());
        tipoVagasRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) => context.TipoVagas.FirstOrDefault(tv => tv.VagaTipoID == id));

        // Posts
        // /tipo-vagas - cadastra tipo de vaga
        tipoVagasRoutes.MapPost("", (TipoVaga tipoVaga,ConnectCICAPIContext context) =>
        {
            context.TipoVagas.Add(tipoVaga);
            context.SaveChanges();
            return tipoVaga;
        });

        // Puts
        // /tipo-vagas/id - atualiza tipo de vaga
        tipoVagasRoutes.MapPut("/{id}", (int id, TipoVaga tipoVaga, ConnectCICAPIContext context) =>
        {
            var tipoVagaToUpdate = context.TipoVagas.FirstOrDefault(tv => tv.VagaTipoID == id);
            
            if(tipoVagaToUpdate is not null){
                tipoVagaToUpdate.Nome = tipoVaga.Nome;
                context.SaveChanges();
            }
            
            return tipoVagaToUpdate;
        });

        // Deletes
        // /tipo-vagas/id - deleta tipo de vaga
        tipoVagasRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var tipoVagaToDelete = context.TipoVagas.FirstOrDefault(tv => tv.VagaTipoID == id);
            
            if(tipoVagaToDelete is not null){
                context.TipoVagas.Remove(tipoVagaToDelete);
                context.SaveChanges();
            }
            
            return tipoVagaToDelete;
        });
    }   

}

