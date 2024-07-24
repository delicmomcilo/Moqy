using Microsoft.AspNetCore.Mvc;
using Moqy.Api.Models;
using Moqy.Api.Services;
using System.Text;
using System.Text.Json;

namespace Moqy.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MockController : ControllerBase
    {
        private readonly IMockDataService _mockDataService;
        private readonly ILogger<MockController> _logger;

        public MockController(IMockDataService mockDataService, ILogger<MockController> logger)
        {
            _mockDataService = mockDataService;
            _logger = logger;
        }

        [HttpPost("stream")]
        [Produces("text/event-stream")]
        public async Task StreamMockData([FromBody] JsonElement input, [FromQuery] bool isSimpleJson = false, [FromQuery] int delayMs = 50, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Received request to stream mock data. Delay: {DelayMs}ms, Simple JSON: {IsSimpleJson}", delayMs, isSimpleJson);

            Schema schema;
            if (isSimpleJson)
            {
                var simpleJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(input.GetRawText());
                schema = _mockDataService.ConvertSimpleJsonToSchema(simpleJson);
            }
            else
            {
                schema = JsonSerializer.Deserialize<Schema>(input.GetRawText());
            }

            Response.Headers.Append("Content-Type", "text/event-stream");
            Response.Headers.Append("Cache-Control", "no-cache");
            Response.Headers.Append("Connection", "keep-alive");

            await using (var writer = new StreamWriter(Response.Body))
            {
                try
                {
                    await foreach (var data in _mockDataService.GenerateStreamingDataAsync(schema, delayMs).WithCancellation(cancellationToken))
                    {
                        foreach (char c in data)
                        {
                            cancellationToken.ThrowIfCancellationRequested();

                            await writer.WriteAsync(c);
                            await writer.FlushAsync();
                            await Task.Delay(delayMs);
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    _logger.LogInformation("Stream aborted by the client.");
                }
            }
        }
    }
}
