using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesEL
{
    public class MensajeChat
    {
        public string Texto { get; set; }
        public bool EsUsuario { get; set; } // true = Yo, false = IA
        public string ImagenUrl { get; set; } // Opcional: Si trae foto de juego
        public Juego JuegoRelacionado { get; set; } // Para guardar datos del juego recomendado
    }
}
