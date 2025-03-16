using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FinalProject_Back.Models;


namespace FinalProject_Back.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly HttpClient _client;
        private readonly AppDbContext _context;

        public ProductController(HttpClient client, AppDbContext context)
        {
            _client = client;
            _context = context;
        }
        [HttpGet("categories/all")]
        public async Task<IActionResult> GetCategoriesAll()
        {
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
        

        [HttpGet("categories/{id}")]
        public async Task<IActionResult> GetCategoryById(string id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
            {
                return NotFound(new { message = "Category not found." });
            }
            return Ok(category);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllProducts()
        {
            var response = await _client.GetAsync("https://api.everrest.educata.dev/shop/products/all");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Response.Headers["Content-Length"] = content.Length.ToString(); 
                return Content(content, "application/json");
            }

            return NotFound();
        }


        [HttpDelete("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _client.DeleteAsync($"https://api.everrest.educata.dev/shop/products/id/{id}");

            if (response.IsSuccessStatusCode)
            {
                return NoContent(); 
            }

            return NotFound(); 
        }

      
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var productResponse = await _client.GetAsync($"https://api.everrest.educata.dev/shop/products/id/{id}");
            var productDetails = await productResponse.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(productDetails))
            {
                return NotFound();
            }

            return Ok(productDetails);
        }
        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct(Product product)
        {
           
            if (product == null)
            {
                return BadRequest("Product is null.");
            }

            using var client = new HttpClient();

            
            var response = await client.PostAsJsonAsync("https://api.everrest.educata.dev/shop/products", product);

            if (response.IsSuccessStatusCode)
            {
         
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }

            
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }





    }



}

