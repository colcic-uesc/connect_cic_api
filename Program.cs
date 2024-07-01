using connect_cic_api.Infra.Persistence;
using connect_cic_api.Domain;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using connect_cic_api.API.Endpoints;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using connect_cic_api.Services.Validators;
using connect_cic_api.Services.DTO;
using connect_cic_api.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// configuração para ignorar referências cíclicas no Json
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "connect_cic_api",
            ValidAudience = "Common",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Sei que. Voce vai querer ser. Uma de nos!"))
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Apenas admin 
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

    // acessar a lista de estudantes -  admins e professores
    options.AddPolicy("CanViewStudents", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Professor")));

    // acessar detalhes de um estudante específico -  usuário autenticado
    options.AddPolicy("CanViewStudentDetails", policy =>
        policy.RequireAuthenticatedUser());

    // modificar um estudante específico -  admins e o próprio estudante
    options.AddPolicy("CanModifyStudent", policy => 
        policy.RequireAssertion(context => {

            if (context.User.IsInRole("Admin"))
                return true;
            
            if (context.User.IsInRole("Student")){
                var routeAlunoId = (context.Resource as HttpContext)?.Request.RouteValues["id"].ToString();
                var studentIdClaim = context.User.FindFirst("id")?.Value;
                
                return !string.IsNullOrEmpty(routeAlunoId) && routeAlunoId == studentIdClaim;
            }
            return false;
        }));
    
    // cadastrar vaga 
    options.AddPolicy("CanAddVancancy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Professor")));

    // excluir um professor / atualizar professor
    options.AddPolicy("CanModifyProfessor", policy =>
        policy.RequireAssertion(context =>
        {
            // Permite acesso se for um admin
            if (context.User.IsInRole("Admin")) 
                return true;

            // Permite acesso se for o próprio professor
            if (context.User.IsInRole("Professor"))
            {
                // Assume que a rota contém o ID do professor como 'id'
                var routeProfessorId = (context.Resource as HttpContext)?.Request.RouteValues["id"]?.ToString();
                var professorIdClaim = context.User.FindFirst("id")?.Value;

                return routeProfessorId == professorIdClaim;
            }

            return false;
        })); 

    // ver alunos interessados nas vagas
     options.AddPolicy("CanViewVacancyInterests", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Professor")));

    // atualizar vagas (conteúdo)
    options.AddPolicy("CanModifyVacancy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Professor")));

    // adicionar interesse em uma vaga ou remover
    options.AddPolicy("CanAddOrRemoveInterest", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Student")));              

    // ver dados de um aluno ou atualizar um aluno
    options.AddPolicy("AdminOrStudent", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Student")));
    
    // ver dados de um professor ou atualizar um professor
    options.AddPolicy("AdminOrProfessor", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Professor")));

});


// Add AppContext to DI
builder.Services.AddDbContext<ConnectCICAPIContext>();

// Add Validators to DI
builder.Services.AddScoped<IValidator<UserPostDTO>, UserPostValidator>();
builder.Services.AddScoped<IValidator<UserStudentPostDTO>, UserStudentValidator>();
builder.Services.AddScoped<IValidator<UserProfessorPostDTO>, UserProfessorValidator>();
builder.Services.AddScoped<IValidator<Professor>, ProfessorValidator>();
builder.Services.AddScoped<IValidator<Student>, StudentValidator>();
builder.Services.AddScoped<IValidator<Vacancy>, VacancyValidator>();
builder.Services.AddScoped<IValidator<VacancyType>, VacancyTypeValidator>();

// Add Authenticator Manager to DI
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<ILoginService, LoginService>();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthentication();
app.UseAuthorization();

//endpoints
app.RegisterUsersEndpoint();
app.RegisterVacancyTypesEndpoint();
app.RegisterVacanciesEndpoint();
app.RegisterStudentsEndpoint();
app.RegisterProfessorsEndpoint();
app.RegisterAuthEndpoints();

app.Run();
