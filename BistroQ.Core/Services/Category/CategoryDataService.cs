using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Entities;

namespace BistroQ.Core.Services;

public class CategoryDataService : ICategoryDataService
{
    IApiClient _apiClient;

    public CategoryDataService(IApiClient apiClient)
    {
        this._apiClient = apiClient;
    }

    async Task<IEnumerable<Category>> ICategoryDataService.GetAllCategoriesAsync()
    {
        var response = await _apiClient.GetAsync<IEnumerable<Category>>("api/Category", null);

        if (response.Success)
        {
            return response.Data;
        }

        return null;
    }
}
