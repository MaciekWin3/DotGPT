﻿using DotGPT.Models.Responses;
using System.Text.Json.Serialization;

namespace DotGPT.Models.Requests
{
    public record ChatGptResponse
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }
        [JsonPropertyName("object")]
        public string? Object { get; set; }
        [JsonPropertyName("created")]
        public int Created { get; set; }
        [JsonPropertyName("model")]
        public string? Model { get; set; }
        [JsonPropertyName("choices")]
        public Choice[] Choices { get; set; } = null!;
        [JsonPropertyName("usage")]
        public Usage Usage { get; set; } = null!;
    }
}