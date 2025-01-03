﻿using BistroQ.Domain.Dtos;

namespace BistroQ.Domain.Contracts.Services;

// <summary>
/// The base API client interface that defines the methods to interact with the REST API.
/// </summary>
public interface IBaseApiClient
{
    /// <summary>
    /// Sends a POST request to the specified URL with the given content.
    /// </summary>
    /// <typeparam name="T">The return type of the request to the REST API.</typeparam>
    /// <param name="url">The URL to send the POST request to.</param>
    /// <param name="contentValue">The content to include in the POST request.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> PostAsync<T>(string url, object contentValue);

    /// <summary>
    /// Sends a PUT request to the specified URL with the given content.
    /// </summary>
    /// <typeparam name="T">The return type of the request to the REST API.</typeparam>
    /// <param name="url">The URL to send the PUT request to.</param>
    /// <param name="contentValue">The content to include in the PUT request.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> PutAsync<T>(string url, object contentValue);

    /// <summary>
    /// Sends a PATCH request to the specified URL with the given content.
    /// </summary>
    /// <typeparam name="T">The return type of the request to the REST API.</typeparam>
    /// <param name="url">The URL to send the PATCH request to.</param>
    /// <param name="contentValue">The content to include in the PATCH request.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> PatchAsync<T>(string url, object contentValue);

    /// <summary>
    /// Sends a GET request to the specified URL and returns the response.
    /// </summary>
    /// <typeparam name="T">The return type of the request to the REST API.</typeparam>
    /// <param name="url">The URL to send the GET request to.</param>
    /// <param name="contentValue">The content to include in the POST request.</param>
    /// <returns>A task that represents the asynchronous operation, containing the response of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> GetAsync<T>(string url, object contentValue);

    /// <summary>
    /// Sends a DELETE request to the specified URL.
    /// </summary>
    /// <param name="url">The URL to send the DELETE request to.</param>
    /// <param name="contentValue">The content to include in the DELETE request.</param
    /// <returns>A task that represents the asynchronous operation, containing the response of type <typeparamref name="T"/>.</returns>
    Task<ApiResponse<T>> DeleteAsync<T>(string url, object contentValue);

    /// <summary>
    /// Sends a GET request to the specified URL and returns a collection response.
    /// </summary>
    /// <typeparam name="T">The return type of the request to the REST API.</typeparam>
    /// <param name="url">The URL to send the GET request to.</param>
    /// <param name="contentValue">The content to include in the GET request.</param>
    /// <returns>A task that represents the asynchronous operation, containing the collection response of type <typeparamref name="T"/>.</returns>
    Task<ApiCollectionResponse<T>> GetCollectionAsync<T>(string url, object contentValue) where T : class;
}
