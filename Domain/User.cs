using connect_cic_api.Application;
namespace connect_cic_api.Domain;

public class User
{
    public int UserID {get; set;}
    public string? Email {get; set;}
    public virtual Student? Student {get; set;}
    public virtual int? StudentID {get; set;}
    public virtual Professor? Professor {get; set;}
    public virtual int? ProfessorID {get; set;}
    private string _password;
    public string? Password { 
        get => _password;
        private set 
        {
            _password = value ?? throw new ArgumentNullException(nameof(Password));
            _password = Utils.ComputeSha256Hash(_password);
        }
    }
    public UserRules Rules { get; private set; }

    public User(){
        Rules = UserRules.Public;
    }

    public User(string login, string password, UserRules rules)
    {
        Create(login, password, rules);
    }
    public User(int id, string login, string password, UserRules rules): this(login, password, rules)
    {
        UserId = id;
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

    public UserRules Authenticate(string login, string password){
        if (Login == login && Password == Utils.ComputeSha256Hash(password)){
            return Rules;
        }
        return UserRules.Public;
    }
}
