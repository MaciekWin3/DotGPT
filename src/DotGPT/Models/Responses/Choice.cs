using System.Text.Json.Serialization;

namespace DotGPT.Models.Responses
{
    public record Choice
    {
        [JsonPropertyName("text")]
        public required string Text { get; set; }
        [JsonPropertyName("index")]
        public int Index { get; set; }
        [JsonPropertyName("logprobs")]
        public string? Logprobs { get; set; }

        [JsonPropertyName("finish_reason")]
        public string? FinishReason { get; set; }
    }
}