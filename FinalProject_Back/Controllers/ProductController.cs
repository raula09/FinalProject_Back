using FinalProject_Back.Models;
using FinalProject_Back.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly AuthHttpClient _authHttpClient;
    private readonly TokenService _tokenService;
    public ProductController(HttpClient httpClient, AuthHttpClient authHttpClient, TokenService tokenService)
    {
        _httpClient = httpClient; 
        _authHttpClient = authHttpClient;
        _tokenService = tokenService;
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
    [HttpPatch("update-cart-product/{id}")]
    public async Task<IActionResult> UpdateCartProduct(string id, [FromBody] object updateData)
    {
        var response = await _authHttpClient.PatchAsync($"https://api.everrest.educata.dev/shop/cart/product/{id}", updateData);

        if (response.IsSuccessStatusCode)
        {
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }
        else
        {
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
    [HttpPost("RateProduct")] 
    public async Task<IActionResult> RateProduct([FromBody] Rate rating)
    {
        var response = await _authHttpClient.PostAsync("shop/products/rate", rating);
        return response.IsSuccessStatusCode
            ? Content(await response.Content.ReadAsStringAsync(), "application/json")
            : StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
    [HttpGet("GetCategories")]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var response = await _httpClient.GetAsync("https://api.everrest.educata.dev/shop/products/categories");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var formattedJson = JToken.Parse(content).ToString(Newtonsoft.Json.Formatting.Indented);
                return Content(formattedJson, "application/json");
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorMessage); 
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}"); 
        }
    }
    [HttpGet("AllBrands")]
    public async Task<IActionResult> GetAllBrands()
    {
        var response = await _httpClient.GetAsync("https://api.everrest.educata.dev/shop/products/brands");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var formattedJson = JToken.Parse(content).ToString(Newtonsoft.Json.Formatting.Indented);
            return Content(formattedJson, "application/json");
        }
        else
        {
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
    [HttpGet("Brand-By-Name")]
    public async Task<IActionResult> GetBrandByName(string name)
    {
        var response = await _httpClient.GetAsync($"https://api.everrest.educata.dev/shop/products/brands/{name}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var formattedJson = JToken.Parse(content).ToString(Newtonsoft.Json.Formatting.Indented);
            return Content(formattedJson, "application/json");
        }
        else
        {
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }
    [HttpGet("Search")]
    public async Task<IActionResult> SearchProducts(
       [FromQuery] int page_size = 10,
       [FromQuery] int page_index = 1,
       [FromQuery] string keywords = "",
       [FromQuery] string category_id = "",
       [FromQuery] string brand = "",
       [FromQuery] int rating = 0,
       [FromQuery] int price_min = 0,
       [FromQuery] int price_max = 0,
       [FromQuery] string sort_by = "rating",
       [FromQuery] string sort_direction = "asc"
   )
    {
        const string baseUrl = "https://api.everrest.educata.dev/shop/products/search";

        var queryParams = new List<string>();

        if (page_size > 0) queryParams.Add($"page_size={page_size}");
        if (page_index > 0) queryParams.Add($"page_index={page_index}");
        if (!string.IsNullOrWhiteSpace(keywords)) queryParams.Add($"keywords={Uri.EscapeDataString(keywords)}");
        if (!string.IsNullOrWhiteSpace(category_id)) queryParams.Add($"category_id={Uri.EscapeDataString(category_id)}");
        if (!string.IsNullOrWhiteSpace(brand)) queryParams.Add($"brand={Uri.EscapeDataString(brand)}");
        if (rating > 0) queryParams.Add($"rating={rating}");
        if (price_min > 0) queryParams.Add($"price_min={price_min}");
        if (price_max > 0) queryParams.Add($"price_max={price_max}");
        if (!string.IsNullOrWhiteSpace(sort_by)) queryParams.Add($"sort_by={Uri.EscapeDataString(sort_by)}");
        if (!string.IsNullOrWhiteSpace(sort_direction)) queryParams.Add($"sort_direction={Uri.EscapeDataString(sort_direction)}");

        var url = baseUrl;
        if (queryParams.Any())
        {
            url += "?" + string.Join("&", queryParams);
        }

        try
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var formattedJson = JToken.Parse(content).ToString(Newtonsoft.Json.Formatting.Indented);
                return Content(formattedJson, "application/json");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, errorContent);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }



}
