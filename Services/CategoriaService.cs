using BlazorTienda.Models;

namespace BlazorTienda.Services
{
    public class CategoriaService
    {
        public List<Categoria> Categorias { get; set; } = new List<Categoria>
        {
            new Categoria { Id = 1, Nombre = "Tecnología" },
            new Categoria { Id = 2, Nombre = "Electrodomésticos" },
            new Categoria { Id = 3, Nombre = "Ropa" },
            new Categoria { Id = 4, Nombre = "Moto" },
            new Categoria { Id = 5, Nombre = "Juegos" },
            new Categoria { Id = 6, Nombre = "Muebles" }
        };

        public void AgregarCategoria(Categoria nueva)
        {
            nueva.Id = Categorias.Max(c => c.Id) + 1;
            Categorias.Add(nueva);
        }

        public bool EliminarCategoria(int id)
        {
            var categoria = Categorias.FirstOrDefault(c => c.Id == id);
            if (categoria != null)
            {
                return Categorias.Remove(categoria);
            }
            return false;
        }
    }
}
