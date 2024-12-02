using BistroQ.Core.Entities;

namespace BistroQ.Core.Contracts.Services;

public interface ICategoryDataService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
}
