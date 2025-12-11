using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesEL
{
    public class ItemCarrito
    {
        public Juego JuegoSeleccionado { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioFinal { get; set; } // Por si tiene descuento

        public ItemCarrito()
        {
            Cantidad = 1;
        }
    }
}
