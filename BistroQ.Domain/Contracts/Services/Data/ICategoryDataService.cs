using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services.Data;

/// <summary>
/// Interface for managing category-related data operations through API endpoints.
/// Provides methods for CRUD operations on product categories.
/// </summary>
public interface ICategoryDataService
{
    /// <summary>
    /// Retrieves a paginated collection of categories based on query parameters.
    /// </summary>
    /// <param name="query">Optional query parameters for filtering, sorting, and pagination</param>
    /// <returns>A collection response containing categories and pagination information</returns>
    /// <exception cref="Exception">Thrown when the API request fails</exception>
    Task<ApiCollectionResponse<IEnumerable<Category>>> GetCategoriesAsync(CategoryCollectionQueryParams query = null);

    /// <summary>
    /// Retrieves a specific category by its ID.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category</param>
    /// <returns>The category details for the specified ID</returns>
    /// <exception cref="Exception">Thrown when the API request fails</exception>
    Task<Category> GetCategoryByIdAsync(int categoryId);

    /// <summary>
    /// Creates a new category with the provided information.
    /// </summary>
    /// <param name="category">The category creation details</param>
    /// <returns>The newly created category</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Category> CreateCategoryAsync(CreateCategoryRequest category);

    /// <summary>
    /// Updates an existing category with the provided information.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category to update</param>
    /// <param name="category">The category update details</param>
    /// <returns>The updated category</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<Category> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest category);

    /// <summary>
    /// Deletes a category with the specified ID.
    /// </summary>
    /// <param name="categoryId">The unique identifier of the category to delete</param>
    /// <returns>True if the deletion was successful</returns>
    /// <exception cref="Exception">Thrown when the API request fails with the error message</exception>
    Task<bool> DeleteCategoryAsync(int categoryId);
}
