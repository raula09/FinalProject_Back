using FinalProject_Back.Models;
using FinalProject_Back.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly AuthHttpClient _authHttpClient;
    private readonly TokenService _tokenService;
    public CartController(AuthHttpClient authHttpClient, TokenService tokenService)
    {
        _authHttpClient = authHttpClient;
        _tokenService = tokenService;
    }

    [HttpPost("add-to-cart")]
    public async Task<IActionResult> AddToCart([FromBody] Cart request)
    {
        var response = await _authHttpClient.PatchAsync("shop/cart/product", request);

        return response.IsSuccessStatusCode
            ? Content(await response.Content.ReadAsStringAsync(), "application/json")
            : StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
    [HttpPost("cart-checkout")]
    public async Task<IActionResult> CartCheckout()
    {
        var response = await _authHttpClient.PostAsync<object>("https://api.everrest.educata.dev/shop/cart/checkout", new { });

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, errorContent);
    }

    [HttpGet("get-cart")]
    public async Task<IActionResult> GetCart()
    {
        var response = await _authHttpClient.GetAsync("shop/cart");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Content(content, "application/json");
        }
        var errorContent = await response.Content.ReadAsStringAsync();
        return StatusCode((int)response.StatusCode, errorContent);
    }
    [HttpDelete("remove-from-cart/{productId}")]
    public async Task<IActionResult> RemoveFromCart(string productId)
    {

        var response = await _authHttpClient.DeleteAsync($"https://api.everrest.educata.dev/shop/cart/product/{productId}");

        if (response.IsSuccessStatusCode)
        {
            return Content(await response.Content.ReadAsStringAsync(), "application/json");
        }
        else
        {
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }
    }



}
