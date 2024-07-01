using connect_cic_api.Domain;

namespace connect_cic_api.Services.DTO;
public record UserPostDTO(string Login, string Password);
public record UserProfessorPostDTO(string Login, string Password, int ProfessorID);
public record UserStudentPostDTO(string Login, string Password, int StudentID);


public record UserAuthDTO(string? Login, string Token);
