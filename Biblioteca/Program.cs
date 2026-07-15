using Biblioteca.Models;
using Biblioteca.Repositories;
using Biblioteca.Repositories.Interfaces;
using Biblioteca.Services;
using Biblioteca.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDefaultIdentity<Utilizator>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddRoles<IdentityRole<int>>()
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repository Wrapper
builder.Services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();

// Services
builder.Services.AddScoped<IAccountService, AccountService>();
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

// Seed roluri si cont admin
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Utilizator>>();

    string[] roluri = { "Administrator", "Cititor" };
    foreach (var rol in roluri)
    {
        if (!await roleManager.RoleExistsAsync(rol))
            await roleManager.CreateAsync(new IdentityRole<int>(rol));
    }

    var admin = await userManager.FindByEmailAsync("admin@biblioweb.ro");
    if (admin == null)
    {
        admin = new Utilizator
        {
            Nume_Complet = "Administrator",
            Email = "admin@biblioweb.ro",
            UserName = "admin@biblioweb.ro",
            Adresa = ""
        };
        await userManager.CreateAsync(admin, "Admin123!");
        await userManager.AddToRoleAsync(admin, "Administrator");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();