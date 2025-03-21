using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FinalProject_Back.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private string _accessToken;
        private string _refreshToken;

        public TokenService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetTokenAsync()
        {
            if (!string.IsNullOrEmpty(_accessToken))
            {
                return _accessToken;
            }

            return await RefreshTokenAsync();
        }

        private async Task<string> RefreshTokenAsync()
        {
            if (string.IsNullOrEmpty(_refreshToken))
            {
                throw new HttpRequestException("No refresh token available.");
            }

            var requestData = new { refresh_token = _refreshToken };
            var content = new StringContent(JsonSerializer.Serialize(requestData), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.everrest.educata.dev/auth/refresh_token", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException("Failed to refresh token");
            }

            var responseData = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseData);

            _accessToken = tokenResponse.AccessToken;
            _refreshToken = tokenResponse.RefreshToken; 

            return _accessToken;
        }


        public void SetTokens(string accessToken, string refreshToken)
        {
            _accessToken = accessToken;
            _refreshToken = refreshToken;
        }
    }

    public class TokenResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
