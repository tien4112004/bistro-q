using BistroQ.Core.Contracts.Services;
using Newtonsoft.Json;
using System.Text;
using System.Web;

namespace BistroQ.Core.Services;

public class ApiClient : IApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUri;

    public ApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _apiBaseUri = Environment.GetEnvironmentVariable("API_BASE_URI") ?? "http://localhost:5256";

        if (_httpClient.BaseAddress == null)
        {
            _httpClient.BaseAddress = new Uri(_apiBaseUri);
        }
    }

    public async Task<T> PostAsync<T>(string url, object contentValue)
    {
        var request = new StringContent(
            JsonConvert.SerializeObject(contentValue),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(url, request);
        return await HandleResponse<T>(response);
    }

    public async Task<T> PutAsync<T>(string url, object contentValue)
    {
        var request = new StringContent(
            JsonConvert.SerializeObject(contentValue),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PutAsync(url, request);
        return await HandleResponse<T>(response);
    }

    public async Task<T> GetAsync<T>(string url, object queryParams = null)
    {
        var finalUrl = BuildUrlWithQueryParams(url, queryParams);
        var response = await _httpClient.GetAsync(finalUrl);
        return await HandleResponse<T>(response);
    }

    public async Task<T> DeleteAsync<T>(string url, object queryParams = null)
    {
        var finalUrl = BuildUrlWithQueryParams(url, queryParams);
        var response = await _httpClient.DeleteAsync(finalUrl);
        return await HandleResponse<T>(response);
    }

    private static string BuildUrlWithQueryParams(string baseUrl, object queryParams)
    {
        if (queryParams == null)
            return baseUrl;

        var queryString = string.Join("&",
            queryParams.GetType()
                .GetProperties()
                .Where(p => p.GetValue(queryParams) != null)
                .Select(p => $"{HttpUtility.UrlEncode(p.Name)}={HttpUtility.UrlEncode(p.GetValue(queryParams)?.ToString())}"));

        return string.IsNullOrEmpty(queryString)
            ? baseUrl
            : $"{baseUrl}?{queryString}";
    }

    private static async Task<T> HandleResponse<T>(HttpResponseMessage response)
    {
        var resultContentString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Request failed with status code {response.StatusCode}: {resultContentString}"
            );
        }

        try
        {
            return JsonConvert.DeserializeObject<T>(resultContentString)
                ?? throw new JsonException("Deserialization resulted in null object");
        }
        catch (JsonException ex)
        {
            throw new JsonException(
                $"Failed to deserialize response content: {resultContentString}",
                ex
            );
        }
    }
}