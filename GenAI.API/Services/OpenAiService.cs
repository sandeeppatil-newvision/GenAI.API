using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace GenAI.API.Services
{
    public class OpenAiService : IOpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAiService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<string> GenerateFormattedContentAsync(string input, string outputType)
        {
            var prompt = $"Based on the following content, generate a {outputType} in structured format:\n\n\"{input}\"";

            var request = new
            {
                model = "gpt-4",
                messages = new[]
                {
                new { role = "user", content = prompt }
            },
                temperature = 0.7,
                max_tokens = 500
            };

            var requestContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var root = JsonDocument.Parse(json);
            return root.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
        }
    }
}
