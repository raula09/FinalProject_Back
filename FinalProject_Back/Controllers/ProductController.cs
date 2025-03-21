using FinalProject_Back.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProductController : ControllerBase
{
    private readonly AuthHttpClient _authHttpClient;

    public ProductController(AuthHttpClient authHttpClient)
    {
        _authHttpClient = authHttpClient;
    }

    [HttpGet("GetProductsBy/{id}")]
    public async Task<IActionResult> GetProductById(string id)
    {
        var response = await _authHttpClient.GetAsync($"shop/products/id/{id}");

        return response.IsSuccessStatusCode
            ? Ok(await response.Content.ReadAsStringAsync())
            : NotFound();
    }

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllProducts()
    {
        var response = await _authHttpClient.GetAsync("shop/products/all");

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
