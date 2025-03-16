using Azure;
using FinalProject_Back.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly HttpClient _client;

        public ProductController(HttpClient client)
        {
            _client = client;
            
        }


        [HttpGet("Get All Products")]
        public async Task<IActionResult> GetAllProducts()
        {
            var Products = await _client.GetAsync("https://api.everrest.educata.dev/shop/products/all");
            var ProductsList = await Products.Content.ReadAsStringAsync();
            if (ProductsList == null)
            {
                return NotFound();
            }
            return Ok(ProductsList);
        }

        [HttpGet("Get Product By {id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            var productResponse = await _client.GetAsync($"https://api.everrest.educata.dev/shop/products/id/{id}");
            var productDetails = await productResponse.Content.ReadAsStringAsync();

            if (productDetails == null)
            {
                return NotFound();
            }

            return Ok(productDetails);
        }

        [HttpPost("Add Product")]
        public async Task<IActionResult> AddProduct(Product product )
        {
            var Json = JsonSerializer.Serialize(product);
            var stringContent = new StringContent(Json, Encoding.UTF8, "application/json");
            var responce = await _client.PostAsync("https://api.everrest.educata.dev/shop/products", stringContent);
            if (responce == null)
            {
                return NotFound();
            }
            return Ok(responce);
        }

        [HttpPatch("Update Product")]
        public async Task<IActionResult> UpdateProduct(string id, Product product)
        {
            product.Id = id;
            var json = JsonSerializer.Serialize(product);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _client.PatchAsync($"https://api.everrest.educata.dev/shop/products/id/{id}", stringContent);
            var ListedResponce = await response.Content.ReadAsStringAsync();
            if (ListedResponce == null)
            {
                return NotFound();
            }
            return Ok(ListedResponce);
        }




        

    }
}
