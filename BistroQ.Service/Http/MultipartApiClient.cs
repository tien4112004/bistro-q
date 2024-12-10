using BistroQ.Domain.Contracts.Http;
using BistroQ.Domain.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace BistroQ.Service.Http;

public class MultipartApiClient : IMultipartApiClient
{
    private readonly HttpClient _httpClient;

    public MultipartApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    public async Task<ApiResponse<T>> PostMultipartAsync<T>(string url, MultipartFormDataContent content)
    {
        var response = await _httpClient.PostAsync(url, content);
        return await HandleResponse<T>(response);
    }

    public async Task<ApiResponse<T>> PostMultipartAsync<T>(string url, object jsonContent, string jsonPartName,
        Dictionary<string, (Stream Stream, string FileName, string ContentType)> files)
    {
        using var content = new MultipartFormDataContent();

        if (jsonContent != null)
        {
            var jsonString = JsonConvert.SerializeObject(jsonContent);
            var jsonPart = new StringContent(jsonString, Encoding.UTF8, "application/json");
            content.Add(jsonPart, jsonPartName);
        }

        if (files != null)
        {
            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.Value.Stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.Value.ContentType);
                content.Add(fileContent, file.Key, file.Value.FileName);
            }
        }

        var response = await _httpClient.PostAsync(url, content);
        return await HandleResponse<T>(response);
    }

    public async Task<ApiResponse<T>> PutMultipartAsync<T>(string url, object jsonContent, string jsonPartName,
        Dictionary<string, (Stream Stream, string FileName, string ContentType)> files)
    {
        using var content = new MultipartFormDataContent();

        if (jsonContent != null)
        {
            var jsonString = JsonConvert.SerializeObject(jsonContent);
            var jsonPart = new StringContent(jsonString, Encoding.UTF8, "application/json");
            content.Add(jsonPart, jsonPartName);
        }

        if (files != null)
        {
            foreach (var file in files)
            {
                var fileContent = new StreamContent(file.Value.Stream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.Value.ContentType);
                content.Add(fileContent, file.Key, file.Value.FileName);
            }
        }

        var response = await _httpClient.PutAsync(url, content);
        return await HandleResponse<T>(response);
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
