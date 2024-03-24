using Identity.Domain.Aggregates.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity.UI.V5.Pages.Account.Internal;

namespace Identity.API.Controllers;

[Route("api/identity")]
[ApiController]
public class IdentityController : ApiController
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _configuration;


    public IdentityController(IMediator mediator, IConfiguration configuration, UserManager<IdentityUser> userManager) : base(mediator)
    {
        _configuration = configuration;
        _userManager = userManager;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] TestModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Email);

        if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
        {
            var tokenString = GenerateJWTToken(user);
            return Ok(new { token = tokenString });
        }

        return Unauthorized();
    }

    private string GenerateJWTToken(IdentityUser user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public class TestModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}