using System.Net;
using System.Text.Json;
using BlazorApp.Shared;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Api
{
    public class CharacterDataFunction(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<CharacterDataFunction>();

        private static readonly HttpClient BeyondClient = new()
        {
            BaseAddress = new Uri("https://character-service.dndbeyond.com"),
        };

        [Function("CharacterData")]
        public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get",
            Route = "CharacterData/{id}")] HttpRequestData req, string id)
        {
            using var characterData = await BeyondClient.GetAsync($"character/v5/character/{id}");
            
            var shapedCharacterData = await ShapeResponse(characterData);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(shapedCharacterData);

            return response;
        }

        private static async Task<CharacterData> ShapeResponse(HttpResponseMessage response)
        {
            var jsonCharacterData = await response.Content.ReadAsStringAsync();
            
            var unshapedCharacterDataObj = JsonDocument.Parse(jsonCharacterData);
            var dataElement = unshapedCharacterDataObj.RootElement.GetProperty("data");

            return new CharacterData(
                dataElement.GetProperty("name").GetString(),
                dataElement.GetProperty("inspiration").GetBoolean(),
                dataElement.GetProperty("baseHitPoints").GetInt32(),
                dataElement.GetProperty("removedHitPoints").GetInt32(),
                dataElement.GetProperty("temporaryHitPoints").GetInt32()
            );
        } 
    }
}
