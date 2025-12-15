using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesEL
{
    public class EsrbRating
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
    }

    public class Juego
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Background_Image { get; set; }
        public double Rating { get; set; }
        public string Released { get; set; }

        public EsrbRating Esrb_Rating { get; set; }

        // RAWG nos da 'description' (HTML) y 'description_raw' (Texto limpio).
        public string Description_Raw { get; set; }

        // --- NUEVO: Aquí guardaremos la ruta del archivo .exe en tu PC ---
        public string RutaEjecutable { get; set; }
    }

    public class RespuestaAPI
    {
        public List<Juego> Results { get; set; }
    }
}