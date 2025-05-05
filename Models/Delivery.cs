public class Delivery
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public bool Disponible { get; set; }

    // 🔥 NUEVOS ATRIBUTOS
    public string Vehiculo { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Estado { get; set; } = "Disponible"; // Por defecto Disponible

    public string? Correo { get; set; }
    public string? Contrasenia { get; set; }
    public string Rol { get; set; } = "Delivery";

}