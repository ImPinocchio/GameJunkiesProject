using GameJunkiesEL;
using MySql.Data.MySqlClient;
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
        // --- MÉTODO LOGIN (Versión MySQL) ---
        public Usuario Login(string email, string password)
        {
            Usuario usuarioEncontrado = null;

            using (MySqlConnection conn = Conexion.GetConexion())
            {
                try
                {
                    conn.Open();

                    // Ajustamos la query a los nombres de columna de tu tabla MySQL
                    // Tabla: Usuarios (Id, Nombre, Correo, Password, Nickname)
                    string query = "SELECT Id, Nombre, Nickname, Correo FROM Usuarios WHERE Correo = @Email AND Password = @Pass";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@Pass", password);

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                usuarioEncontrado = new Usuario();

                                // Mapeamos las columnas de la BD a tu objeto Usuario
                                // OJO: Si tu clase Usuario usa 'IdUsuario', aquí lo asignamos desde 'Id' de la BD
                                usuarioEncontrado.IdUsuario = Convert.ToInt32(reader["Id"]);
                                usuarioEncontrado.NombreCompleto = reader["Nombre"].ToString();
                                usuarioEncontrado.Nickname = reader["Nickname"].ToString();
                                usuarioEncontrado.Email = reader["Correo"].ToString();

                                // Asignamos un Rol por defecto si la tabla no tiene columna Rol
                                usuarioEncontrado.Rol = "Cliente";
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // En un proyecto real, aquí registrarías el error en un log
                    throw new Exception("Error en Login MySQL: " + ex.Message);
                }
            }
            return usuarioEncontrado;
        }

        // --- MÉTODO REGISTRAR (Versión MySQL) ---
        public int Registrar(Usuario usuario)
        {
            int filasAfectadas = 0;

            using (MySqlConnection conn = Conexion.GetConexion())
            {
                try
                {
                    conn.Open();

                    // Query ajustada a la tabla MySQL
                    string query = "INSERT INTO Usuarios (Nombre, Correo, Password, Nickname) " +
                                   "VALUES (@Nombre, @Email, @Pass, @Nick)";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nombre", usuario.NombreCompleto);
                        cmd.Parameters.AddWithValue("@Email", usuario.Email);
                        cmd.Parameters.AddWithValue("@Pass", usuario.Password);
                        cmd.Parameters.AddWithValue("@Nick", usuario.Nickname);

                        filasAfectadas = cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Registrar en MySQL: " + ex.Message);
                }
            }

            return filasAfectadas;
        }
    }
}