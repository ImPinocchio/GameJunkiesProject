using GameJunkiesEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesBL
{
    public static class ServicioBiblioteca
    {
        // Lista estática para guardar los juegos PROPIOS del usuario
        private static List<Juego> misJuegos = new List<Juego>();

        public static void AgregarJuego(Juego juego)
        {
            // Evitamos duplicados: Solo agregamos si no lo tiene ya
            if (!misJuegos.Any(j => j.Id == juego.Id))
            {
                misJuegos.Add(juego);
            }
        }

        public static List<Juego> ObtenerMisJuegos()
        {
            return misJuegos;
        }

        public static bool YaTengoEsteJuego(int idJuego)
        {
            return misJuegos.Any(j => j.Id == idJuego);
        }
    }
}
