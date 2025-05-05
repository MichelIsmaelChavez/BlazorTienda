using Microsoft.JSInterop;
using BlazorTienda.Models;

namespace BlazorTienda.Services
{
    public class EmailService
    {
        private readonly IJSRuntime _jsRuntime;
        private readonly SesionService _sesionService;

        public EmailService(IJSRuntime jsRuntime, SesionService sesionService)
        {
            _jsRuntime = jsRuntime;
            _sesionService = sesionService;
        }

        public async Task EnviarFactura(Entrega entrega)
        {
            try
            {
                Console.WriteLine("=== Iniciando envío de correo ===");
                
                // Verificar que el usuario actual existe
                if (_sesionService.UsuarioActual == null)
                {
                    throw new Exception("No hay usuario autenticado");
                }

                // Usar el correo del usuario actual
                var correoDestino = _sesionService.UsuarioActual.Correo;
                var nombreDestino = _sesionService.UsuarioActual.Nombre;

                Console.WriteLine($"Correo del usuario actual: {correoDestino}");
                Console.WriteLine($"Nombre del usuario actual: {nombreDestino}");

                // Formatear la tabla de productos sin etiquetas HTML
                var productosHtml = string.Join("\n", entrega.Productos.Select(p => 
                    $"{p.Nombre}\t{p.Cantidad}\tBs. {p.PrecioUnitario:N2}"));

                var templateParams = new
                {
                    to_email = correoDestino,
                    to_name = nombreDestino,
                    numero_factura = entrega.Id,
                    fecha = entrega.FechaCompra.ToString("dd/MM/yyyy HH:mm"),
                    cliente = nombreDestino,
                    metodo_pago = entrega.TipoPago,
                    tipo_entrega = entrega.TipoEntrega,
                    productos_html = productosHtml,
                    total = entrega.Total.ToString("N2")
                };

                Console.WriteLine("=== Parámetros del correo ===");
                Console.WriteLine($"Correo destino: {templateParams.to_email}");
                Console.WriteLine($"Nombre destino: {templateParams.to_name}");
                Console.WriteLine($"Número de factura: {templateParams.numero_factura}");
                Console.WriteLine($"Fecha: {templateParams.fecha}");
                Console.WriteLine($"Cliente: {templateParams.cliente}");
                Console.WriteLine($"Método de pago: {templateParams.metodo_pago}");
                Console.WriteLine($"Tipo de entrega: {templateParams.tipo_entrega}");
                Console.WriteLine($"Total: {templateParams.total}");

                var result = await _jsRuntime.InvokeAsync<object>(
                    "emailjs.send",
                    "service_ie75wku",
                    "template_lkoh3zb",
                    templateParams
                );

                Console.WriteLine("=== Correo enviado exitosamente ===");
                Console.WriteLine($"Resultado: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine("=== Error al enviar el correo ===");
                Console.WriteLine($"Mensaje: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                throw;
            }
        }

        private string GenerarMensajeFactura(Entrega entrega)
        {
            var mensaje = $@"
                <h2>Factura de compra #{entrega.Id}</h2>
                <p>Estimado/a {entrega.ClienteNombre},</p>
                <p>Gracias por tu compra en BlazorTienda. Aquí está el detalle de tu factura:</p>
                
                <h3>Detalles de la compra:</h3>
                <ul>
                    <li>Fecha: {entrega.FechaCompra:dd/MM/yyyy HH:mm}</li>
                    <li>Método de pago: {entrega.TipoPago}</li>
                    <li>Tipo de entrega: {entrega.TipoEntrega}</li>
                    <li>Dirección: {entrega.DireccionEntrega}</li>
                </ul>

                <h3>Productos:</h3>
                <table border='1' cellpadding='5' cellspacing='0'>
                    <tr>
                        <th>Producto</th>
                        <th>Cantidad</th>
                        <th>Precio Unitario</th>
                        <th>Subtotal</th>
                    </tr>";

            foreach (var producto in entrega.Productos)
            {
                mensaje += $@"
                    <tr>
                        <td>{producto.Nombre}</td>
                        <td>{producto.Cantidad}</td>
                        <td>Bs. {producto.PrecioUnitario:N2}</td>
                        <td>Bs. {(producto.Cantidad * producto.PrecioUnitario):N2}</td>
                    </tr>";
            }

            mensaje += $@"
                </table>
                <h3>Total: Bs. {entrega.Total:N2}</h3>
                
                <p>Si tienes alguna pregunta, no dudes en contactarnos.</p>
                <p>Saludos,<br>El equipo de BlazorTienda</p>";

            return mensaje;
        }
    }
} 