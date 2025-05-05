using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BlazorTienda.Models
{
    public class Producto
    {
        public int Id { get; set; }
        
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }           // 👈 NUEVO CAMPO
        public string ImagenUrl { get; set; } = "img/placeholder.png";
        public string Categoria { get; set; } = string.Empty;

        // Agregamos la nueva propiedad
        public bool EsFavorito { get; set; } = false;
         public bool EnOferta { get; set; } = false;    
        public decimal PrecioOferta { get; set; } = 0m;

        public decimal PorcentajeDescuento => EnOferta ? 100 - (PrecioOferta * 100 / Precio) : 0;

        [JsonIgnore]
        public IBrowserFile? ImagenArchivo { get; set; }

        public int Rating { get; set; } = 4; // Valor por defecto (puede ser 0)
        public int NumeroOpiniones { get; set; } = 0; // Valor por defecto
        public int Ventas { get; set; } = 0;

    }
}



