// API REST de El Tierno Tiranosaurio
// Responsable: Kevin (Integrante 1)
// Contrato completo: docs/CONTRATO_API.md

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
