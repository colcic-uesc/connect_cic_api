namespace connect_cic_api.Domain;

public class Vaga
{
    public int VagaID { get; set; }
    public required float Valor { get; set; }
    public required DateTime DataInicio { get; set; }
    public required DateTime DataFim { get; set; } 

    public string? Requisitos { get; set; }

    public string? Descricao { get; set; }
    public required string TituloProjeto { get; set; }

    public required string Status { get; set; }


}
