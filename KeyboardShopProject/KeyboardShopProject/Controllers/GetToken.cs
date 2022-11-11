using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Keyboard.Models.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetToken : Controller
    {
        private readonly IConfiguration _config;
        private readonly IMediator _mediator;

        public GetToken(IConfiguration config, IMediator mediator)
        {
            _config = config;
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Post))]
        public async Task<IActionResult> Post(int clientId)
        {
            if (clientId != null)
            {
                var user = await _mediator.Send(new GetClientByIdCommand(clientId));

                if (user.Client != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _config.GetSection("Jwt:Subject").Value),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.Client.ClientID.ToString()),
                        new Claim("DisplayName", user.Client.FullName?? string.Empty),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims,
                                                    expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: signIn);
                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid client Id");
                }
            }
            return BadRequest("Missing client Id");
        }
    }
}
