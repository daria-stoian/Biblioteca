using Biblioteca.Models;
using Biblioteca.Services;
using Biblioteca.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICarteRepository, CarteRepository>();
builder.Services.AddScoped<IImprumutRepository, ImprumutRepository>();
builder.Services.AddScoped<IRecenzieRepository, RecenzieRepository>();
builder.Services.AddScoped<ICategorieRepository, CategorieRepository>();
builder.Services.AddScoped<IAutorRepository, AutorRepository>();
builder.Services.AddScoped<IListaDorinteRepository, ListaDorinteRepository>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();
builder.Services.AddScoped<IEvenimentRepository, EvenimentRepository>();
builder.Services.AddScoped<IRezervareEvenimentRepository, RezervareEvenimentRepository>();

builder.Services.AddScoped<ICarteService, CarteService>();
builder.Services.AddScoped<IImprumutService, ImprumutService>();
builder.Services.AddScoped<IRecenzieService, RecenzieService>();
builder.Services.AddScoped<ICategorieService, CategorieService>();
builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddScoped<IListaDorinteService, ListaDorinteService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IEvenimentService, EvenimentService>();
builder.Services.AddScoped<IRezervareEvenimentService, RezervareEvenimentService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
