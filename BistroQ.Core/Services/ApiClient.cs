using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
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

    public async Task<ApiResponse<T>> PostAsync<T>(string url, object contentValue)
    {
        var request = new StringContent(
            JsonConvert.SerializeObject(contentValue),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PostAsync(url, request);
        return await HandleResponse<T>(response);
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string url, object contentValue)
    {
        var request = new StringContent(
            JsonConvert.SerializeObject(contentValue),
            Encoding.UTF8,
            "application/json"
        );

        var response = await _httpClient.PutAsync(url, request);
        return await HandleResponse<T>(response);
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string url, object queryParams = null)
    {
        var finalUrl = BuildUrlWithQueryParams(url, queryParams);
        var response = await _httpClient.GetAsync(finalUrl);
        return await HandleResponse<T>(response);
    }

    public async Task<PaginationResponseDto<T>> GetCollectionAsync<T>(string url, object queryParams = null) where T : class
    {
        var finalUrl = BuildUrlWithQueryParams(url, queryParams);
        var response = await _httpClient.GetAsync(finalUrl);

        var resultContentString = await response.Content.ReadAsStringAsync();

        try
        {
            var res = JsonConvert.DeserializeObject<PaginationResponseDto<T>>(resultContentString);

            if (res == null)
            {
                return null;
            }

            return res;
        }
        catch (JsonException ex)
        {
            return null;
        }
    }

    public async Task<ApiResponse<T>> DeleteAsync<T>(string url, object queryParams = null)
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

    private static async Task<ApiResponse<T>> HandleResponse<T>(HttpResponseMessage response)
    {
        var resultContentString = await response.Content.ReadAsStringAsync();

        try
        {
            var res = JsonConvert.DeserializeObject<ApiResponse<T>>(resultContentString);

            if (res == null)
            {
                return new ApiResponse<T>
                {
                    Success = false,
                    Message = "Empty response",
                    StatusCode = (int)response.StatusCode
                };
            }

            return res;
        }
        catch (JsonException ex)
        {
            return new ApiResponse<T>
            {
                Success = false,
                Message = "Failed to process response",
                Error = ex.Message,
                StatusCode = (int)response.StatusCode
            };
        }
    }
}