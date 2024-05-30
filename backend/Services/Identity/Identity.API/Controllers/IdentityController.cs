using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Identity.API.Controllers;

[Route("api/identity")]
[ApiController]
public class IdentityController(IMediator mediator, IConfiguration configuration, UserManager<IdentityUser> userManager) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] TestModel model)
    {
        if (model.Email is null || model.Password is null)
            return Unauthorized();


        var user = await userManager.FindByNameAsync(model.Email);

        if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
        {
            var tokenString = GenerateJwtToken(user);
            return Ok(new { token = tokenString });
        }

        return Unauthorized();
    }

    private string GenerateJwtToken(IdentityUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public class TestModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}