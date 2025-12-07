using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesDL
{
    // 'internal' para que solo la capa de Datos la vea. Seguridad ante todo.
    internal class Conexion
    {
        public static SqlConnection GetConexion()
        {
            // 1. Leemos la cadena corregida del App.config
            string cadena = ConfigurationManager.ConnectionStrings["MiConexion"].ConnectionString;

            // 2. Preparamos el objeto conexión
            SqlConnection conexion = new SqlConnection(cadena);

            // 3. Retornamos la conexión (cerrada, lista para usarse)
            return conexion;
        }
    }
}
