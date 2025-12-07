using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesEL
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }

        public string Nickname { get; set; } 

        public string Email { get; set; }
        public string Password { get; set; }
        public string Rol { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Usuario() { }
    }
}
