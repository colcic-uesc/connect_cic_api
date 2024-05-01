namespace connect_cic_api.Domain;

public class Student
{
    public int StudentID {get; set;}
    public required string Name {get; set;}
    public required string Email {get; set;}
    public required string Course {get; set;}
    public required float CRAA {get; set;}
    public required string Status {get; set;}
    public virtual ICollection<Vacancy>? Vacancies {get; set;}

    public virtual int? UserID { get; set; }
    public virtual User? User { get; set; }
}
