namespace connect_cic_api.Domain;

public class Vacancy
{
    public int VacancyID { get; set; }
    public float Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; } 
    public string? Requirements { get; set; }
    public string? Description { get; set; }
    public string? ProjectTitle { get; set; }
    public string? Status { get; set; }
    public virtual ICollection<Student>? Students { get; set; } 
    public virtual Professor? Professor { get; set; }
    public int ProfessorID { get; set; }
    public virtual VacancyType? VacancyType { get; set; }
    public int VacancyTypeID { get; set; }


}
