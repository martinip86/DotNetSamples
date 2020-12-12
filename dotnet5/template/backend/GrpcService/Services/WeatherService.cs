using System;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace GrpcService.Services
{
    public class WeatherService : Weather.WeatherBase
    {
        private readonly ILogger<WeatherService> _logger;
        public WeatherService(ILogger<WeatherService> logger)
        {
            _logger = logger;
        }

        public override Task<WeatherDefinition> GetWeatherDefinition(WeatherDefinitionRequest request, ServerCallContext context)
        {
            _logger.LogInformation($"{nameof(GetWeatherDefinition)} called");

            var tempRange = GetTempRange(request.Summary);

            return Task.FromResult(new WeatherDefinition
            {
                Summary = request.Summary,
                Description = GetDescription(request.Summary),
                TempRangeStart = tempRange.Item1,
                TempRangeEnd = tempRange.Item2
            });
        }

        private (int, int) GetTempRange(string requestSummary)
        {
            switch (requestSummary.ToLowerInvariant())
            {
                case "sweltering":
                case "scorching":
                    return (30,99);
                case "balmy":
                case "hot":
                    return (25, 29);
                case "mild":
                case "warm":
                    return (18, 24);
                case "Chilly":
                case "Cool":
                    return (6, 17);
                case "Freezing":
                case "Bracing":
                    return (-100, 5);
                default:
                    throw new Exception("Unexpected weather");
            }
        }

        private string GetDescription(string requestSummary)
        {
            switch (requestSummary.ToLowerInvariant())
            {
                case "sweltering":
                case "scorching":
                    return "hide in an air conditioned place and don't come out";
                case "balmy":
                case "hot":
                    return "suns out, guns out!";
                case "mild":
                case "warm":
                    return "you can go out in a t-shirt and jeans";
                case "Chilly":
                case "Cool":
                    return "you should wear a jacket";
                case "Freezing":
                case "Bracing":
                    return "the works; jacket, gloves, thermal underwear...";
                default:
                    throw new Exception("Unexpected weather");
            }
        }
    }
}
