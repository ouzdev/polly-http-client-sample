using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Shared.Common;

namespace ApiServer.Controllers;

[Route("api")]
[ApiController]
[Authorize()]
public class AppController : Controller
{
    /// <summary>
    /// Get Hello World
    /// </summary>
    /// <returns></returns>
    [HttpGet("hello")]
    public IActionResult GetHelloWord()
    {
        return Ok("Hello Word!");
    }

    /// <summary>
    /// Get Token
    /// </summary>
    /// <param name="password"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("token")]
    [AllowAnonymous]
    public async Task<IActionResult> RequestToken([FromBody] string password, CancellationToken cancellationToken)
    {
        if (!await PasswordIsValid(password))
            return BadRequest("Invalid request");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(CommonConstants.SecurityKey));
        var claims = new[]
        {
            new Claim(ClaimTypes.SerialNumber, password)
        };
        var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: "ApiServer",
            audience: "ApiServer",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credential);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token)
        });
    }

    private Task<bool> PasswordIsValid(string password)
    {
        return Task.FromResult(password.Length is < 8 or > 14);
    }
}