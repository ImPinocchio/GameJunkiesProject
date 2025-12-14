using GameJunkiesEL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesDL
{
    public class TransaccionDAL
    {
        public bool RegistrarCompra(Usuario usuario, List<ItemCarrito> items, decimal total, string metodoPago)
        {
            using (MySqlConnection conn = Conexion.GetConexion())
            {
                conn.Open();
                // Iniciamos una TRANSACCIÓN: O se guarda todo, o no se guarda nada (por seguridad)
                MySqlTransaction transaccion = conn.BeginTransaction();

                try
                {
                    // 1. GUARDAR LA VENTA
                    string queryVenta = "INSERT INTO Ventas (IdUsuario, Total, MetodoPago) VALUES (@IdUser, @Total, @Pago)";
                    using (MySqlCommand cmdVenta = new MySqlCommand(queryVenta, conn, transaccion))
                    {
                        cmdVenta.Parameters.AddWithValue("@IdUser", usuario.IdUsuario);
                        cmdVenta.Parameters.AddWithValue("@Total", total);
                        cmdVenta.Parameters.AddWithValue("@Pago", metodoPago);
                        cmdVenta.ExecuteNonQuery();
                    }

                    // 2. GUARDAR CADA JUEGO EN LA BIBLIOTECA
                    string queryBiblio = "INSERT INTO Biblioteca (IdUsuario, IdJuegoAPI, NombreJuego, ImagenURL) VALUES (@IdUser, @IdAPI, @Nombre, @Img)";

                    foreach (var item in items)
                    {
                        using (MySqlCommand cmdBiblio = new MySqlCommand(queryBiblio, conn, transaccion))
                        {
                            cmdBiblio.Parameters.AddWithValue("@IdUser", usuario.IdUsuario);
                            cmdBiblio.Parameters.AddWithValue("@IdAPI", item.JuegoSeleccionado.Id);
                            cmdBiblio.Parameters.AddWithValue("@Nombre", item.JuegoSeleccionado.Name);
                            cmdBiblio.Parameters.AddWithValue("@Img", item.JuegoSeleccionado.Background_Image);
                            cmdBiblio.ExecuteNonQuery();
                        }
                    }

                    // Si todo salió bien, confirmamos los cambios
                    transaccion.Commit();
                    return true;
                }
                catch (Exception)
                {
                    // Si algo falló, deshacemos todo para no dejar datos corruptos
                    transaccion.Rollback();
                    throw;
                }
            }
        }
    }
}