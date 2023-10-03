using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using nike_shoes_shop_backend.Data;
using nike_shoes_shop_backend.Models;

namespace nike_shoes_shop_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        public readonly DataContext _context;

        public UsersController(DataContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet("listUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Users>> GetUsers()
        {
            var list_User = await _context.Users.ToListAsync();
            return list_User;
        }

        [HttpGet("user/id")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var camp = await _context.Users.FindAsync(id);
            return camp == null ? NotFound() : Ok(camp);
        }

        [HttpPost("user")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateUser(Users user)
        {
            await _context.AddAsync(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { userId = user.userId }, user);
        }


        [HttpGet("CartUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IEnumerable<Object> GetCartUser(string id)
        {
            var cartUser = from u in _context.Users
                           join c in _context.Carts
                           on u.userId equals c.userId
                           join p in _context.Products
                           on c.productID equals p.id
                           select new
                           {
                               idProduct = p.id,
                               title = p.title,
                               text = p.text,
                               rating = p.rating,
                               btn = p.btn,
                               img = p.img,
                               price = p.price
                           };
            return cartUser;

        }

        [HttpPut("updateUser")]
        public async Task<IActionResult> UpdateUser(string userId, Users user)
        {
            if (userId != user.userId)
            {
                return BadRequest("Invalid Id");
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.userId == userId);

            if (existingUser == null)
            {
                return NotFound("User not found");
            }
            try
            {
                existingUser.fullName = user.fullName;
                existingUser.dob = user.dob;
                if (user.password != "")
                {
                    existingUser.password = user.password;
                }
                existingUser.email = user.email;
                existingUser.phone = user.phone;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();

                return Ok(existingUser);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

    }
}