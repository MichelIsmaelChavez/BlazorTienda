using Blazored.LocalStorage;
using BlazorTienda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTienda.Services
{
    public class SesionService
    {
        private readonly ILocalStorageService _localStorage;

        public Usuario? UsuarioActual { get; private set; }
        public int CarritoItems { get; set; } // Nueva propiedad

        // Evento que se dispara cuando la sesión cambia
        public event Action? OnAuthChange;

        // Constructor que inyecta ILocalStorageService
        public SesionService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        // Cargar la sesión desde LocalStorage al inicio
        public async Task CargarSesionDesdeLocalStorage()
        {
            // Intentar obtener el usuario del LocalStorage
            UsuarioActual = await _localStorage.GetItemAsync<Usuario>("usuarioActual");

            if (UsuarioActual != null)
            {
                OnAuthChange?.Invoke(); // Notifica que la sesión se ha restaurado
            }
        }

        public async Task<bool> IniciarSesionAsync(string correo, string contrasena, List<Usuario> usuarios)
        {
            var usuario = usuarios.FirstOrDefault(u =>
                u.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase) &&
                u.Contrasena == contrasena);

            if (usuario != null)
            {
                // Revisar si ya hay una versión anterior en LocalStorage
                var existente = await _localStorage.GetItemAsync<Usuario>("usuarioActual");
                if (existente != null && existente.Correo == usuario.Correo)
                {
                    // Conservar datos personalizados como FotoRostro y Ubicación
                    usuario.FotoRostro = existente.FotoRostro;
                    usuario.Latitud = existente.Latitud;
                    usuario.Longitud = existente.Longitud;
                }

                UsuarioActual = usuario;
                await _localStorage.SetItemAsync("usuarioActual", usuario); // Guardar usuario en LocalStorage
                OnAuthChange?.Invoke();
                return true;
            }

            return false;
        }


        public async Task CerrarSesionAsync()
        {
            UsuarioActual = null;
            CarritoItems = 0;
            await _localStorage.RemoveItemAsync("usuarioActual"); // Eliminar usuario del LocalStorage
            OnAuthChange?.Invoke();
        }

        public bool EstaAutenticado => UsuarioActual != null;

        public bool TieneAccesoAdmin()
        {
            var rolesPermitidos = new[] {
                "Gerente general",
                "Supervisor general",
                "Administrador",
                "Encargado de almacén",
                "Encargado de entregas"
            };

            return UsuarioActual != null && rolesPermitidos.Contains(UsuarioActual.Rol);
        }

        public async Task GuardarUsuarioActualAsync()
        {
            if (UsuarioActual != null)
            {
                await _localStorage.SetItemAsync("usuarioActual", UsuarioActual);
            }
        }

    }
}
