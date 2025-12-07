using GameJunkiesEL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesDL
{
    public class UsuarioDAL
    {
        // --- MÉTODO LOGIN (Ya corregido con Nickname) ---
        public Usuario Login(string email, string password)
        {
            Usuario usuarioEncontrado = null;

            using (SqlConnection conn = Conexion.GetConexion())
            {
                conn.Open();

                // Agregamos Nickname al SELECT
                string query = "SELECT IdUsuario, NombreCompleto, Nickname, Email, Rol FROM Usuarios WHERE Email = @Email AND Password = @Pass";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Pass", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            usuarioEncontrado = new Usuario();
                            usuarioEncontrado.IdUsuario = Convert.ToInt32(reader["IdUsuario"]);
                            usuarioEncontrado.NombreCompleto = reader["NombreCompleto"].ToString();

                            // LEEMOS EL NICKNAME
                            usuarioEncontrado.Nickname = reader["Nickname"].ToString();

                            usuarioEncontrado.Email = reader["Email"].ToString();
                            usuarioEncontrado.Rol = reader["Rol"].ToString();
                        }
                    }
                }
            }
            return usuarioEncontrado;
        }

        // --- MÉTODO REGISTRAR (Aquí estaba el error) ---
        public int Registrar(Usuario usuario)
        {
            int filasAfectadas = 0;

            // ESTO ES LO QUE FALTABA: Abrir la conexión antes de usarla
            using (SqlConnection conn = Conexion.GetConexion())
            {
                conn.Open();

                // Query con Nickname
                string query = "INSERT INTO Usuarios (NombreCompleto, Nickname, Email, Password, Rol) " +
                               "VALUES (@Nombre, @Nick, @Email, @Pass, 'Cliente')";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", usuario.NombreCompleto);
                    cmd.Parameters.AddWithValue("@Nick", usuario.Nickname); // Guardamos el nick
                    cmd.Parameters.AddWithValue("@Email", usuario.Email);
                    cmd.Parameters.AddWithValue("@Pass", usuario.Password);

                    // Ejecutamos el comando y guardamos el resultado
                    filasAfectadas = cmd.ExecuteNonQuery();
                }
            }

            // Devolvemos cuántas filas se guardaron (1 = Éxito)
            return filasAfectadas;
        }
    }
}
