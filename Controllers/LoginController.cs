using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiUser.Models;
using ApiUser.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace ApiUser.Controllers;
[Authorize]
[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginService _loginService;
    private readonly IConfiguration _configuration;

    public LoginController(LoginService loginService, IConfiguration config)
    {
        _loginService = loginService;
        _configuration = config;
    }
    // public AuthController(IConfiguration configuration)
    // {
    //     _configuration = configuration;
    // }

    /// <summary>
    /// Devuelve todos los Usuarios
    /// </summary>
    /// <returns></returns>
    [HttpGet]

    public async Task<List<UserLogin>> Get() =>
        await _loginService.GetAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<UserLogin>> Get(string id)
    {
        var user = await _loginService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        return user;
    }

    /// <summary>
    /// Crear nuevo pais
    /// </summary>
    /// <param></param>
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
    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Post(UserLogin UserLogin)
    {
        Console.WriteLine(UserLogin);
        var userDb = await _loginService.FilterNameAsync(UserLogin.User);
        Console.WriteLine(userDb);
        var passwordCorrect = userDb == null ? false : UserLogin.Password == userDb.Password;

        if (userDb == null || !passwordCorrect)
        {
            return NotFound();
        }

        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.Sid, userDb._id)
        };
        var key = System.Text.Encoding.ASCII.GetBytes(_configuration.GetValue<string>("SecretKey"));
        var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature);
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(4),
            signingCredentials: creds
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return Ok(jwt);
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> Update(string id, UserLogin updatedUser)
    {
        var user = await _loginService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        updatedUser._id = user._id;

        await _loginService.UpdateAsync(id, updatedUser);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _loginService.GetAsync(id);

        if (user is null)
        {
            return NotFound();
        }

        await _loginService.RemoveAsync(id);

        return NoContent();
    }
}