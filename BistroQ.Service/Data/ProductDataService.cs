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

    public async Task<ApiCollectionResponse<IEnumerable<Product>>> GetProductsAsync(ProductCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<ProductResponse>>("api/Product", query);
        if (response.Success)
        {
            var products = _mapper.Map<IEnumerable<Product>>(response.Data);
            return new ApiCollectionResponse<IEnumerable<Product>>
                (products, response.TotalItems, response.CurrentPage, response.TotalPages);
        }
        
        throw new Exception("Failed to get products");
    }
}