using BistroQ.Core.Entities;

namespace BistroQ.Contracts.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
}
