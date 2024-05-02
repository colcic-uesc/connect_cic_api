namespace connect_cic_api.Domain;

public class VacancyType
{
    public int VacancyTypeID {get; set;}
    public string? Name {get; set;}
    public virtual ICollection<Vacancy>? Vacancies {get; set;}
}
