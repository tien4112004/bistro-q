using BistroQ.Contracts.Services;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Products;
using BistroQ.Core.Entities;

namespace BistroQ.Services;

public class ProductService : IProductService
{
    private readonly IProductDataService _productDataService;

    public ProductService(IProductDataService productDataService)
    {
        _productDataService = productDataService;
    }

    public async Task<PaginationResponseDto<IEnumerable<Product>>> GetProductsAsync(ProductCollectionQueryParams productQueryParams)
    {
        productQueryParams = productQueryParams ?? new ProductCollectionQueryParams();
        var response = await _productDataService.GetProductsAsync(productQueryParams);
        return response;
    }
}
