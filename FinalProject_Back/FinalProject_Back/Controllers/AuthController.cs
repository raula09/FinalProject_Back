using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FinalProject_Back.Models;
using FinalProject_Back.Models.FinalProject_Back.Models;
using System.Net.Http;

namespace FinalProject_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public AuthController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(User user)
        {
            if (user == null)
            {
                return BadRequest("user  is null.");
            }

            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://api.everrest.educata.dev/auth/sign_up", user);
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
                return BadRequest("user properties is null");
            }
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://api.everrest.educata.dev/auth/sign_in", signIn);
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

            var response = await _httpClient.PatchAsJsonAsync(apiUrl, request);

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
                return BadRequest("user properties is null");
            }

            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync("https://api.everrest.educata.dev/auth/verify_email", email);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return Content(result, "application/json");
            }
            return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
        }


    }
}