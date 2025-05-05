using BlazorTienda.Models;
using Blazored.LocalStorage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorTienda.Services
{
    public class UsuarioService
    {
        // Lista inicial de usuarios
        public List<Usuario> Usuarios { get; private set; } = new()
        {
            new Usuario { Id = 1, Nombre = "Admin", Correo = "admin@tienda.com", Contrasena = "123", Rol = "Administrador" },
            new Usuario { Id = 2, Nombre = "Juan Perez", Correo = "juan@gmail.com", Contrasena = "123", Rol = "Cliente" },
            new Usuario { Id = 3, Nombre = "Carlos Vargas", Correo = "gerente@tienda.com", Contrasena = "123", Rol = "Gerente general" },
            new Usuario { Id = 4, Nombre = "Lucía Rojas", Correo = "supervisor@tienda.com", Contrasena = "123", Rol = "Supervisor general" },
            new Usuario { Id = 5, Nombre = "Ana Mendoza", Correo = "almacen@tienda.com", Contrasena = "123", Rol = "Encargado de almacén" },
            new Usuario { Id = 6, Nombre = "Mario Suárez", Correo = "entregas@tienda.com", Contrasena = "123", Rol = "Encargado de entregas" },
            new Usuario { Id = 7, Nombre = "Jose Luis", Correo = "ayudante@tienda.com", Contrasena = "123", Rol = "Ayudante de almacén" },
            new Usuario { Id = 8, Nombre = "Rosa López", Correo = "limpieza@tienda.com", Contrasena = "123", Rol = "Personal de limpieza" }
        };

        private readonly ILocalStorageService _localStorage;

        public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int usuarioId)
        {
            await CargarUsuariosDesdeLocalStorage();
            return Usuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public UsuarioService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
            _ = CargarUsuariosDesdeLocalStorage();
        }

        // Agregar un nuevo usuario
        public async Task AgregarUsuarioAsync(Usuario nuevo)
        {
            nuevo.Id = Usuarios.Any() ? Usuarios.Max(u => u.Id) + 1 : 1;
            Usuarios.Add(nuevo);
            await GuardarUsuariosEnLocalStorage();
        }

        // Editar un usuario existente
        public void EditarUsuario(Usuario editado)
        {
            var index = Usuarios.FindIndex(u => u.Id == editado.Id);
            if (index != -1)
            {
                Usuarios[index] = editado;
            }
        }

        // Eliminar un usuario por ID
        public void EliminarUsuario(int id)
        {
            var usuario = Usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
            {
                Usuarios.Remove(usuario);
            }
        }

        // Guardar usuarios en localStorage
        public async Task GuardarUsuariosEnLocalStorage()
        {
            await _localStorage.SetItemAsync("usuarios", Usuarios);
        }

        // Cargar usuarios desde localStorage
        public async Task CargarUsuariosDesdeLocalStorage()
        {
            var usuariosGuardados = await _localStorage.GetItemAsync<List<Usuario>>("usuarios");
            if (usuariosGuardados != null)
            {
                Usuarios = usuariosGuardados;
            }
        }

        // Obtener usuarios con fotos de rostro registradas
        public async Task<List<Usuario>> ObtenerUsuariosConRostro()
        {
            await CargarUsuariosDesdeLocalStorage();
            return Usuarios
                .Where(u => !string.IsNullOrEmpty(u.FotoRostro))
                .Select(u => new Usuario
                {
                    Correo = u.Correo,
                    FotoRostro = u.FotoRostro
                })
                .ToList();
        }

        // Obtener un usuario por correo
        public async Task<Usuario?> ObtenerUsuarioPorCorreoAsync(string correo)
        {
            await CargarUsuariosDesdeLocalStorage();
            return Usuarios.FirstOrDefault(u => u.Correo.Equals(correo, System.StringComparison.OrdinalIgnoreCase));
        }

        public Usuario? ObtenerUsuarioPorId(int usuarioId)
        {
            return Usuarios.FirstOrDefault(u => u.Id == usuarioId);
        }

        public async Task ActualizarUsuarioAsync(Usuario actualizado)
        {
            var index = Usuarios.FindIndex(u => u.Id == actualizado.Id);
            if (index != -1)
            {
                Usuarios[index] = actualizado;
                await GuardarUsuariosEnLocalStorage();
            }
        }
        public async Task<List<Usuario>> ObtenerTodosAsync()
        {
            await CargarUsuariosDesdeLocalStorage();
            return Usuarios;
        }
        public async Task AgregarAsync(Usuario nuevo)
        {
            nuevo.Id = Usuarios.Any() ? Usuarios.Max(u => u.Id) + 1 : 1;
            Usuarios.Add(nuevo);
            await GuardarUsuariosEnLocalStorage();
        }




    }


}


