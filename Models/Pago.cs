using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorTienda.Models
{
    public class Pago
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public int SolicitudId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; } = DateTime.Now;
        public string MetodoPago { get; set; } = "Transferencia";
        public string ComprobanteUrl { get; set; } = string.Empty;
        public string Estado { get; set; } = "Procesado"; // Procesado, Pendiente, Rechazado

        // Propiedades de navegación (no se persistirán)
        [NotMapped]
        public Usuario? Usuario { get; set; }
        [NotMapped]
        public Solicitud? Solicitud { get; set; }
    }
}
