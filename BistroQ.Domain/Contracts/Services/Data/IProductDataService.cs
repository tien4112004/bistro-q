using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services.Data;

public interface IProductDataService
{
    Task<ApiCollectionResponse<IEnumerable<Product>>> GetProductsAsync(ProductCollectionQueryParams query);

    Task<Product> CreateProductAsync(CreateProductRequest request);

    Task<Product> UpdateProductAsync(int productId, UpdateProductRequest request);

    Task<bool> DeleteProductAsync(int productId);
}