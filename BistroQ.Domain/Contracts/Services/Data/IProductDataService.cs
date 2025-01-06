using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Products;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services.Data;

/// <summary>
/// Interface for managing product-related data operations through API endpoints.
/// Provides methods for CRUD operations on products, including image handling and recommendations.
/// </summary>
public interface IProductDataService
{
    /// <summary>
    /// Retrieves a paginated collection of products based on query parameters.
    /// </summary>
    /// <param name="query">Query parameters for filtering, sorting, and pagination</param>
    /// <returns>A collection response containing products and pagination information</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the message "Failed to get products"</exception>
    Task<ApiCollectionResponse<IEnumerable<Product>>> GetProductsAsync(ProductCollectionQueryParams query);

    /// <summary>
    /// Creates a new product without an image.
    /// </summary>
    /// <param name="request">The product creation details</param>
    /// <returns>The newly created product</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Product> CreateProductAsync(CreateProductRequest request);

    /// <summary>
    /// Creates a new product with an optional image file.
    /// </summary>
    /// <param name="request">The product creation details</param>
    /// <param name="Stream">The image file stream</param>
    /// <param name="FileName">The name of the image file</param>
    /// <param name="ContentType">The MIME type of the image file</param>
    /// <returns>The newly created product</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Product> CreateProductAsync(CreateProductRequest request, Stream Stream, string FileName, string ContentType);

    /// <summary>
    /// Updates an existing product's details.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to update</param>
    /// <param name="request">The product update details</param>
    /// <returns>The updated product</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Product> UpdateProductAsync(int productId, UpdateProductRequest request);

    /// <summary>
    /// Updates a product's image.
    /// </summary>
    /// <param name="productId">The unique identifier of the product</param>
    /// <param name="Stream">The new image file stream</param>
    /// <param name="FileName">The name of the new image file</param>
    /// <param name="ContentType">The MIME type of the new image file</param>
    /// <returns>The updated product</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Product> UpdateProductImageAsync(int productId, Stream Stream, string FileName, string ContentType);

    /// <summary>
    /// Deletes a product with the specified ID.
    /// </summary>
    /// <param name="productId">The unique identifier of the product to delete</param>
    /// <returns>True if the deletion was successful</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<bool> DeleteProductAsync(int productId);

    /// <summary>
    /// Retrieves a collection of recommended products.
    /// </summary>
    /// <returns>A collection response containing recommended products and pagination information</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the message "Failed to get products"</exception>
    Task<ApiCollectionResponse<IEnumerable<Product>>> GetRecommendationAsync();
}