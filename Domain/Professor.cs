namespace connect_cic_api.Domain;

public class Professor
{
    public virtual ICollection<Vaga>? Vagas { get; set; }
}
