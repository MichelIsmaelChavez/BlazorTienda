using BlazorTienda.Models;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorTienda.Services
{
    public class EntregaService
    {
        private readonly IJSRuntime _jsRuntime;
        private const string StorageKey = "entregas";
        private List<Entrega> _entregas = new();
        private int _contador = 1;

        public EntregaService(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task InicializarAsync()
        {
            var json = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrWhiteSpace(json))
            {
                _entregas = JsonSerializer.Deserialize<List<Entrega>>(json) ?? new List<Entrega>();
                if (_entregas.Any())
                {
                    _contador = _entregas.Max(e => e.Id) + 1;
                }
            }
        }

        public async Task<List<Entrega>> ObtenerTodasAsync()
        {
            await InicializarAsync();
            return _entregas;
        }

        public async Task<Entrega?> ObtenerPorIdAsync(int id)
        {
            await InicializarAsync();
            return _entregas.FirstOrDefault(e => e.Id == id);
        }

        public async Task AgregarAsync(Entrega entrega)
        {
            await InicializarAsync();
            entrega.Id = _contador++;
            _entregas.Add(entrega);
            await GuardarEnLocalStorageAsync();
        }

        public async Task ActualizarAsync(Entrega entrega)
        {
            await InicializarAsync();
            var existente = _entregas.FirstOrDefault(e => e.Id == entrega.Id);
            if (existente != null)
            {
                existente.ClienteNombre = entrega.ClienteNombre;
                existente.ClienteEmail = entrega.ClienteEmail;
                existente.DireccionEntrega = entrega.DireccionEntrega;
                existente.TipoEntrega = entrega.TipoEntrega;
                existente.DeliveryId = entrega.DeliveryId;
                existente.DeliveryEmail = entrega.DeliveryEmail; // ← ESTA LÍNEA ES CLAVE
                existente.EstadoEntrega = entrega.EstadoEntrega;
                existente.Total = entrega.Total;
                existente.Productos = entrega.Productos;

                await GuardarEnLocalStorageAsync();
            }
        }



        public async Task EliminarAsync(int id)
        {
            await InicializarAsync();
            var entrega = _entregas.FirstOrDefault(e => e.Id == id);
            if (entrega != null)
            {
                _entregas.Remove(entrega);
                await GuardarEnLocalStorageAsync();
            }
        }

        private async Task GuardarEnLocalStorageAsync()
        {
            var json = JsonSerializer.Serialize(_entregas);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }

        public async Task<List<Entrega>> BuscarPorClienteAsync(string nombreCliente)
        {
            await InicializarAsync();
            return _entregas
                .Where(e => e.ClienteNombre.Contains(nombreCliente, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<List<Entrega>> ObtenerPorDeliveryAsync(int deliveryId)
        {
            await InicializarAsync();
            return _entregas
                .Where(e => e.DeliveryId == deliveryId)
                .ToList();
        }
    }
}
