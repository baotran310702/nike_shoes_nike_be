using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.IdentityModel.Tokens;
using nike_shoes_shop_backend.Data;
using nike_shoes_shop_backend.Identity;
using nike_shoes_shop_backend.Models;

namespace nike_shoes_shop_backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        public readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }


        [HttpGet("products")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> GetProducts()
        {
            var list_Product = await _context.Products.ToListAsync();
            return list_Product;
        }

        [HttpGet("popularSales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> GetPopularSales()
        {
            var topPopular = await _context.Products.ToListAsync();

            var top3Popular = topPopular.Take(3);

            return top3Popular;
        }

        [HttpGet("topRateSales")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<Product>> TopRatesSales()
        {
            var topPopular = await _context.Products.ToListAsync();

            var top3Popular = topPopular.Take(12);

            return top3Popular;
        }

        [HttpGet("highlights")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetHighlights()
        {
            var topPopular = await _context.Products.ToListAsync();

            var lastItem = topPopular.Last();

            if (lastItem == null)
            {
                return NotFound();
            }

            return Ok(lastItem);
        }

        [HttpGet("product")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(string id)
        {
            var product = await _context.Products.FindAsync(id);
            return product == null ? NotFound() : Ok(product);
        }

        [HttpGet("searchProduct")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> getProducts(string keyword)
        {
            if (string.IsNullOrEmpty(keyword))
            {
                return BadRequest();
            }

            var products = await _context.Products
            .Where(p => p.title.Contains(keyword)).Take(5).ToListAsync();

            if (products == null)
            {
                return NotFound("No products found");
            }
            return Ok(products);
        }

        [HttpPost("createProduct")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = product.id }, product);
        }

        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteProduct(string id)
        {
            var prod = (from p in _context.Products
                        where id == p.id
                        select p).FirstOrDefault();
            if (prod == null)
            {
                return NotFound();
            }
            _context.Remove<Product>(prod);
            _context.SaveChangesAsync();
            return Ok("Removed");
        }

    }
}