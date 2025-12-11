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
        // TUS CREDENCIALES CORREGIDAS
        private const string apiKey = "f21ca8e92d514282b42e90b37e869acd";
        private const string baseUrl = "https://api.rawg.io/api/games";

        public async Task<List<Juego>> ObtenerJuegosPopulares()
        {
            try
            {
                using (HttpClient cliente = new HttpClient())
                {
                    // Nota: Agregué 'exclude_additions=true' para evitar DLCs sueltos y limpiar la lista
                    string urlFinal = $"{baseUrl}?key={apiKey}&page_size=20&ordering=-added&exclude_additions=true";

                    HttpResponseMessage respuesta = await cliente.GetAsync(urlFinal);

                    if (respuesta.IsSuccessStatusCode)
                    {
                        string json = await respuesta.Content.ReadAsStringAsync();
                        RespuestaAPI datos = JsonConvert.DeserializeObject<RespuestaAPI>(json);

                        // --- FILTRADO INTELIGENTE ---
                        // 1. Verificamos que 'datos.Results' no sea null.
                        // 2. Usamos .Where() para dejar pasar solo lo que NO sea "Adults Only".
                        // 3. GTA es "Mature", así que pasará el filtro.

                        var listaFiltrada = datos.Results
                            .Where(j => j.Esrb_Rating == null || j.Esrb_Rating.Slug != "adults-only")
                            .ToList();

                        return listaFiltrada;
                    }
                }
            }
            catch (Exception)
            {
                return new List<Juego>();
            }

            return new List<Juego>();
        }
    }
}
