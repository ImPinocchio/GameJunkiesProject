using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GameJunkiesBL
{
    public class ServicioIA
    {
        // Tu clave actual (la que funciona)
        private const string apiKey = "AIzaSyBDitzmb1iasBAZ2OA74NadJ2VdBX1HZJs";

        // VOLVEMOS AL 1.5 FLASH (El más estable y con mayor cuota gratis)
        private const string apiUrl = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";

        public async Task<string> EnviarMensaje(string mensajeUsuario)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = $"{apiUrl}?key={apiKey}";

                    string promptSistema = "Eres JunkieBot, experto en videojuegos. Respuestas breves. " +
                                           "Si recomiendas un juego, escribe: [JUEGO: Nombre Exacto].";

                    var body = new
                    {
                        contents = new[] { new { parts = new[] { new { text = promptSistema + "\n\nUsuario: " + mensajeUsuario } } } }
                    };

                    string jsonBody = JsonConvert.SerializeObject(body);
                    HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(url, content);
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        JObject datos = JObject.Parse(jsonResponse);
                        // Navegación segura por el JSON para evitar errores si la estructura varía
                        string respuestaIA = datos["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();
                        return respuestaIA ?? "La IA está pensando...";
                    }
                    else
                    {
                        // Si falla, te dirá el error exacto (404, 429, etc.)
                        return $"Error ({response.StatusCode}): {jsonResponse}";
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error técnico: " + ex.Message;
            }
        }
    }
}