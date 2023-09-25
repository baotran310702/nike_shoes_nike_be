using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using nike_shoes_shop_backend.Data;
using nike_shoes_shop_backend.Models;

namespace nike_shoes_shop_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {

        private IConfiguration _config;
        private readonly DataContext _context;

        public LoginController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        private Users AuthenticateUser(string username, string password)
        {
            var listUser = from u in _context.Users
                           select u;
            Users _user = null;

            foreach (var item in listUser)
            {
                if (item.username == username && item.password == password)
                {
                    _user = new Users
                    {
                        userId = item.userId,
                        username = item.username,
                        password = item.password,
                        role = item.role,
                    };
                }
            }
            return _user;
        }

        private string GenerateToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claim = new List<Claim>
            {
                new Claim("username",user.username),
                new Claim("role",user.role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], claims: claim, expires: DateTime.Now.AddSeconds(30), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(LoginRequestModel models)
        {
            IActionResult res = Unauthorized();
            var user_ = AuthenticateUser(models.username, models.password);

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