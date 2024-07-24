using System.Text.Json.Serialization;

namespace Moqy.Api.Models
{
    public class Schema
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public Dictionary<string, SchemaProperty> Properties { get; set; } = new();

        [JsonPropertyName("items")]
        public SchemaProperty? Items { get; set; }

        [JsonPropertyName("minItems")]
        public int? MinItems { get; set; }

        [JsonPropertyName("maxItems")]
        public int? MaxItems { get; set; }

        [JsonPropertyName("format")]
        public string? Format { get; set; }

        [JsonPropertyName("minimum")]
        public double? Minimum { get; set; }

        [JsonPropertyName("maximum")]
        public double? Maximum { get; set; }

        [JsonPropertyName("enum")]
        public List<object>? EnumValues { get; set; }
    }

    public class SchemaProperty : Schema
    {
        [JsonPropertyName("faker")]
        public string? Faker { get; set; }
    }
}