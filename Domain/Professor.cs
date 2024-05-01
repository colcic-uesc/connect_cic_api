namespace connect_cic_api.Domain;

public class Professor
{
    public int ProfessorID { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Department { get; set; }
    public virtual ICollection<Vacancy>? Vacancies { get; set; }
    public virtual User? User { get; set; }
    public virtual int? UserID { get; set; }
}
