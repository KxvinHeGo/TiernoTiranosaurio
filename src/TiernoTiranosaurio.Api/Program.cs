// API REST de El Tierno Tiranosaurio
// Responsable: Kevin (Integrante 1)
// Contrato completo: docs/CONTRATO_API.md

using TiernoTiranosaurio.Api.Datos;

var builder = WebApplication.CreateBuilder(args);

// Configuración de cada PC (no se sube a GitHub): permite poner el nombre de SU servidor SQL
// sin tocar appsettings.json. En Azure la cadena se configura en el portal.
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Conexión a la base de datos (lee ConnectionStrings:TiernoTiranosaurio)
builder.Services.AddSingleton<BaseDatos>();

// CORS: permite que la Web (Blazor) llame a la API desde otro dominio/puerto
builder.Services.AddCors(o => o.AddDefaultPolicy(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Swagger habilitado siempre para poder probar también en Azure durante la demo
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors();
app.MapControllers();

app.Run();
