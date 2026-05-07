using Biblioteca.Models;
using Biblioteca.Services;
using Biblioteca.Services.Interfaces;
using Biblioteca.Repositories;
using Biblioteca.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllersWithViews();

// Repository Wrapper
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

// Services
builder.Services.AddScoped<ICarteService, CarteService>();
builder.Services.AddScoped<IImprumutService, ImprumutService>();
builder.Services.AddScoped<IRecenzieService, RecenzieService>();
builder.Services.AddScoped<ICategorieService, CategorieService>();
builder.Services.AddScoped<IAutorService, AutorService>();
builder.Services.AddScoped<IListaDorinteService, ListaDorinteService>();
builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IEvenimentService, EvenimentService>();
builder.Services.AddScoped<IRezervareEvenimentService, RezervareEvenimentService>();
builder.Services.AddScoped<IUtilizatorService, UtilizatorService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
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