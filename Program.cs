using connect_cic_api.Infra.Persistence;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// configuração para ignorar referências cíclicas no Json
builder.Services.Configure<Microsoft.AspNetCore.Http.Json.JsonOptions>(options => options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add AppContext to DI
builder.Services.AddDbContext<ConnectCICAPIContext>();

var app = builder.Build();


//endpoints



app.Run();
