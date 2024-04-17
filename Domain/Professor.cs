namespace connect_cic_api.Domain;

public class Professor
{
    public int ProfessorID { get; set; }
    public required string Nome { get; set; }
    public required string EmailContato { get; set; }
    public required string Departamento { get; set; }
    public virtual ICollection<Vaga>? Vagas { get; set; }
    public virtual Usuario? Usuario { get; set; }
    public int UsuarioID { get; set; }
}
