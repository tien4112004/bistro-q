using BistroQ.Core.Dtos;
using BistroQ.Core.Dtos.Products;
using BistroQ.Core.Entities;

namespace BistroQ.Contracts.Services;

public interface IProductService
{
    Task<PaginationResponseDto<IEnumerable<Product>>> GetProductsAsync(ProductCollectionQueryParams productQueryParams);
}
