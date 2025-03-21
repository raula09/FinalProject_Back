using Microsoft.AspNetCore.Mvc;
using FinalProject_Back.Models;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FinalProject_Back.Services;
using System.Text.Json;

namespace FinalProject_Back.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly HttpClient _httpClient;
        private readonly TokenService _tokenService;

        public AuthController(HttpClient httpClient, TokenService tokenService)
        {
            _httpClient = httpClient;
            _tokenService = tokenService;
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

    
            var tokenResponse = JsonSerializer.Deserialize<Models.TokenResponse>(responseContent);

            if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken) || string.IsNullOrEmpty(tokenResponse.RefreshToken))
            {
                return BadRequest("Invalid token response from server.");
            }

    
            _tokenService.SetTokens(tokenResponse.AccessToken, tokenResponse.RefreshToken);

            return Ok(tokenResponse);
        }
        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp(User user)
        {
            if (user == null)
            {
                return BadRequest("User is null.");
            }

            var response = await _httpClient.PostAsJsonAsync("https://api.everrest.educata.dev/auth/sign_up", user);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                return StatusCode((int)response.StatusCode, responseContent);
            }


            var signInResponse = await SignIn(new SignIn { Email = user.Email, Password = user.Password });

            return signInResponse;
        }

    }
}
