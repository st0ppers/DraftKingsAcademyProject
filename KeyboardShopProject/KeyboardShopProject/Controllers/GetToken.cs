using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Keyboard.DL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Keyboard.ShopProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GetToken : Controller
    {
        private readonly IClientSqlRepository _clientSqlRepository;
        private readonly IConfiguration _config;

        public GetToken(IClientSqlRepository clientSqlRepository, IConfiguration config)
        {
            _clientSqlRepository = clientSqlRepository;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Post))]
        public async Task<IActionResult> Post(int clientId)
        {
            if (clientId != null)
            {
                var user = await _clientSqlRepository.GetById(clientId);

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _config.GetSection("Jwt:Subject").Value),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("UserId", user.ClientID.ToString()),
                        new Claim("DisplayName", user.FullName?? string.Empty),
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

        [AllowAnonymous]
        [HttpPost(nameof(CreateUser))]
        public async Task<IActionResult> CreateUser(int clientId)
        {
            if (clientId != null)
            {
                return BadRequest($"Enter clientId");
            }
            return Ok();
        }
    }
}
