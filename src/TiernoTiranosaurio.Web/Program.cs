// Aplicación web de El Tierno Tiranosaurio (Razor Pages)
// Responsable: Juan (Integrante 3)
// Páginas: Index, Consulta, MaestroDetalle, Vista

using TiernoTiranosaurio.Web.Servicios;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

// Cliente HTTP sin límite global: ApiCliente maneja el timeout y el respaldo (Azure -> Local)
builder.Services.AddHttpClient<ApiCliente>(client =>
{
    client.Timeout = Timeout.InfiniteTimeSpan;
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
