using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

public class CategoryDataService : ICategoryDataService
{
    private readonly IApiClient _apiClient;

    public CategoryDataService(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    async Task<IEnumerable<Category>> ICategoryDataService.GetAllCategoriesAsync()
    {
        var response = await _apiClient.GetAsync<IEnumerable<Category>>("api/Category", null);

        if (response.Success)
        {
            return response.Data;
        }

        throw new Exception("Failed to get categories");
    }
}