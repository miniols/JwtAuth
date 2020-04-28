using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtTokenJwtIssuer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> LoginJwt([FromBody]Login login)
        {
            List<Login> users = new List<Login>();
            users.Add(new Login
            {
                User = "Test",
                Key = "TestKey"
            });

            var usr = new List<Login>();

            var selusr = users.Select(x => x.User == login.User & x.Key == login.Key).FirstOrDefault();

            if (selusr)
            {
                var authClaims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Role, "FromJwt")
                };

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecureKeyForEnlanggreie"));

                var token = new JwtSecurityToken(
                    issuer: "https://localhost:44369",
                    audience: "https://localhost:44389",
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return NotFound("Could not find user");

        }


        public class Login
        {
            public string User { get; set; }
            public string Key { get; set; }
        }
    }
}