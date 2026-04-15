using System.Text.Json;
using BlazorApp.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
    private static readonly HttpClient BeyondClient = new()
    {
        BaseAddress = new Uri("https://character-service.dndbeyond.com"),
    };
    
    [Route("character/{id:int}")]
    [HttpGet]
    public async Task<CharacterData> Get(int id)
    {
        using var characterData = await BeyondClient.GetAsync($"character/v5/character/{id}");

        return await ShapeResponse(characterData);
    }
    
    private static async Task<CharacterData> ShapeResponse(HttpResponseMessage beyondResponse)
    {
        var jsonCharacterData = await beyondResponse.Content.ReadAsStringAsync();
            
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
