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
using Microsoft.AspNetCore.Hosting.Builder;

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Chave secreta do projeto connect_cic_api"))
        };
    });

builder.Services.AddAuthorization(options =>
{
    // acessar a lista de estudantes -  admins e professores
    options.AddPolicy("CanViewStudents", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Professor")));

    // acessar detalhes de um estudante específico -  usuário autenticado
    options.AddPolicy("CanViewStudentDetails", policy =>
        policy.RequireAuthenticatedUser());

    // modificar um estudante específico -  admins e o próprio estudante
    options.AddPolicy("CanModifyStudent", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || (context.User.IsInRole("Student") && context.User.HasClaim("student_id", context.Resource.ToString()))));
    
    // cadastrar vaga 
    options.AddPolicy("CanAddVancancy", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Professor")));

    // excluir um professor / atualizar professor
    options.AddPolicy("CanModifyProfessor", policy =>
        policy.RequireAssertion(context =>
            context.User.IsInRole("Admin") || context.User.IsInRole("Professor"))); 

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
});


// Add AppContext to DI
builder.Services.AddDbContext<ConnectCICAPIContext>();

// Add Validators to DI
builder.Services.AddScoped<IValidator<UserPostDTO>, UserPostValidator>();
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
