using connect_cic_api.Domain;
namespace connect_cic_api.API.Endpoints;
using connect_cic_api.Infra.Persistence;

public static class TipoVagas
{
    public static void RegisterUsuariosEndpoint (this IEndpointRouteBuilder routes){
        var tipoVagasRoutes = routes.MapGroup("/tipoVagas");

        tipoVagasRoutes.MapGet("", (ConnectCICAPIContext context) => context.TipoVagas.ToList());

        tipoVagasRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) => context.TipoVagas.FirstOrDefault(tv => tv.VagaTipoID == id));

        tipoVagasRoutes.MapPost("", (TipoVaga tipoVaga,ConnectCICAPIContext context) =>
        {
            context.TipoVagas.Add(tipoVaga);
            context.SaveChanges();
            return tipoVaga;
        });

        tipoVagasRoutes.MapPut("/{id}", (int id, TipoVaga tipoVaga, ConnectCICAPIContext context) =>
        {
            var tipoVagaToUpdate = context.TipoVagas.FirstOrDefault(tv => tv.VagaTipoID == id);
            
            if(tipoVagaToUpdate is not null){
                tipoVagaToUpdate.Nome = tipoVaga.Nome;
                context.SaveChanges();
            }
            
            return tipoVagaToUpdate;
        });

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

