namespace connect_cic_api.Services.DTO;

public class ProfessorDTO
{   
    public int ProfessorID { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Department { get; set; }
   
}
