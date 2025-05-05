using BlazorTienda.Models;
using BlazorTienda.Services;
using Microsoft.JSInterop;
using System.Text.Json;

public class CarritoService
{
    private const string StorageKeyPrefix = "carrito_";
    private readonly IJSRuntime js;
    private readonly SesionService sesion;

    public List<ItemCarrito> Items { get; private set; } = new();
    public event Action? OnChange;

    public CarritoService(IJSRuntime js, SesionService sesion)
    {
        this.js = js;
        this.sesion = sesion;
    }

    private string StorageKey => StorageKeyPrefix + (sesion.UsuarioActual?.Correo ?? "invitado");

    private void NotificarCambio() => OnChange?.Invoke();

    public async Task InicializarAsync()
    {
        var json = await js.InvokeAsync<string>("localStorage.getItem", StorageKey);
        if (!string.IsNullOrEmpty(json))
        {
            Items = JsonSerializer.Deserialize<List<ItemCarrito>>(json)!;
        }
    }

    private async Task GuardarAsync()
    {
        var json = JsonSerializer.Serialize(Items);
        await js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
    }

    public async Task AgregarAlCarritoAsync(Producto producto)
    {
        var item = Items.FirstOrDefault(i => i.Producto.Id == producto.Id);
        if (item != null)
        {
            item.Cantidad++;
        }
        else
        {
            Items.Add(new ItemCarrito { Producto = producto, Cantidad = 1 });
        }

        await GuardarAsync();
        NotificarCambio();
    }

    public async Task ActualizarCantidadAsync(Producto producto, int nuevaCantidad)
    {
        var item = Items.FirstOrDefault(i => i.Producto.Id == producto.Id);
        if (item != null)
        {
            if (nuevaCantidad > 0)
                item.Cantidad = nuevaCantidad;
            else
                Items.Remove(item);

            await GuardarAsync();
            NotificarCambio();
        }
    }

    public async Task EliminarDelCarritoAsync(Producto producto)
    {
        var item = Items.FirstOrDefault(i => i.Producto.Id == producto.Id);
        if (item != null)
        {
            Items.Remove(item);
            await GuardarAsync();
            NotificarCambio();
        }
    }

    public async Task VaciarAsync()
    {
        Items.Clear();
        await js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
        NotificarCambio();
    }

    public decimal ObtenerSubtotal() =>
        Items.Sum(i => (i.Producto.EnOferta ? i.Producto.PrecioOferta : i.Producto.Precio) * i.Cantidad);

    public decimal ObtenerDescuentos() =>
        Items.Where(i => i.Producto.EnOferta)
             .Sum(i => (i.Producto.Precio - i.Producto.PrecioOferta) * i.Cantidad);

    public decimal ObtenerTotal() => ObtenerSubtotal();

    public int ObtenerCantidadTotal() =>
        Items.Sum(i => i.Cantidad);
}
