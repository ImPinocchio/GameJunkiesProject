using GameJunkiesDL;
using GameJunkiesEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesBL
{
    public class UsuarioBL
    {
        // Instancia de la capa de datos
        private UsuarioDAL usuarioDal = new UsuarioDAL();

        public Usuario ValidarAcceso(string email, string password)
        {
            // REGLA DE NEGOCIO: Validar que no lleguen vacíos
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Por favor, ingrese correo y contraseña.");
            }

            // Si todo está bien, llamamos a la capa de datos
            return usuarioDal.Login(email, password);
        }
        public bool RegistrarUsuario(Usuario nuevoUsuario)
        {
            // REGLA: Validar que NINGÚN campo esté vacío, incluyendo el Nick
            if (string.IsNullOrEmpty(nuevoUsuario.NombreCompleto) ||
                string.IsNullOrEmpty(nuevoUsuario.Nickname) ||    // <--- Verifica que esté esto
                string.IsNullOrEmpty(nuevoUsuario.Email) ||
                string.IsNullOrEmpty(nuevoUsuario.Password))
            {
                throw new Exception("Debes llenar todos los campos, incluido el Nickname.");
            }

            // REGLA 2: Contraseña mínima (opcional, pero profesional)
            if (nuevoUsuario.Password.Length < 4)
            {
                throw new Exception("La contraseña debe tener al menos 4 caracteres.");
            }

            // Llamamos a la DL
            // Si devuelve 1, es que se guardó. Si devuelve 0, falló.
            return usuarioDal.Registrar(nuevoUsuario) > 0;
        }

    }

}
