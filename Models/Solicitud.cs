namespace BlazorTienda.Models
{
    public class Solicitud
    {
        public int Id { get; set; }

        // Referencia al usuario que hace la solicitud
       public int UsuarioId { get; set; }
       
        public Usuario Usuario { get; set; }

        public string DocumentoCliente { get; set; } = "";
        public string TrabajoCliente { get; set; } = "";
        public string CelularCliente { get; set; } = "";
        public decimal IngresoMensual { get; set; } = 0;

        public Producto ProductoSolicitado { get; set; }

        public decimal MontoTotal { get; set; }
        public int PlanCuotas { get; set; }
        public decimal CuotaMensual { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public decimal MontoPagado { get; set; } = 0;
        public decimal MontoPendiente => MontoTotal - MontoPagado;

        public bool MostrarEnMisPagos => Estado == "Aprobado";

        // NUEVOS CAMPOS
        public DateTime FechaAprobacion { get; set; } // fecha cuando fue aprobado

        public List<ProductoSolicitud> Productos { get; set; } = new List<ProductoSolicitud>();

        public List<Pago> Pagos { get; set; } = new List<Pago>();

        // Método para calcular el monto pendiente
       
    }
}



