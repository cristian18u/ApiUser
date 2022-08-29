using ApiUser.Models;
using ApiUsers.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ApiUser.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(UsersService usersService) =>
        _usersService = usersService;

    /// <summary>
    /// Devuelve todos los paises
    /// </summary>
    /// <returns></returns>
    [HttpGet]

    public async Task<List<User>> Get() =>
        await _usersService.GetAsync();
    static readonly HttpClient client = new HttpClient();
    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<User>> Get(string id)
    {
        var book = await _usersService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        return book;
    }


    /// <summary>
    /// Crear nuevo pais
    /// </summary>
    /// <param name="newUser"></param>
    /// <returns>A newly created TodoItem</returns>
    /// <remarks>
    /// Sample request:
    ///
    ///     POST /Todo
    ///     {
    ///        "id": 1,
    ///        "name": "Item #1",
    ///        "isComplete": true
    ///     }
    ///
    /// </remarks>
    /// <response code="201">Returns the newly created item</response>
    /// <response code="400">If the item is null</response>
    [HttpPost]
    public async Task<IActionResult> Post(User newUser)
    {
        await _usersService.CreateAsync(newUser);

        return CreatedAtAction(nameof(Get), new { id = newUser.UserId }, newUser);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, User updatedUser)
    {
        var book = await _usersService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        updatedUser.UserId = book.UserId;

        await _usersService.UpdateAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var book = await _usersService.GetAsync(id);

        if (book is null)
        {
            return NotFound();
        }

        await _usersService.RemoveAsync(id);

        return NoContent();
    }

    [HttpGet("filter")]
    public async Task<ActionResult<User>> FilterName(string name)
    {
        var user = await _usersService.FilterNameAsync(name);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    [HttpGet]
    [Route("load")]
    public async Task<string> Load()
    {
        // string url = "https://restcountries.com/v3/all";
        // HttpClient client = new HttpClient();
        // var httpResponse = await client.GetAsync(url);
        // var content = await httpResponse.Content.ReadAsStringAsync();
        // dynamic result = JsonConvert.DeserializeObject<dynamic>(content);
        // foreach (var country in result)
        // {
        //     var pais = new Country();
        //     pais.Name = country.name.common;
        //     pais.Code = country.cca3;
        //     pais.Continent = country.continents[0];
        //     pais.Flag = country.flags[0];
        //     if (country.capital != null) pais.Capital = country.capital[0];
        //     pais.Population = country.population;
        //     await _usersService.CreateAsync(pais);
        // }
        return "charge database";
    }
}