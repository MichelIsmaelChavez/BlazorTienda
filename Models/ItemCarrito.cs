namespace BlazorTienda.Models
{
    public class ItemCarrito
    {
        public Producto Producto { get; set; } = new();
        public int Cantidad { get; set; } = 1;
        public decimal Total => Producto != null ? Producto.Precio * Cantidad : 0;
    }
}
