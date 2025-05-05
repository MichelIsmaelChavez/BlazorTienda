namespace BlazorTienda.Models
{
    public class ProductoSolicitud
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = string.Empty;
      
       public string ImagenUrl { get; set; } = "img/placeholder.png";
      
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }

        public string Sku { get; set; }
    }
}
