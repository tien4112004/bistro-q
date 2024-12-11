using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Contracts.Services.Data;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Category;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

public class CategoryDataService : ICategoryDataService
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;
    public CategoryDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<Category> CreateCategoryAsync(CreateCategoryRequest request)
    {
        var response = await _apiClient.PostAsync<CategoryResponse>("api/admin/category", request);
        if (response.Success)
        {
            return _mapper.Map<Category>(response.Data);
        }

        throw new Exception(response.Message);
    }

    public async Task<Category> UpdateCategoryAsync(int categoryId, UpdateCategoryRequest request)
    {
        var response = await _apiClient.PutAsync<CategoryResponse>($"api/admin/category/{categoryId}", request);

        if (response.Success)
        {
            return _mapper.Map<Category>(response.Data);
        }

        throw new Exception(response.Message);
    }

    public async Task<bool> DeleteCategoryAsync(int categoryId)
    {
        var response = await _apiClient.DeleteAsync<object>($"api/admin/category/{categoryId}", null);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }

        return true;
    }

    public async Task<ApiCollectionResponse<IEnumerable<Category>>> GetCategoriesAsync(CategoryCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<CategoryResponse>>("/api/category", query);

        if (response.Success)
        {
            var categories = _mapper.Map<IEnumerable<Category>>(response.Data);

            return new ApiCollectionResponse<IEnumerable<Category>>
                (categories, response.TotalItems, response.CurrentPage, response.TotalPages);
        }

        throw new Exception("Failed to get categories");
    }

    public async Task<Category> GetCategoryByIdAsync(int categoryId)
    {
        var response = await _apiClient.GetAsync<CategoryResponse>($"api/category/{categoryId}", null);

        if (response.Success)
        {
            return _mapper.Map<Category>(response.Data);
        }

        throw new Exception(response.Message);
    }
}