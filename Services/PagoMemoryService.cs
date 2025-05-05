using BlazorTienda.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorTienda.Services
{
   
    public class PagoMemoryService
    {
        private List<Pago> _pagos = new();
        private int _nextId = 1;
        private readonly IJSRuntime _jsRuntime;
        private const string LocalStorageKey = "pagosData";

        public PagoMemoryService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
            _ = LoadFromLocalStorage();
        }

        private async Task LoadFromLocalStorage()
        {
            try
            {
                var data = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", LocalStorageKey);
                if (!string.IsNullOrEmpty(data))
                {
                    _pagos = JsonSerializer.Deserialize<List<Pago>>(data) ?? new List<Pago>();
                    _nextId = _pagos.Max(p => p.Id) + 1;
                }
            }
            catch
            {
                // Si hay error, empezamos con lista vacía
                _pagos = new List<Pago>();
            }
        }

        private async Task SaveToLocalStorage()
        {
            try
            {
                var data = JsonSerializer.Serialize(_pagos);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", LocalStorageKey, data);
            }
            catch
            {
                // Ignorar errores de almacenamiento
            }
        }

        public async Task<List<Pago>> ObtenerTodosPagosAsync()
        {
            return await Task.FromResult(_pagos);
        }

        public async Task<Pago?> ObtenerPagoPorIdAsync(int id)
        {
            return await Task.FromResult(_pagos.FirstOrDefault(p => p.Id == id));
        }

        public async Task<List<Pago>> ObtenerPagosPorUsuarioAsync(int usuarioId)
        {
            return await Task.FromResult(_pagos.Where(p => p.UsuarioId == usuarioId).ToList());
        }

        public async Task<List<Pago>> ObtenerPagosPorSolicitudAsync(int solicitudId)
        {
            return await Task.FromResult(_pagos.Where(p => p.SolicitudId == solicitudId).ToList());
        }

        public async Task<Pago> RegistrarPagoAsync(Pago pago)
        {
            pago.Id = _nextId++;
            pago.FechaPago = DateTime.Now;
            _pagos.Add(pago);
            await SaveToLocalStorage();
            return pago;
        }

        public async Task ActualizarPagoAsync(Pago pago)
        {
            var index = _pagos.FindIndex(p => p.Id == pago.Id);
            if (index >= 0)
            {
                _pagos[index] = pago;
                await SaveToLocalStorage();
            }
        }

        public async Task EliminarPagoAsync(int id)
        {
            _pagos.RemoveAll(p => p.Id == id);
            await SaveToLocalStorage();
        }
        // Nuevo método para obtener pagos recientes
        public async Task<List<Pago>> ObtenerPagosRecientesAsync(DateTime desdeFecha)
        {
            return await Task.FromResult(
                _pagos.Where(p => p.FechaPago >= desdeFecha)
                      .OrderByDescending(p => p.FechaPago)
                      .ToList()
       );
        }
    }
}
