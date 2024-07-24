using System.Runtime.CompilerServices;
using Moqy.Api.Models;
using System.Text.Json;
using Bogus;

namespace Moqy.Api.Services
{
    public interface IMockDataService
    {
        IAsyncEnumerable<string> GenerateStreamingDataAsync(Schema schema, int delayMs,
            [EnumeratorCancellation] CancellationToken cancellationToken = default);

        Schema ConvertSimpleJsonToSchema(Dictionary<string, JsonElement> simpleJson);
    }

    public class MockDataService : IMockDataService
    {
        private readonly Random _random = new Random();
        private readonly Faker _faker = new Faker();

        public async IAsyncEnumerable<string> GenerateStreamingDataAsync(Schema schema, int delayMs,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (schema.Type.ToLower() == "object")
            {
                yield return "{";

                bool first = true;
                foreach (var prop in schema.Properties)
                {
                    if (!first)
                    {
                        yield return ",";
                    }

                    var mockValue = GenerateMockData(prop.Value);
                    string serializedValue = JsonSerializer.Serialize(mockValue);
                    yield return $"\"{prop.Key}\": {serializedValue}";
                    first = false;

                    await Task.Delay(delayMs, cancellationToken);
                }

                yield return "}";
            }
            else
            {
                throw new NotImplementedException("Only object types are currently supported for streaming.");
            }
        }

        private object GenerateMockData(Schema schema)
        {
            return schema.Type.ToLower() switch
            {
                "object" => GenerateObject(schema),
                "array" => GenerateArray(schema),
                _ => GeneratePrimitive(schema)
            };
        }

        private Dictionary<string, object> GenerateObject(Schema schema)
        {
            var obj = new Dictionary<string, object>();
            foreach (var prop in schema.Properties)
            {
                obj[prop.Key] = GenerateMockData(prop.Value);
            }

            return obj;
        }

        private List<object> GenerateArray(Schema schema)
        {
            var count = _random.Next(schema.MinItems ?? 1, (schema.MaxItems ?? 10) + 1);
            var array = new List<object>();
            for (int i = 0; i < count; i++)
            {
                array.Add(GenerateMockData(schema.Items));
            }

            return array;
        }

        private object GeneratePrimitive(Schema schema)
        {
            if (schema.EnumValues?.Any() == true)
            {
                return schema.EnumValues[_random.Next(schema.EnumValues.Count)];
            }

            return schema.Type.ToLower() switch
            {
                "string" => GenerateString(schema.Format),
                "number" => GenerateNumber(schema.Minimum, schema.Maximum),
                "integer" => GenerateInteger(schema.Minimum, schema.Maximum),
                "boolean" => _faker.Random.Bool(),
                _ => throw new ArgumentException($"Unsupported type: {schema.Type}")
            };
        }

        private string GenerateString(string format)
        {
            return format?.ToLower() switch
            {
                "date-time" => _faker.Date.Recent().ToString("o"),
                "email" => _faker.Internet.Email(),
                "uuid" => Guid.NewGuid().ToString(),
                _ => _faker.Lorem.Word()
            };
        }

        private double GenerateNumber(double? min, double? max)
        {
            return _faker.Random.Double(min ?? 0, max ?? 100);
        }

        private int GenerateInteger(double? min, double? max)
        {
            return _faker.Random.Int((int)(min ?? 0), (int)(max ?? 100));
        }

        public Schema ConvertSimpleJsonToSchema(Dictionary<string, JsonElement> simpleJson)
        {
            var schema = new Schema
            {
                Type = "object",
                Properties = new Dictionary<string, SchemaProperty>()
            };

            foreach (var kvp in simpleJson)
            {
                var propertySchema = new SchemaProperty
                {
                    Type = GetJsonElementType(kvp.Value)
                };

                if (propertySchema.Type == "string")
                {
                    propertySchema.Format = "string";
                }
                else if (propertySchema.Type == "integer")
                {
                    propertySchema.Type = "integer";
                }
                else if (propertySchema.Type == "number")
                {
                    propertySchema.Type = "number";
                }
                else if (propertySchema.Type == "boolean")
                {
                    propertySchema.Type = "boolean";
                }
                else if (propertySchema.Type == "array")
                {
                    propertySchema.Type = "array";
                    propertySchema.Items = new SchemaProperty
                        { Type = "string" };
                }

                schema.Properties.Add(kvp.Key, propertySchema);
            }

            return schema;
        }

        private string GetJsonElementType(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.String => "string",
                JsonValueKind.Number => element.TryGetInt32(out _) ? "integer" : "number",
                JsonValueKind.True => "boolean",
                JsonValueKind.False => "boolean",
                JsonValueKind.Array => "array",
                _ => throw new ArgumentException($"Unsupported JsonElement type: {element.ValueKind}")
            };
        }
    }
}