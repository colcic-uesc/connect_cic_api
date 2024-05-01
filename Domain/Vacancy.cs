namespace connect_cic_api.Domain;

public class Vacancy
{
    public int VacancyID { get; set; }
    public required float Value { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; } 
    public string? Requirements { get; set; }
    public string? Description { get; set; }
    public required string ProjectTitle { get; set; }
    public required string Status { get; set; }
    public virtual ICollection<Student>? Students { get; set; } 
    public virtual Professor? Professor { get; set; }
    public int ProfessorID { get; set; }
    public virtual VacancyType? VacancyType { get; set; }
    public int VacancyTypeID { get; set; }


}
