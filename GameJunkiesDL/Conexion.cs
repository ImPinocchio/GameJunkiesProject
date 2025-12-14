using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient; // Asegúrate de tener el paquete MySql.Data instalado

namespace GameJunkiesDL
{
    public class Conexion
    {
        // Cambiamos SqlConnection por MySqlConnection
        public static MySqlConnection GetConexion()
        {
            // 1. Leemos la NUEVA cadena "MiConexionMySQL" del App.config
            // (Asegúrate que el nombre coincida con lo que pusiste en el XML)
            string cadena = ConfigurationManager.ConnectionStrings["MiConexionMySQL"].ConnectionString;

            // 2. Preparamos el objeto conexión de MySQL
            MySqlConnection conexion = new MySqlConnection(cadena);

            // 3. Retornamos la conexión
            return conexion;
        }
    }
}