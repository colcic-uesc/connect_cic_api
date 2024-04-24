namespace connect_cic_api.Domain;

public class Aluno
{
    public int AlunoID {get; set;}
    public required string Nome {get; set;}
    public required string Email {get; set;}
    public required string Curso {get; set;}
    public required float CRAA {get; set;}
    public required string Status {get; set;}
    public virtual ICollection<Vaga>? Vagas {get; set;}

    public int UsuarioID { get; set; }
    public virtual Usuario? Usuario { get; set; }
}
