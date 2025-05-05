using Blazored.LocalStorage;
using BlazorTienda.Models;

namespace BlazorTienda.Services
{
    public class DeliveryService
    {
        private readonly ILocalStorageService _localStorage;
        private const string StorageKey = "deliverys";
        private List<Delivery> _deliverys = new();
        private int _ultimoId = 1;

        public DeliveryService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task InicializarAsync()
        {
            _deliverys = await _localStorage.GetItemAsync<List<Delivery>>(StorageKey) ?? new List<Delivery>();

            if (_deliverys.Any())
            {
                _ultimoId = _deliverys.Max(d => d.Id) + 1;
            }
            else
            {
                await AgregarDeliveriesPorDefecto();
            }
        }

        private async Task AgregarDeliveriesPorDefecto()
        {
            _deliverys = new List<Delivery>
            {
                new Delivery { Id = _ultimoId++, Nombre = "Carlos Pérez", Telefono = "78965412", Vehiculo = "Moto", Placa = "HJK123", Estado = "Disponible" },
                new Delivery { Id = _ultimoId++, Nombre = "Lucía Gómez", Telefono = "72123456", Vehiculo = "Bicicleta", Placa = "XYZ789", Estado = "Disponible" },
                new Delivery { Id = _ultimoId++, Nombre = "Pedro López", Telefono = "76543210", Vehiculo = "Auto", Placa = "ABC456", Estado = "Disponible" },
                new Delivery { Id = _ultimoId++, Nombre = "Ana Ramírez", Telefono = "70011223", Vehiculo = "Vagoneta", Placa = "JHG321", Estado = "Disponible" },
                new Delivery { Id = _ultimoId++, Nombre = "Marco Justiniano", Telefono = "76029573", Vehiculo = "Moto", Placa = "LSF963", Estado = "Disponible" }
            };

            await GuardarCambiosAsync();
        }

        public List<Delivery> ObtenerTodos() => _deliverys;

        public async Task AgregarAsync(Delivery delivery)
        {
            delivery.Id = _ultimoId++;
            _deliverys.Add(delivery);
            await GuardarCambiosAsync();
        }

        public async Task ActualizarAsync(Delivery deliveryActualizado)
        {
            var index = _deliverys.FindIndex(d => d.Id == deliveryActualizado.Id);
            if (index != -1)
            {
                _deliverys[index] = deliveryActualizado;
                await GuardarCambiosAsync();
            }
        }

        public async Task EliminarAsync(int id)
        {
            var delivery = _deliverys.FirstOrDefault(d => d.Id == id);
            if (delivery != null)
            {
                _deliverys.Remove(delivery);
                await GuardarCambiosAsync();
            }
        }

        private async Task GuardarCambiosAsync()
        {
            await _localStorage.SetItemAsync(StorageKey, _deliverys);
        }
        public async Task<List<Delivery>> ObtenerTodosAsync()
        {
            await InicializarAsync();
            return _deliverys;
        }

        public async Task<Delivery?> ObtenerPorIdAsync(int id)
        {
            await InicializarAsync();
            return _deliverys.FirstOrDefault(d => d.Id == id);
        }
    }
}
