using System.Text.Json.Serialization;

namespace GenAI.API.Models
{
    public class OpenAiResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
