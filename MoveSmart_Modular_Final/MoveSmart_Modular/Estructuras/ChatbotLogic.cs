using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArbolEmpresaMudanzas.Estructuras
{
    public class ChatbotLogic
    {
        // ✅ CAMBIO APLICADO: Leemos la clave desde la clase Secrets
        private readonly string API_KEY = Secrets.ApiKey;

        // URL Base
        private const string BASE_URL = "https://generativelanguage.googleapis.com/v1beta";
        private const string URL_CHAT = BASE_URL + "/models/gemini-2.5-flash:generateContent";

        private static readonly HttpClient client = new HttpClient(new HttpClientHandler { UseProxy = false });

        public async Task<string> EnviarMensajeGemini(string mensajeUsuario)
        {
            try
            {
                // Validación básica
                if (string.IsNullOrEmpty(API_KEY) || API_KEY.Contains("PEGA_AQUI"))
                {
                    return "❌ Error: La API Key no está configurada en Secrets.cs";
                }

                string keyLimpia = API_KEY.Trim();

                // Diagnóstico
                if (mensajeUsuario.Trim().ToUpper() == "MODELOS")
                {
                    return await ListarModelosDisponibles(keyLimpia);
                }

                // --- CÓDIGO NORMAL DE CHAT ---
                string finalUrl = $"{URL_CHAT}?key={keyLimpia}";

                string promptSistema = $@"
                    Eres MoveBot, asistente de MoveSmart Logistics.
                    Responde brevemente en español.
                    Usuario: {mensajeUsuario}";

                var payload = new { contents = new[] { new { parts = new[] { new { text = promptSistema } } } } };
                string jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(finalUrl, content);
                string respuestaJson = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    using (System.Text.Json.JsonDocument doc = System.Text.Json.JsonDocument.Parse(respuestaJson))
                    {
                        if (doc.RootElement.TryGetProperty("candidates", out System.Text.Json.JsonElement candidates) && candidates.GetArrayLength() > 0)
                        {
                            return candidates[0].GetProperty("content").GetProperty("parts")[0].GetProperty("text").GetString();
                        }
                        return "🤖 (Sin respuesta)";
                    }
                }
                else
                {
                    return $"🚨 ERROR ({response.StatusCode}):\n{respuestaJson}";
                }
            }
            catch (Exception ex)
            {
                return $"💥 ERROR: {ex.Message}";
            }
        }

        private async Task<string> ListarModelosDisponibles(string key)
        {
            try
            {
                string url = $"{BASE_URL}/models?key={key}";
                HttpResponseMessage response = await client.GetAsync(url);
                string json = await response.Content.ReadAsStringAsync();
                return response.IsSuccessStatusCode ? "📋 MODELOS:\n" + json : "❌ ERROR LISTANDO:\n" + json;
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }
}