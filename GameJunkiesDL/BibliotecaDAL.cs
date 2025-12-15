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
        // 1. OBTENER JUEGOS (Leyendo la ruta del .exe)
        public List<Juego> ObtenerJuegosUsuario(int idUsuario)
        {
            List<Juego> misJuegos = new List<Juego>();

            using (MySqlConnection conn = Conexion.GetConexion())
            {
                try
                {
                    conn.Open();
                    // Importante: Aquí pedimos 'RutaEjecutable'
                    string query = "SELECT IdJuegoAPI, NombreJuego, ImagenURL, RutaEjecutable FROM Biblioteca WHERE IdUsuario = @IdUser";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdUser", idUsuario);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Juego juego = new Juego
                                {
                                    Id = Convert.ToInt32(reader["IdJuegoAPI"]),
                                    Name = reader["NombreJuego"].ToString(),
                                    Background_Image = reader["ImagenURL"].ToString(),
                                    Rating = 5.0,
                                    // Leemos la ruta de la base de datos (si es nulo, ponemos null)
                                    RutaEjecutable = reader["RutaEjecutable"] != DBNull.Value ? reader["RutaEjecutable"].ToString() : null
                                };
                                misJuegos.Add(juego);
                            }
                        }
                    }
                }
                catch (Exception) { /* Ignoramos errores de lectura */ }
            }
            return misJuegos;
        }

        // 2. GUARDAR LA RUTA (Para cuando encuentres el .exe en tu PC)
        public bool ActualizarRutaJuego(int idUsuario, int idJuegoAPI, string ruta)
        {
            using (MySqlConnection conn = Conexion.GetConexion())
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Biblioteca SET RutaEjecutable = @ruta WHERE IdUsuario = @IdUser AND IdJuegoAPI = @IdGame";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ruta", ruta);
                        cmd.Parameters.AddWithValue("@IdUser", idUsuario);
                        cmd.Parameters.AddWithValue("@IdGame", idJuegoAPI);

                        int filas = cmd.ExecuteNonQuery();
                        return filas > 0;
                    }
                }
                catch (Exception) { return false; }
            }
        }
    }
}