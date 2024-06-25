using connect_cic_api.Application;
namespace connect_cic_api.Domain;

public class User
{
    public int UserID {get; set;}
    public string? Login {get; set;}
    public virtual Student? Student {get; set;}
    public virtual int? StudentID {get; set;}
    public virtual Professor? Professor {get; set;}
    public virtual int? ProfessorID {get; set;}
    private string? _password;
    public string? Password { 
        get => _password;
        private set 
        {
            _password = value ?? throw new ArgumentNullException(nameof(Password));
            _password = Utils.ComputeSha256Hash(_password);
        }
    }
    public UserRules Rules { get; private set; }

    public User(string login, string password, UserRules rules)
    {
        Create(login, password, rules);
    }
    public User(int id, string login, string password, UserRules rules): this(login, password, rules)
    {
        UserID = id;
    }

    public User(int id, string login, string password, UserRules rules, int? studentID, int? professorId): this(id, login, password, rules)
    {
        if (studentID != null)
            StudentID = studentID;
        else if (professorId != null)
            ProfessorID = professorId;
        else
            throw new ArgumentException("User must be a student or a professor");
    }

    public void Update(string login, string password, UserRules rules)
    {
        Create(login, password, rules);
    }

    public User Create(string login, string password, UserRules rules){
        Login = login;
        Password = password;
        Rules = rules;

        return this;
    }

}
