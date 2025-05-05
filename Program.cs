using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorTienda;
using BlazorTienda.Services;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configuración base para Vercel
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Servicios que no dependen de LocalStorage
builder.Services.AddSingleton<ProductoService>();
builder.Services.AddSingleton<CategoriaService>();
builder.Services.AddSingleton<CarritoService>();
builder.Services.AddSingleton<SolicitudService>();
builder.Services.AddSingleton<EntregaService>();
builder.Services.AddScoped<DeliveryService>();

// Servicios que necesitan ILocalStorage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<SesionService>();
builder.Services.AddScoped<UsuarioService>();   
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<CarritoService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<EntregaService>();
builder.Services.AddScoped<PagoMemoryService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<EmailService>();

var host = builder.Build();

// Cargar datos persistentes desde el LocalStorage
var usuarioService = host.Services.GetRequiredService<UsuarioService>();
await usuarioService.CargarUsuariosDesdeLocalStorage();

var sesionService = host.Services.GetRequiredService<SesionService>();
await sesionService.CargarSesionDesdeLocalStorage();

await host.RunAsync();
