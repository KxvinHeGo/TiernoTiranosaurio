// Aplicación web de El Tierno Tiranosaurio (Razor Pages)
// Responsable: Juan (Integrante 3)
// Páginas: Index, Consulta, MaestroDetalle, Vista

using TiernoTiranosaurio.Web.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Configuración del cliente HTTP que consume la API (local o Azure)
string urlApi = builder.Configuration["UrlApi"] ?? "https://localhost:7100/";
if (!urlApi.EndsWith('/')) urlApi += "/";

builder.Services.AddHttpClient<ApiCliente>(client =>
{
    client.BaseAddress = new Uri(urlApi);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

app.Run();
