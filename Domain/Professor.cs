using connect_cic_api.Application;
namespace connect_cic_api.Domain;

public class Professor
{
    public int ProfessorID { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Department { get; set; }
    public virtual ICollection<Vacancy>? Vacancies { get; set; }
    public virtual User? User { get; set; }
    public virtual int? UserID { get; set; }

}
