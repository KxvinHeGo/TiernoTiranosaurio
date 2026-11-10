// API REST de El Tierno Tiranosaurio
// Responsable: Integrante 1
// Endpoint acordado: POST /api/auth/login  ->  { "usuario": "...", "rol": "..." }

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger habilitado siempre para poder probar también en Azure durante la demo
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
