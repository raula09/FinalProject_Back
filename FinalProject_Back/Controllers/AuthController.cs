using FinalProject_Back.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;

public class AuthController : ControllerBase
{
    private readonly HttpClient httpClient;

    public AuthController(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUp([FromBody] SignUpRequest signUpRequest)
    {
        if (signUpRequest == null)
        {
            return BadRequest("User details are null.");
        }

        var response = await httpClient.PostAsJsonAsync("https://api.everrest.educata.dev/auth/sign_up", signUpRequest);

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

        var response = await httpClient.PostAsJsonAsync("https://api.everrest.educata.dev/auth/sign_in", signIn);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpPatch("update")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
    {
        var apiUrl = "https://api.everrest.educata.dev/auth/update";
        var jsonContent = new StringContent(JsonSerializer.Serialize(request), System.Text.Encoding.UTF8, "application/json");

        var response = await httpClient.PatchAsync(apiUrl, jsonContent);


        if (!response.IsSuccessStatusCode)
        {
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }

        var updatedUser = await response.Content.ReadFromJsonAsync<UserResponse>();
        return Ok(updatedUser);
    }

    [HttpPost("verify-email")]
    public async Task<IActionResult> VerifyEmail(VerifyEmail email)
    {
        if (email == null)
        {
            return BadRequest("User properties are null.");
        }

        var response = await httpClient.PostAsJsonAsync("https://api.everrest.educata.dev/auth/verify_email", email);
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return Content(result, "application/json");
        }

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}
