using BistroQ.Domain.Dtos;

namespace BistroQ.Domain.Contracts.Http;

/// <summary>
/// Interface for handling multipart form data API requests.
/// Provides methods for uploading files and JSON content using multipart/form-data.
/// </summary>
public interface IMultipartApiClient
{
    /// <summary>
    /// Sends a POST request with multipart form data content.
    /// </summary>
    /// <typeparam name="T">The type of the expected response data.</typeparam>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="content">The multipart form data content to send.</param>
    /// <returns>An API response containing data of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> PostMultipartAsync<T>(string url, MultipartFormDataContent content);

    /// <summary>
    /// Sends a POST request with JSON content and files as multipart form data.
    /// </summary>
    /// <typeparam name="T">The type of the expected response data.</typeparam>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="jsonContent">The JSON content to include in the request.</param>
    /// <param name="jsonPartName">The name of the JSON part in the multipart form.</param>
    /// <param name="files">Dictionary of files to upload, containing stream, filename, and content type for each file.</param>
    /// <returns>An API response containing data of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> PostMultipartAsync<T>(string url, object jsonContent, string jsonPartName,
        Dictionary<string, (Stream Stream, string FileName, string ContentType)> files);

    /// <summary>
    /// Sends a PUT request with JSON content and files as multipart form data.
    /// </summary>
    /// <typeparam name="T">The type of the expected response data.</typeparam>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="jsonContent">The JSON content to include in the request.</param>
    /// <param name="jsonPartName">The name of the JSON part in the multipart form.</param>
    /// <param name="files">Dictionary of files to upload, containing stream, filename, and content type for each file.</param>
    /// <returns>An API response containing data of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> PutMultipartAsync<T>(string url, object jsonContent, string jsonPartName,
        Dictionary<string, (Stream Stream, string FileName, string ContentType)> files);

    /// <summary>
    /// Sends a PUT request with only file content as multipart form data.
    /// </summary>
    /// <typeparam name="T">The type of the expected response data.</typeparam>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="files">Dictionary of files to upload, containing stream, filename, and content type for each file.</param>
    /// <returns>An API response containing data of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> PutMultipartAsync<T>(string url, Dictionary<string, (Stream Stream, string FileName, string ContentType)> files);

    /// <summary>
    /// Sends a PATCH request with only file content as multipart form data.
    /// </summary>
    /// <typeparam name="T">The type of the expected response data.</typeparam>
    /// <param name="url">The URL to send the request to.</param>
    /// <param name="files">Dictionary of files to upload, containing stream, filename, and content type for each file.</param>
    /// <returns>An API response containing data of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> PatchMultipartAsync<T>(string url, Dictionary<string, (Stream Stream, string FileName, string ContentType)> files);
}