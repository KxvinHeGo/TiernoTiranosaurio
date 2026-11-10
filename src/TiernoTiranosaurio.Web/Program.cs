// Aplicación web de El Tierno Tiranosaurio (Razor Pages)
// Responsable: Integrante 3
// Páginas a construir: Consulta, MaestroDetalle, Vista

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

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
