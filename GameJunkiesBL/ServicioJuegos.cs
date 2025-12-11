using GameJunkiesEL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesBL
{
    public class ServicioJuegos
    {
        private const string apiKey = "f21ca8e92d514282b42e90b37e869acd";
        private const string baseUrl = "https://api.rawg.io/api/games";

        public async Task<List<Juego>> ObtenerJuegosPopulares(int pagina)
        {
            try
            {
                using (HttpClient cliente = new HttpClient())
                {
                    string urlFinal = $"{baseUrl}?key={apiKey}&page={pagina}&page_size=24&ordering=-added&exclude_additions=true";
                    HttpResponseMessage respuesta = await cliente.GetAsync(urlFinal);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string json = await respuesta.Content.ReadAsStringAsync();
                        RespuestaAPI datos = JsonConvert.DeserializeObject<RespuestaAPI>(json);

                        return datos.Results
                            .Where(j => j.Esrb_Rating == null || j.Esrb_Rating.Slug != "adults-only")
                            .ToList();
                    }
                }
            }
            catch { return new List<Juego>(); }
            return new List<Juego>();
        }

        public async Task<List<Juego>> BuscarJuegos(string textoBusqueda)
        {
            try
            {
                using (HttpClient cliente = new HttpClient())
                {
                    string urlFinal = $"{baseUrl}?key={apiKey}&search={textoBusqueda}&page_size=24&exclude_additions=true";
                    HttpResponseMessage respuesta = await cliente.GetAsync(urlFinal);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string json = await respuesta.Content.ReadAsStringAsync();
                        RespuestaAPI datos = JsonConvert.DeserializeObject<RespuestaAPI>(json);

                        return datos.Results
                            .Where(j => j.Esrb_Rating == null || j.Esrb_Rating.Slug != "adults-only")
                            .ToList();
                    }
                }
            }
            catch { return new List<Juego>(); }
            return new List<Juego>();
        }

        // --- NUEVO MÉTODO: OBTENER DETALLE POR ID ---
        public async Task<Juego> ObtenerDetalleJuego(int idJuego)
        {
            try
            {
                using (HttpClient cliente = new HttpClient())
                {
                    // La URL cambia: baseUrl + /ID
                    string urlFinal = $"{baseUrl}/{idJuego}?key={apiKey}";
                    HttpResponseMessage respuesta = await cliente.GetAsync(urlFinal);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string json = await respuesta.Content.ReadAsStringAsync();
                        // Deserializamos un SOLO objeto Juego, no una lista
                        Juego juegoDetalle = JsonConvert.DeserializeObject<Juego>(json);
                        return juegoDetalle;
                    }
                }
            }
            catch { return null; }
            return null;
        }
    }
}