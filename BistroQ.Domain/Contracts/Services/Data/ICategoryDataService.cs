using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services.Data;

public interface ICategoryDataService
{
    Task<ApiCollectionResponse<IEnumerable<Category>>> GetCategoriesAsync(CategoryCollectionQueryParams query = null);

    Task<Category> GetCategoryByIdAsync(int categoryId);

    Task<Category> CreateCategoryAsync(CreateCategoryRequest category);

    Task<Category> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest category);

    Task<bool> DeleteCategoryAsync(int categoryId);
}