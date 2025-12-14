using GameJunkiesEL;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesDL
{
    public class BibliotecaDAL
    {
        // Método para obtener los juegos de un usuario específico
        public List<Juego> ObtenerJuegosUsuario(int idUsuario)
        {
            List<Juego> misJuegos = new List<Juego>();

            using (MySqlConnection conn = Conexion.GetConexion())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT IdJuegoAPI, NombreJuego, ImagenURL FROM Biblioteca WHERE IdUsuario = @IdUser";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdUser", idUsuario);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Convertimos la fila de MySQL en un objeto Juego para que la UI lo entienda
                                Juego juego = new Juego
                                {
                                    Id = Convert.ToInt32(reader["IdJuegoAPI"]),
                                    Name = reader["NombreJuego"].ToString(),
                                    Background_Image = reader["ImagenURL"].ToString(),
                                    Rating = 5.0 // (Opcional) Ponemos 5 estrellas porque ya lo compraste ;)
                                };
                                misJuegos.Add(juego);
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    // Si falla, devolvemos la lista vacía para no romper el programa
                }
            }
            return misJuegos;
        }
    }
}