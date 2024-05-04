using connect_cic_api.Domain;

namespace connect_cic_api.Services.DTO;
public record UserPostDTO(string Login, string Password, UserRules Rules, int? StudentID, int? ProfessorID);


public record UserAuthDTO(string? Login, string Token);
