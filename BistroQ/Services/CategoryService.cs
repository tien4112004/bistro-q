using BistroQ.Contracts.Services;
using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Entities;

namespace BistroQ.Services;

class CategoryService : ICategoryService
{
    private readonly ICategoryDataService _categoryDataService;

    public CategoryService(ICategoryDataService categoryDataService)
    {
        this._categoryDataService = categoryDataService;
    }

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return await _categoryDataService.GetAllCategoriesAsync();
    }
}
