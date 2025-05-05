public class ProductoEntrega
{
    public int ProductoId { get; set; }
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string ImagenUrl { get; set; } = "img/placeholder.png";
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public bool EnOferta { get; set; }
   
}
