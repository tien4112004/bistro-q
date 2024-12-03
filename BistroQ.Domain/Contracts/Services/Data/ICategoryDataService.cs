using BistroQ.Domain.Models.Entities;

namespace BistroQ.Domain.Contracts.Services.Data;

public interface ICategoryDataService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
}