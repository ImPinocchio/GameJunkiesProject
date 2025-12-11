using GameJunkiesEL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesBL
{
    public static class ServicioCarrito
    {
        // Esta lista estática guardará tus juegos mientras la app esté abierta
        private static List<ItemCarrito> listaItems = new List<ItemCarrito>();

        // Método para agregar un juego al carrito
        public static void AgregarJuego(Juego juego, decimal precioUnitario)
        {
            // 1. Verificamos si el juego ya existe en el carrito
            var itemExistente = listaItems.FirstOrDefault(i => i.JuegoSeleccionado.Id == juego.Id);

            if (itemExistente != null)
            {
                // Si ya existe, solo sumamos la cantidad
                itemExistente.Cantidad++;
            }
            else
            {
                // Si no existe, creamos uno nuevo
                ItemCarrito nuevoItem = new ItemCarrito
                {
                    JuegoSeleccionado = juego,
                    Cantidad = 1,
                    PrecioFinal = precioUnitario
                };
                listaItems.Add(nuevoItem);
            }
        }

        // Método para obtener la lista actual (para mostrarla luego en el Form)
        public static List<ItemCarrito> ObtenerCarrito()
        {
            return listaItems;
        }

        // Método para calcular el total a pagar
        public static decimal CalcularTotal()
        {
            // Suma (Precio * Cantidad) de todos los items
            return listaItems.Sum(item => item.PrecioFinal * item.Cantidad);
        }

        // Método para vaciar el carrito (al terminar compra)
        public static void VaciarCarrito()
        {
            listaItems.Clear();
        }

        // Método para contar cuántos items únicos hay (para el ícono del carrito)
        public static int ContarItems()
        {
            return listaItems.Sum(x => x.Cantidad);
        }
    }
}
