namespace connect_cic_api.Domain;

public class Student
{
    public int StudentID {get; set;}
    public string? Name {get; set;}
    public string? Email {get; set;}
    public string? Course {get; set;}
    public float CRAA {get; set;}
    public string? Status {get; set;}
    public virtual ICollection<Vacancy>? Vacancies {get; set;}

    public virtual int? UserID { get; set; }
    public virtual User? User { get; set; }
}
