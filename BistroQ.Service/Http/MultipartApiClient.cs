using BistroQ.Domain.Contracts.Http;
using BistroQ.Domain.Dtos;
using Newtonsoft.Json;

namespace BistroQ.Service.Http;

/// <summary>
/// Implements multipart form data handling for API requests, supporting file uploads and JSON content.
/// Provides methods for POST, PUT, and PATCH operations with multipart/form-data content type.
/// </summary>
public class MultipartApiClient : IMultipartApiClient
{
    #region Private Fields
    private readonly HttpClient _httpClient;
    #endregion

    #region Constructor
    public MultipartApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    #endregion

    #region Public Methods
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
            var properties = jsonContent.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(jsonContent)?.ToString();
                if (value != null)
                {
                    content.Add(new StringContent(value), $"{jsonPartName}.{property.Name}");
                }
            }
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
            var properties = jsonContent.GetType().GetProperties();
            foreach (var property in properties)
            {
                var value = property.GetValue(jsonContent)?.ToString();
                if (value != null)
                {
                    content.Add(new StringContent(value), $"{jsonPartName}.{property.Name}");
                }
            }
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

    public async Task<ApiResponse<T>> PutMultipartAsync<T>(string url, Dictionary<string, (Stream Stream, string FileName, string ContentType)> files)
    {
        using var content = new MultipartFormDataContent();
        foreach (var file in files)
        {
            var fileContent = new StreamContent(file.Value.Stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.Value.ContentType);
            content.Add(fileContent, file.Key, file.Value.FileName);
        }

        var response = await _httpClient.PutAsync(url, content);
        return await HandleResponse<T>(response);
    }

    public async Task<ApiResponse<T>> PatchMultipartAsync<T>(string url, Dictionary<string, (Stream Stream, string FileName, string ContentType)> files)
    {
        using var content = new MultipartFormDataContent();
        foreach (var file in files)
        {
            var fileContent = new StreamContent(file.Value.Stream);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.Value.ContentType);
            content.Add(fileContent, file.Key, file.Value.FileName);
        }

        var response = await _httpClient.PatchAsync(url, content);
        return await HandleResponse<T>(response);
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Handles the HTTP response and deserializes it into an API response object.
    /// </summary>
    /// <typeparam name="T">The type of the expected response data.</typeparam>
    /// <param name="response">The HTTP response message to handle.</param>
    /// <returns>An API response containing data of type <typeparamref name="T"/>.</returns>
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
    #endregion
}
