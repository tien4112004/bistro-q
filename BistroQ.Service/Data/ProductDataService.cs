using AutoMapper;
using BistroQ.Domain.Contracts.Http;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

/// <summary>
/// Implementation of product data service that communicates with the API endpoints.
/// Handles both regular API calls and multipart form data for image uploads.
/// </summary>
public class ProductDataService : IProductDataService
{
    #region Private Fields
    /// <summary>
    /// Client for making standard HTTP requests to the API.
    /// </summary>
    private readonly IApiClient _apiClient;

    /// <summary>
    /// Client for handling multipart form data requests (file uploads).
    /// </summary>
    private readonly IMultipartApiClient _multipartApiClient;

    /// <summary>
    /// Mapper for converting between DTOs and domain models.
    /// </summary>
    private readonly IMapper _mapper;
    #endregion

    #region Constructor
    public ProductDataService(IApiClient apiClient, IMapper mapper, IMultipartApiClient multipartApiClient)
    {
        _apiClient = apiClient;
        _mapper = mapper;
        _multipartApiClient = multipartApiClient;
    }
    #endregion

    #region Public Methods
    public async Task<ApiCollectionResponse<IEnumerable<Product>>> GetGridDataAsync(ProductCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<ProductResponse>>("/api/admin/product", query);
        if (response.Success)
        {
            var products = _mapper.Map<IEnumerable<Product>>(response.Data);
            return new ApiCollectionResponse<IEnumerable<Product>>
                (products, response.TotalItems, response.CurrentPage, response.TotalPages);
        }
        throw new Exception("Failed to get products");
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest request)
    {
        var response = await _apiClient.PostAsync<ProductResponse>("api/admin/product", request);
        if (response.Success)
        {
            return _mapper.Map<Product>(response.Data);
        }

        throw new Exception(response.Message);
    }

    public async Task<Product> UpdateProductAsync(int productId, UpdateProductRequest request)
    {
        var response = await _apiClient.PutAsync<ProductResponse>($"api/admin/product/{productId}", request);

        if (response.Success)
        {
            return _mapper.Map<Product>(response.Data);
        }

        throw new Exception(response.Message);
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        var response = await _apiClient.DeleteAsync<object>($"api/admin/product/{productId}", null);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }

        return true;
    }

    public async Task<ApiCollectionResponse<IEnumerable<Product>>> GetProductsAsync(ProductCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<ProductResponse>>("/api/product", query);

        if (response.Success)
        {
            var products = _mapper.Map<IEnumerable<Product>>(response.Data);
            return new ApiCollectionResponse<IEnumerable<Product>>
                (products, response.TotalItems, response.CurrentPage, response.TotalPages);
        }

        throw new Exception("Failed to get products");
    }

    public async Task<ApiCollectionResponse<IEnumerable<Product>>> GetRecommendationAsync()
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<ProductResponse>>("/api/product/recommendations", null);

        if (response.Success)
        {
            var products = _mapper.Map<IEnumerable<Product>>(response.Data);
            return new ApiCollectionResponse<IEnumerable<Product>>
                (products, response.TotalItems, response.CurrentPage, response.TotalPages);
        }

        throw new Exception("Failed to get products");
    }

    public async Task<Product> CreateProductAsync(CreateProductRequest request, Stream stream, string fileName, string contentType)
    {
        var files = new Dictionary<string, (Stream Stream, string FileName, string ContentType)>();

        if (stream != null && !string.IsNullOrEmpty(fileName))
        {
            files.Add("Image", (stream, fileName, contentType));
        }

        var response = await _multipartApiClient.PostMultipartAsync<ProductResponse>("/api/admin/product",
            request, "Product", files);

        if (response.Success)
        {
            return _mapper.Map<Product>(response.Data);
        }
        throw new Exception(response.Message);
    }

    public async Task<Product> UpdateProductImageAsync(int productId, Stream stream, string fileName, string contentType)
    {
        var response = await _multipartApiClient.PatchMultipartAsync<ProductResponse>($"api/admin/product/{productId}",
            new Dictionary<string, (Stream Stream, string FileName, string ContentType)>
            {
                {"image", (stream, fileName, contentType) }
            });

        if (response.Success)
        {
            return _mapper.Map<Product>(response.Data);
        }
        throw new Exception(response.Message);
    }
    #endregion
}