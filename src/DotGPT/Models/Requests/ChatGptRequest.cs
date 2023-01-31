using System.Text.Json.Serialization;

namespace DotGPT.Models.Requests
{
    public record ChatGptRequest
    {
        [JsonPropertyName("model")]
        public string Model { get; set; } = "text-davinci-003";

        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.7;

        [JsonPropertyName("max_tokens")]
        public int MaxTokens { get; set; } = 256;

        [JsonPropertyName("top_p")]
        public int TopP { get; set; } = 1;

        [JsonPropertyName("frequency_penalty")]
        public int Frequency { get; set; } = 0;

        [JsonPropertyName("presence_penalty")]
        public int Presence { get; set; } = 0;

        public ChatGptRequest(string model, string prompt, double temperature, int maxTokens, int topP, int frequency, int presence)
        {
            Model = model;
            Prompt = prompt;
            Temperature = temperature;
            MaxTokens = maxTokens;
            TopP = topP;
            Frequency = frequency;
            Presence = presence;
        }

        public ChatGptRequest(string prompt)
        {
            Prompt = prompt;
        }
    }
}