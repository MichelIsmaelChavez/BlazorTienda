using BlazorTienda.Models;
using BlazorTienda.Pages;

namespace BlazorTienda.Services
{
    public class SolicitudService
    {
        public List<Usuario> Usuarios { get; private set; } = new();
        private List<Solicitud> solicitudes = new();
        private int nextPagoId = 1;

        // Obtener todas las solicitudes (versión síncrona)
        public List<Solicitud> ObtenerSolicitudes()
        {
            return solicitudes;
        }
        // Método que falta para resolver el error
        


        // Obtener todas las solicitudes (versión asíncrona)
        public async Task<List<Solicitud>> ObtenerTodasSolicitudesAsync()
        {
            await Task.Delay(1); // Simulación de operación asíncrona
            return solicitudes;
        }

        // Agregar nueva solicitud
        public void AgregarSolicitud(Solicitud nuevaSolicitud)
        {
            nuevaSolicitud.Id = solicitudes.Any() ? solicitudes.Max(s => s.Id) + 1 : 1;
            nuevaSolicitud.FechaSolicitud = DateTime.Now;
            nuevaSolicitud.Estado = "Pendiente";
            nuevaSolicitud.Pagos = new List<Pago>();
            solicitudes.Add(nuevaSolicitud);
        }

        // Cambiar estado de una solicitud
        public void CambiarEstado(int usuarioId, int productoId, string nuevoEstado)
        {
            var solicitud = solicitudes.FirstOrDefault(s =>
                s.UsuarioId == usuarioId && s.Productos.Any(p => p.ProductoId == productoId));

            if (solicitud != null)
            {
                solicitud.Estado = nuevoEstado;

                if (nuevoEstado == "Aprobado")
                {
                    solicitud.FechaAprobacion = DateTime.Now;
                }
            }
        }

        // Limpiar todas las solicitudes
        public void LimpiarSolicitudes()
        {
            solicitudes.Clear();
        }

        // Registrar un pago para una solicitud
        public async Task RegistrarPagoAsync(int solicitudId, Pago pago)
        {
            var solicitud = solicitudes.FirstOrDefault(s => s.Id == solicitudId);
            if (solicitud != null)
            {
                pago.Id = nextPagoId++;
                pago.FechaPago = DateTime.Now;
                solicitud.Pagos.Add(pago);
                solicitud.MontoPagado = solicitud.Pagos.Sum(p => p.Monto);
            }
            await Task.CompletedTask;
        }

       



        // Obtener solicitud por ID
        public async Task<Solicitud?> ObtenerSolicitudPorIdAsync(int id)
        {
            await Task.Delay(1); // Simulación de operación asíncrona
            return solicitudes.FirstOrDefault(s => s.Id == id);
        }

        // Obtener solicitudes por usuario ID
        public async Task<List<Solicitud>> ObtenerSolicitudesPorUsuarioAsync(int usuarioId)
        {
            await Task.Delay(100); // Simulación de operación asíncrona

            return solicitudes
                .Where(s => s.UsuarioId == usuarioId)
                .OrderByDescending(s => s.FechaSolicitud)
                .Select(s => new Solicitud
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    Usuario = s.Usuario,
                    DocumentoCliente = s.DocumentoCliente,
                    CelularCliente = s.CelularCliente,
                    TrabajoCliente = s.TrabajoCliente,
                    MontoTotal = s.MontoTotal,
                    PlanCuotas = s.PlanCuotas,
                    CuotaMensual = s.CuotaMensual,
                    FechaSolicitud = s.FechaSolicitud,
                    FechaAprobacion = s.FechaAprobacion,
                    Estado = s.Estado,
                    MontoPagado = s.Pagos.Sum(p => p.Monto),
                    Productos = s.Productos.Select(p => new ProductoSolicitud
                    {
                        ProductoId = p.ProductoId,
                        Nombre = p.Nombre,
                        ImagenUrl = p.ImagenUrl,
                        Cantidad = p.Cantidad,
                        PrecioUnitario = p.PrecioUnitario
                    }).ToList(),
                    Pagos = s.Pagos.OrderByDescending(p => p.FechaPago).ToList()
                })
                .ToList();
        }

        // Actualizar una solicitud existente
        public async Task ActualizarSolicitudAsync(Solicitud solicitudActualizada)
        {
            var solicitudExistente = solicitudes.FirstOrDefault(s => s.Id == solicitudActualizada.Id);
            if (solicitudExistente != null)
            {
                solicitudExistente.Estado = solicitudActualizada.Estado;
                solicitudExistente.FechaAprobacion = solicitudActualizada.FechaAprobacion;
                solicitudExistente.MontoPagado = solicitudActualizada.MontoPagado;
            }
            await Task.CompletedTask;
        }

        // Pagar una cuota específica
        public void PagarCuota(Solicitud solicitud, decimal montoPago)
        {
            var solicitudExistente = solicitudes.FirstOrDefault(s => s.Id == solicitud.Id);
            if (solicitudExistente != null)
            {
                solicitudExistente.MontoPagado = Math.Min(
                    solicitudExistente.MontoPagado + montoPago,
                    solicitudExistente.MontoTotal);
            }
        }
        //nueva funcion
        public async Task<List<Solicitud>> ObtenerSolicitudesPendientesRechazadas(int usuarioId)
        {
            await Task.Delay(100); // Simulación de operación asíncrona (como en tus otros métodos)

            return solicitudes
                .Where(s => s.UsuarioId == usuarioId &&
                           (s.Estado == "Pendiente" || s.Estado == "Rechazado"))
                .OrderByDescending(s => s.FechaSolicitud)
                .Select(s => new Solicitud
                {
                    Id = s.Id,
                    UsuarioId = s.UsuarioId,
                    DocumentoCliente = s.DocumentoCliente,
                    CelularCliente = s.CelularCliente,
                    TrabajoCliente = s.TrabajoCliente,
                    MontoTotal = s.MontoTotal,
                    PlanCuotas = s.PlanCuotas,
                    CuotaMensual = s.CuotaMensual,
                    FechaSolicitud = s.FechaSolicitud,
                    FechaAprobacion = s.FechaAprobacion,
                    Estado = s.Estado,
                    Productos = s.Productos.Select(p => new ProductoSolicitud
                    {
                        ProductoId = p.ProductoId,
                        Nombre = p.Nombre,
                        ImagenUrl = p.ImagenUrl,
                        Cantidad = p.Cantidad,
                        PrecioUnitario = p.PrecioUnitario
                    }).ToList()
                })
                .ToList();
        }


        // Método para eliminar una solicitud
        public async Task EliminarSolicitudAsync(int id)
        {
            var solicitud = solicitudes.FirstOrDefault(s => s.Id == id);
            if (solicitud != null)
            {
                solicitudes.Remove(solicitud);
            }
            await Task.CompletedTask;
        }
    }
}








