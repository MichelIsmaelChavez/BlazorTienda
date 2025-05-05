namespace BlazorTienda.Models
{
    public class Entrega
    {
        public int Id { get; set; }
        public string ClienteNombre { get; set; }
        public string ClienteEmail { get; set; }
        public int DeliveryId { get; set; }
        public string DireccionEntrega { get; set; }
        public string TipoEntrega { get; set; } // "Domicilio" o "Tienda"
        public string EstadoEntrega { get; set; } = "Pendiente";
        public decimal Total { get; set; }
        public List<ProductoEntrega> Productos { get; set; } = new List<ProductoEntrega>();
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        // Nuevos campos relacionados al pago
        public string TipoPago { get; set; } = "efectivo"; // "efectivo" o "cuotas"
        public int PlanCuotas { get; set; } = 0; // Ej: 3, 6, 12 cuotas
        public decimal CuotaMensual { get; set; } = 0;

        public double Latitud { get; set; }
        public double Longitud { get; set; }
        public string? DeliveryEmail { get; set; }

        public List<string> HistorialEstados { get; set; } = new();

        public double LatitudDelivery { get; set; }
        public double LongitudDelivery { get; set; }
    }
}
