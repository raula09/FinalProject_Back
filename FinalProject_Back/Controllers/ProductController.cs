using FinalProject_Back.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly AuthHttpClient _authHttpClient;

    public ProductController(HttpClient httpClient, AuthHttpClient authHttpClient)
    {
        _httpClient = httpClient; 
        _authHttpClient = authHttpClient;
    }


    [HttpGet("GetProductsBy/{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var response = await _httpClient.GetAsync($"https://api.everrest.educata.dev/shop/products/id/{id}");

        if (response.IsSuccessStatusCode)
        {
          
            var product = await response.Content.ReadAsStringAsync();

            var formattedJson = JToken.Parse(product).ToString(Newtonsoft.Json.Formatting.Indented);

            
            return Content(formattedJson, "application/json");
        }
        else
        {
            return NotFound();
        }
    }


    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllProducts()
    {
      
        var apiUrl = "https://api.everrest.educata.dev/shop/products/all";
        var response = await _httpClient.GetAsync(apiUrl);

        return response.IsSuccessStatusCode
            ? Content(await response.Content.ReadAsStringAsync(), "application/json")
            : NotFound();
    }


    [HttpDelete("DeleteProduct/{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var response = await _authHttpClient.DeleteAsync($"shop/products/id/{id}");

        return response.IsSuccessStatusCode ? NoContent() : NotFound();
    }


    [HttpPost("Create")]
    public async Task<IActionResult> CreateProduct(Product product)
    {
        if (product == null)
            return BadRequest("Product is null.");

        var response = await _authHttpClient.PostAsync("shop/products", product);

        return response.IsSuccessStatusCode
            ? Content(await response.Content.ReadAsStringAsync(), "application/json")
            : StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}
