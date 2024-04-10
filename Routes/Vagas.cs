namespace connect_cic_api.API.Endpoints;

public static class Vagas
{
    public static void RegisterVagasEndpoint(this IEndpointRouteBuilder routes){
        var VagasRoutes = routes.MapGroup("/vagas");
    }
}
