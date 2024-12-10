using BistroQ.Domain.Dtos;

namespace BistroQ.Domain.Contracts.Http;

public interface IMultipartApiClient
{
    Task<ApiResponse<T>> PostMultipartAsync<T>(string url, MultipartFormDataContent content);
    Task<ApiResponse<T>> PostMultipartAsync<T>(string url, object jsonContent, string jsonPartName,
        Dictionary<string, (Stream Stream, string FileName, string ContentType)> files);
    Task<ApiResponse<T>> PutMultipartAsync<T>(string url, object jsonContent, string jsonPartName,
        Dictionary<string, (Stream Stream, string FileName, string ContentType)> files);
}
