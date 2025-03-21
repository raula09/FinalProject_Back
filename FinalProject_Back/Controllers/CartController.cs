using FinalProject_Back.Models;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class CartController : ControllerBase
{
    private readonly AuthHttpClient _authHttpClient;

    public CartController(AuthHttpClient authHttpClient)
    {
        _authHttpClient = authHttpClient;
    }

    [HttpPost("add-to-cart")]
    public async Task<IActionResult> AddToCart([FromBody] Cart request)
    {
        var response = await _authHttpClient.PostAsync("shop/cart/product", request);

        return response.IsSuccessStatusCode
            ? Content(await response.Content.ReadAsStringAsync(), "application/json")
            : StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}
