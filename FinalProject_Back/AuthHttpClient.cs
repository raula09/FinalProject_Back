using FinalProject_Back.Services;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;

public class AuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;
    private readonly ILogger<AuthHttpClient> _logger;

    public AuthHttpClient(HttpClient httpClient, TokenService tokenService, ILogger<AuthHttpClient> logger)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
        _logger = logger;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _tokenService.GetTokenAsync();

        if (string.IsNullOrEmpty(token))
        {
            _logger.LogError("No valid authorization token available.");
            throw new InvalidOperationException("No valid authorization token available.");
        }

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"GET request to {endpoint} failed with status code {response.StatusCode}");
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while making GET request to {endpoint}: {ex.Message}");
            throw;
        }
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PostAsJsonAsync(endpoint, data);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"POST request to {endpoint} failed with status code {response.StatusCode}");
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while making POST request to {endpoint}: {ex.Message}");
            throw;
        }
    }

    public async Task<HttpResponseMessage> PatchAsync<T>(string endpoint, T data)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.PatchAsJsonAsync(endpoint, data);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"PATCH request to {endpoint} failed with status code {response.StatusCode}");
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while making PATCH request to {endpoint}: {ex.Message}");
            throw;
        }
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        try
        {
            await SetAuthorizationHeaderAsync();
            var response = await _httpClient.DeleteAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"DELETE request to {endpoint} failed with status code {response.StatusCode}");
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while making DELETE request to {endpoint}: {ex.Message}");
            throw;
        }
    }
}
