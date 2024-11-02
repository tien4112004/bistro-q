using System.Text;
using BistroQ.Core.Contracts.Services;
using Newtonsoft.Json;

namespace BistroQ.Core.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUri;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _apiBaseUri = Environment.GetEnvironmentVariable("API_BASE_URI") ?? "http://localhost:5256";
        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri(_apiBaseUri);
        }
    }

    public async Task PostAsync<T>(string url, object contentValue)
    {
        var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync(url, content);
        await HandleResponse(response);
    }

    public async Task PutAsync<T>(string url, object contentValue)
    {
        var content = new StringContent(JsonConvert.SerializeObject(contentValue), Encoding.UTF8, "application/json");
        var response = await _httpClient.PutAsync(url, content);
        await HandleResponse(response);
    }

    public async Task<T> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        await HandleResponse(response);
        var resultContentString = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<T>(resultContentString);
    }

    public async Task DeleteAsync(string url)
    {
        var response = await _httpClient.DeleteAsync(url);
        await HandleResponse(response);
    }

    private static async Task HandleResponse(HttpResponseMessage response)
    {
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed with status code {response.StatusCode}: {errorContent}");
        }
    }
}
