using FinalProject_Back.Models;
using FinalProject_Back.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

public class AuthController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;

    public AuthController(HttpClient httpClient, TokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
    {
        if (signUpRequest == null)
        {
            return BadRequest("User details are null.");
        }

        var response = await _httpClient.PostAsJsonAsync("https://api.everrest.educata.dev/auth/sign_up", signUpRequest);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpPost("sign-in")]
    public async Task<IActionResult> SignIn(SignIn signIn)
    {
        if (signIn == null)
        {
            return BadRequest("User properties are null.");
        }

        var response = await _httpClient.PostAsJsonAsync("https://api.everrest.educata.dev/auth/sign_in", signIn);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, responseContent);
        }

        
        Console.WriteLine("Sign-in response: " + responseContent);

      
        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

        if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken) || string.IsNullOrEmpty(tokenResponse.RefreshToken))
        {
            return BadRequest("Invalid token response from server.");
        }

        _tokenService.SetTokens(tokenResponse.AccessToken, tokenResponse.RefreshToken);

        return Ok(tokenResponse);
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var apiUrl = "https://api.everrest.educata.dev/auth/update";
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");

        var response = await _httpClient.PatchAsync(apiUrl, jsonContent);


        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        var updatedUser = await response.Content.ReadFromJsonAsync<UserResponse>();
        return Ok(updatedUser);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmail email)
    {
        if (email == null)
        {
            return BadRequest("User properties are null.");
        }

        var response = await _httpClient.PostAsJsonAsync("https://api.everrest.educata.dev/auth/verify_email", email);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}
