using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using nike_shoes_shop_backend.Models;

namespace nike_shoes_shop_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

        private Users AuthenticateUser(Users users)
        {
            Users _user = null;

            if (users.username == "admin" && users.password == "12345")
            {
                _user = new Users
                {
                    username = "mrkevin",
                    password = "123456"
                };
            }

            return _user;
        }

        private string GenerateToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            IActionResult res = Unauthorized();
            var user_ = AuthenticateUser(user);

            if (user_ != null)
            {
                var token = GenerateToken(user_);
                res = Ok(new { token = token });
            }
            return res;
        }

        [Authorize]
        [HttpGet]
        public string Get()
        {
            return "Jwt authen";
        }
    }
}