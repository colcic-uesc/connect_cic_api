namespace connect_cic_api.Domain;

public class Professor
{
    public int ProfessorID { get; set; }
    public virtual ICollection<Vaga>? Vagas { get; set; }
}
