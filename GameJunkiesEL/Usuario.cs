using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesEL
{
    public class Usuario
    {
        // ... (Tus propiedades actuales: IdUsuario, Nombre, etc.) ...
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }

        // --- NUEVO: SESIÓN GLOBAL ---
        // Esto guardará al usuario logueado para que todos lo vean
        public static Usuario SesionActual { get; set; }
    }
}
