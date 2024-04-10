namespace connect_cic_api.Domain;

public class Usuario
{
    public int UsuarioID {get; set;}
    public required string Email {get; set;}
    public required string Senha {get; set;}
    public required string Permissao {get; set;}
}
