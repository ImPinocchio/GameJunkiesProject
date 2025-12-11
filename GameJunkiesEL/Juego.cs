using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesEL
{
    // Clase auxiliar para leer la clasificación (Ej: "Mature", "Everyone")
    public class EsrbRating
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; } // ej: "mature", "adults-only"
    }

    public class Juego
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Background_Image { get; set; }
        public double Rating { get; set; }
        public string Released { get; set; }

        // AGREGAMOS ESTO:
        public EsrbRating Esrb_Rating { get; set; }
    }

    public class RespuestaAPI
    {
        public List<Juego> Results { get; set; }
    }



}
