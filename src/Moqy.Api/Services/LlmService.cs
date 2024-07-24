using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Moqy.Api.Configuration;
using Moqy.Api.Exceptions;

namespace Moqy.Api.Services
{
    public interface ILlmService
    {
        Task<string> GenerateTextAsync(string prompt);
    }
    
    public class LlmService : ILlmService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<LlmService> _logger;
        private readonly LlmServiceOptions _options;

        public LlmService(HttpClient httpClient, IOptions<LlmServiceOptions> options, ILogger<LlmService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            _options = options.Value;
            _httpClient.BaseAddress = new Uri(_options.BaseUrl);
        }

        public async Task<string> GenerateTextAsync(string prompt)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(new { prompt }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("/generate", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<LlmResponse>(responseBody);
                return result.generated_text;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with LLM service");
                throw new LlmServiceException("Error communicating with LLM service", ex);
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Error deserializing LLM service response");
                throw new LlmServiceException("Error deserializing LLM service response", ex);
            }
        }

        private class LlmResponse
        {
            public string generated_text { get; set; }
        }
    }
}