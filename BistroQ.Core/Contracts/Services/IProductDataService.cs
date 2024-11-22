using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Products;
using BistroQ.Core.Entities;

namespace BistroQ.Core.Contracts.Services;

public interface IProductDataService
{
    Task<PaginationResponseDto<IEnumerable<Product>>> GetProductsAsync(ProductCollectionQueryParams query);
}
