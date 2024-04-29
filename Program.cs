using connect_cic_api.Infra.Persistence;
using connect_cic_api.Domain;
using Microsoft.EntityFrameworkCore;
using connect_cic_api.API.Endpoints;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Identity;
//using connect_cic_api.Services.Validators;
using connect_cic_api.Services.DTO;
using connect_cic_api.Application.Services;
using System.Text;
using Microsoft.AspNetCore.Hosting.Builder;

var builder = WebApplication.CreateBuilder(args);

// configuração para ignorar referências cíclicas no Json
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add AppContext to DI
builder.Services.AddDbContext<ConnectCICAPIContext>();

var app = builder.Build();


//endpoints
app.RegisterUsuariosEndpoint();
app.RegisterTipoVagasEndpoint();
app.RegisterVagasEndpoint();
app.RegisterAlunosEndpoint();
app.RegisterProfessoresEndpoint();

app.Run();
