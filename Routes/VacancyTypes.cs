using FluentValidation;
using FluentValidation.Results;
using connect_cic_api.Domain;
using connect_cic_api.API.Endpoints;
using connect_cic_api.Infra.Persistence;

public static class VacancyTypes
{
    public static void RegisterVacancyTypesEndpoint(this IEndpointRouteBuilder routes)
    {
        var vacancyTypesRoutes = routes.MapGroup("/vacancy-types");

        # region GETs
        // /vacancy-types - lista tipos de vagas
        vacancyTypesRoutes.MapGet("", (ConnectCICAPIContext context) =>
        {
            return Results.Ok(context.VacancyTypes.ToList());
        });

        // /vacancy-types/id - um tipo de vaga especifico
        vacancyTypesRoutes.MapGet("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var vacancyType = context.VacancyTypes.FirstOrDefault(tv => tv.VacancyTypeID == id);
            return vacancyType != null ? Results.Ok(vacancyType) : Results.NotFound();
        });
        # endregion

        # region POSTs
        // /vacancy-types - cadastra tipo de vaga
        vacancyTypesRoutes.MapPost("", (
        IValidator<VacancyType> validator,
        VacancyType vacancyType,
        ConnectCICAPIContext context) =>
        {
            ValidationResult validationResult = validator.Validate(vacancyType);

            if (!validationResult.IsValid)
            {
                return Results.ValidationProblem(validationResult.ToDictionary());
            }

            context.VacancyTypes.Add(vacancyType);
            context.SaveChanges();
            return Results.Created($"/vacancyType/{vacancyType.VacancyTypeID}", vacancyType);
        }).RequireAuthorization("AdminOnly");
        # endregion

        # region PUTs
        // /vacancy-types/id - atualiza tipo de vaga
        vacancyTypesRoutes.MapPut("/{id}", (int id, VacancyType vacancyType, ConnectCICAPIContext context) =>
        {
            var vacancyTypeToUpdate = context.VacancyTypes.FirstOrDefault(tv => tv.VacancyTypeID == id);
            
            if (vacancyTypeToUpdate is not null)
            {
                vacancyTypeToUpdate.Name = vacancyType.Name;
                context.SaveChanges();
            }
            
            return vacancyTypeToUpdate != null ? Results.Ok(vacancyTypeToUpdate) : Results.NotFound();
        }).RequireAuthorization("AdminOnly");
        # endregion

        # region DELETEs
        // /vacancy-types/id - deleta tipo de vaga
        vacancyTypesRoutes.MapDelete("/{id}", (int id, ConnectCICAPIContext context) =>
        {
            var vacancyTypeToDelete = context.VacancyTypes.FirstOrDefault(tv => tv.VacancyTypeID == id);
            
            if (vacancyTypeToDelete is not null)
            {
                context.VacancyTypes.Remove(vacancyTypeToDelete);
                context.SaveChanges();
            }
            
            return vacancyTypeToDelete != null ? Results.NoContent() : Results.NotFound();
        }).RequireAuthorization("AdminOnly");
        # endregion
    }   
}
