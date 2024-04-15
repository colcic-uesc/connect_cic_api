namespace connect_cic_api.Domain;

public class TipoVaga
{
    public int VagaTipoID {get; set;}
    public required string Nome {get; set;}
    public virtual ICollection<Vaga>? Vagas {get; set;}
}
