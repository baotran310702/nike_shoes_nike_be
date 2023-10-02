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
    public class StoryController : ControllerBase
    {

        private IConfiguration _config;
        private readonly DataContext _context;

        public StoryController(DataContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IEnumerable<Story>> GetStories()
        {
            var list_story = await _context.Story.ToListAsync();
            return list_story;
        }

        [HttpPost("createStory")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> createStory(Story story)
        {
            await _context.AddAsync(story);
            await _context.SaveChangesAsync();
            return Ok(story);
        }

        [HttpDelete]
        public IActionResult DeleteStory(string id)
        {
            var story = (from s in _context.Story where id == s.id select s).FirstOrDefault();
            if (story == null)
            {
                return NotFound();
            }
            _context.Remove<Story>(story);
            _context.SaveChangesAsync();
            return Ok("Story deleted");
        }

    }
}