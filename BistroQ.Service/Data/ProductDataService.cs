using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

public class ProductDataService : IProductDataService
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public ProductDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

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
}