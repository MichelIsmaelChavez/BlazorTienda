using System.ComponentModel.DataAnnotations;

namespace BlazorTienda.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Correo inválido")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string Contrasena { get; set; } = string.Empty;

        [Required(ErrorMessage = "Debe seleccionar un rol")]
        public string Rol { get; set; } = string.Empty;

        // NUEVAS PROPIEDADES DE UBICACIÓN
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        public string FotoRostro { get; set; } // Nueva propiedad para almacenar la foto en base64

        public string DocumentoCliente { get; set; } = "";
        public string TrabajoCliente { get; set; } = "";
        public string CelularCliente { get; set; } = "";
    }
}
