using System.Net;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class HttpTrigger
    {
        private readonly ILogger _logger;
        private string characterId;

        public HttpTrigger(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<HttpTrigger>();
        }
        
        private static HttpClient beyondClient = new()
        {
            BaseAddress = new Uri("https://character-service.dndbeyond.com"),
        };

        [Function("CharacterData")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            characterId = "155705536";
            using HttpResponseMessage characterData = await beyondClient.GetAsync($"character/v5/character/{characterId}");
            
            var jsonCharacterData = await characterData.Content.ReadAsStringAsync();

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(jsonCharacterData);

            return response;
        }
    }
}
