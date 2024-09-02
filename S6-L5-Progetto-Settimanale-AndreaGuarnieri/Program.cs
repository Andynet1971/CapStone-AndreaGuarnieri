using Microsoft.AspNetCore.Authentication.Cookies;
using CapStone_AndreaGuarnieri.Models.Interfaces;
using CapStone_AndreaGuarnieri.Models.Services;
using CapStone_AndreaGuarnieri.DataAccess;


var builder = WebApplication.CreateBuilder(args);

// Recupera la stringa di connessione
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configura l'autenticazione tramite cookie
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
        options.SlidingExpiration = false;
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    });

// Configura i data access con la stringa di connessione utilizzando un factory method
builder.Services.AddScoped<IPrenotazione>(provider => new PrenotazioneDataAccess(connectionString));
builder.Services.AddScoped<ICamera>(provider => new CameraDataAccess(connectionString));
builder.Services.AddScoped<ICliente>(provider => new ClienteDataAccess(connectionString));
builder.Services.AddScoped<IServizioAggiuntivo>(provider => new ServizioAggiuntivoDataAccess(connectionString));
builder.Services.AddScoped<IUtente>(provider => new UtenteDataAccess(connectionString));
builder.Services.AddScoped<IServizio>(provider => new ServizioDataAccess(connectionString)); // Aggiungi questa linea

builder.Services.AddScoped<CameraService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<PrenotazioneService>();
builder.Services.AddScoped<UtenteService>();
builder.Services.AddScoped<ServizioAggiuntivoService>();
builder.Services.AddScoped<ServizioService>(); // Aggiungi questa linea

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
