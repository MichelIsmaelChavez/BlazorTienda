using BlazorTienda.Models;
using Microsoft.JSInterop;
using System.Text.Json;

namespace BlazorTienda.Services
{
    public class ProductoService
    {
        private const string StorageKey = "productos";
        private readonly IJSRuntime js;

        public ProductoService(IJSRuntime js)
        {
            this.js = js;
        }

        public string TextoBusqueda { get; set; } = string.Empty;

        public List<Producto> Productos { get; set; } = new();

        // Inicializa los productos desde el localStorage o agrega productos iniciales
        public async Task InicializarProductosAsync()
        {
            var json = await js.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrEmpty(json))
            {
                Productos = JsonSerializer.Deserialize<List<Producto>>(json)!;
            }
            else
            {
                Productos = ObtenerProductosIniciales();
                await GuardarEnLocalStorage();
            }
        }

        // Productos iniciales si el localStorage está vacío
        private List<Producto> ObtenerProductosIniciales()
        {
            return new List<Producto>
            {
              new Producto { Id = 1, Nombre = "Laptop Dell", Descripcion = "Potente para trabajo", Precio = 1500, PrecioOferta = 1350, EnOferta = true, Categoria = "Tecnología", ImagenUrl = "img/laptop.jpg", Stock = 10 },
              new Producto { Id = 2, Nombre = "Auriculares Sony", Descripcion = "Cancelación de ruido", Precio = 300, Categoria = "Audio", ImagenUrl = "img/auriculares.jpg", Stock = 25 },
              new Producto { Id = 3, Nombre = "Smartphone Samsung", Descripcion = "Pantalla AMOLED, 128GB", Precio = 1200, PrecioOferta = 1050, EnOferta = true, Categoria = "Tecnología", ImagenUrl = "img/smartphone.jpg", Stock = 15 },
              new Producto { Id = 4, Nombre = "Tablet Lenovo", Descripcion = "10 pulgadas, Android", Precio = 850, Categoria = "Tecnología", ImagenUrl = "img/tablet.jpg", Stock = 20 },
              new Producto { Id = 5, Nombre = "Refrigerador LG", Descripcion = "Doble puerta, no frost", Precio = 2600, PrecioOferta = 2390, EnOferta = true, Categoria = "Electrodomésticos", ImagenUrl = "img/refrigerador.jpg", Stock = 5 },
              new Producto { Id = 6, Nombre = "Microondas Samsung", Descripcion = "Grill + descongelado rápido", Precio = 750, Categoria = "Electrodomésticos", ImagenUrl = "img/microondas.jpg", Stock = 8 },
              new Producto { Id = 7, Nombre = "Campera Adidas", Descripcion = "Térmica, resistente al agua", Precio = 480, PrecioOferta = 420, EnOferta = true, Categoria = "Ropa", ImagenUrl = "img/campera.jpg", Stock = 30 },
              new Producto { Id = 8, Nombre = "Zapatillas Nike Air", Descripcion = "Modelo deportivo unisex", Precio = 620, Categoria = "Ropa", ImagenUrl = "img/zapatillas.jpg", Stock = 22 },
              new Producto { Id = 9, Nombre = "Casco Integral LS2", Descripcion = "Homologado DOT/ECE", Precio = 350, Categoria = "Moto", ImagenUrl = "img/casco.jpg", Stock = 12 },
              new Producto { Id = 10, Nombre = "Guantes Motocross", Descripcion = "Protección y agarre", Precio = 150, Categoria = "Moto", ImagenUrl = "img/guantes.jpg", Stock = 18 },
              new Producto { Id = 11, Nombre = "PlayStation 5", Descripcion = "Última generación, 825GB SSD", Precio = 4200, PrecioOferta = 3890, EnOferta = true, Categoria = "Juegos", ImagenUrl = "img/ps5.jpg", Stock = 6 },
              new Producto { Id = 12, Nombre = "Nintendo Switch OLED", Descripcion = "Edición especial Mario", Precio = 2999, Categoria = "Juegos", ImagenUrl = "img/switch.jpg", Stock = 10 },
              new Producto { Id = 13, Nombre = "Sillón Reclinable", Descripcion = "Cuero sintético, con apoyapiés", Precio = 900, Categoria = "Muebles", ImagenUrl = "img/sillon.jpg", Stock = 4 },
              new Producto { Id = 14, Nombre = "Escritorio Gamer", Descripcion = "Con LED RGB y soporte doble monitor", Precio = 1100, PrecioOferta = 980, EnOferta = true, Categoria = "Muebles", ImagenUrl = "img/escritorio.jpg", Stock = 7 }
            };
        }

        // Guarda la lista de productos en el localStorage
        private async Task GuardarEnLocalStorage()
        {
            var json = JsonSerializer.Serialize(Productos);
            await js.InvokeVoidAsync("localStorage.setItem", StorageKey, json);
        }

        // Agrega un nuevo producto
        public async Task AgregarProductoAsync(Producto nuevo)
        {
            nuevo.Id = Productos.Any() ? Productos.Max(p => p.Id) + 1 : 1;
            Productos.Add(nuevo);
            await GuardarEnLocalStorage();
        }

        // Elimina un producto por ID
        public async Task EliminarProductoAsync(int id)
        {
            var prod = Productos.FirstOrDefault(p => p.Id == id);
            if (prod != null)
            {
                Productos.Remove(prod);
                await GuardarEnLocalStorage();
            }
        }

        // Edita un producto existente
        public async Task EditarProductoAsync(Producto editado)
        {
            var index = Productos.FindIndex(p => p.Id == editado.Id);
            if (index != -1)
            {
                Productos[index] = editado;
                await GuardarEnLocalStorage();
            }
        }

        // Obtiene un producto por su ID
        public Producto ObtenerProductoPorId(int id)
        {
            return Productos.FirstOrDefault(p => p.Id == id);
        }

        // Filtra productos por categoría y texto de búsqueda
        public IEnumerable<Producto> ProductosFiltrados(string categoriaSeleccionada)
        {
            return Productos
                .Where(p =>
                    (string.IsNullOrEmpty(categoriaSeleccionada) || p.Categoria == categoriaSeleccionada) &&
                    (string.IsNullOrEmpty(TextoBusqueda) ||
                     p.Nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                     p.Descripcion.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                     p.Categoria.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase))
                );
        }

        // Obtiene productos por categoría
        public List<Producto> ObtenerProductosPorCategoria(string categoria)
        {
            return Productos.Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Obtiene productos en oferta
        public List<Producto> GetProductosEnOferta()
        {
            return Productos.Where(p => p.EnOferta).ToList();
        }
    }
}
