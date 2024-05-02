using FluentValidation;
using FluentValidation.Results;
using connect_cic_api.Domain;
namespace connect_cic_api.API.Endpoints;
using connect_cic_api.Infra.Persistence;

public static class VacancyTypes
{
    public static void RegisterVacancyTypesEndpoint (this IEndpointRouteBuilder routes){
        var VacancyTypesRoutes = routes.MapGroup("/vacancy-types");

        // Gets
        // /vacancy-types - lista tipos de vagas
        // /vacancy-types/id - um tipo de vaga especifico
        VacancyTypesRoutes.MapGet("", (ConnectCICAPIContext context) => context.VacancyTypes.ToList());
        VacancyTypesRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) => context.VacancyTypes.FirstOrDefault(tv => tv.VacancyTypeID == id));

        // Posts
        // /vacancy-types - cadastra tipo de vaga
        VacancyTypesRoutes.MapPost("", async (
        IValidator<VacancyType> validator,
        VacancyType vacancyType,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = await validator.ValidateAsync(vacancyType);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            context.VacancyTypes.Add(vacancyType);
            context.SaveChanges();
            return Results.Created($"/vacancyType/{vacancyType.VacancyTypeID}", vacancyType);
        });

        // Puts
        // /vacancy-types/id - atualiza tipo de vaga
        VacancyTypesRoutes.MapPut("/{id}", (int id, VacancyType vacancyType, ConnectCICAPIContext context) =>
        {
            var VacancyTypeToUpdate = context.VacancyTypes.FirstOrDefault(tv => tv.VacancyTypeID == id);
            
            if(VacancyTypeToUpdate is not null){
                VacancyTypeToUpdate.Name = vacancyType.Name;
                context.SaveChanges();
            }
            
            return VacancyTypeToUpdate;
        });

        // Deletes
        // /vacancy-types/id - deleta tipo de vaga
        VacancyTypesRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var VacancyTypeToDelete = context.VacancyTypes.FirstOrDefault(tv => tv.VacancyTypeID == id);
            
            if(VacancyTypeToDelete is not null){
                context.VacancyTypes.Remove(VacancyTypeToDelete);
                context.SaveChanges();
            }
            
            return VacancyTypeToDelete;
        });
    }   

}

