
using FinalProject_Back.Services;
using System.Net.Http.Headers;
using System.Text.Json;

public class AuthHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly TokenService _tokenService;

    public AuthHttpClient(HttpClient httpClient, TokenService tokenService)
    {
        _httpClient = httpClient;
        _tokenService = tokenService;
    }

    private async Task SetAuthorizationHeaderAsync()
    {
        var token = await _tokenService.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.GetAsync(endpoint);
    }

    public async Task<HttpResponseMessage> PostAsync<T>(string endpoint, T data)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.PostAsJsonAsync(endpoint, data);
    }

    public async Task<HttpResponseMessage> PatchAsync<T>(string endpoint, T data)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.PatchAsJsonAsync(endpoint, data);
    }

    public async Task<HttpResponseMessage> DeleteAsync(string endpoint)
    {
        await SetAuthorizationHeaderAsync();
        return await _httpClient.DeleteAsync(endpoint);
    }
}
