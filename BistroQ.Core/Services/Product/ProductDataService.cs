using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Products;
using BistroQ.Core.Entities;

namespace BistroQ.Core.Services;

public class ProductDataService : IProductDataService
{
    private readonly IApiClient _apiClient;

    public ProductDataService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<PaginationResponseDto<IEnumerable<Product>>> GetProductsAsync(ProductCollectionQueryParams query)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<Product>>("api/Product", query);
        return response;
    }
}
