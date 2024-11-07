using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Models.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Services;

public class OrderDataService : IOrderDataService
{
    public IApiClient apiClient;

    public OrderDataService(IApiClient apiClient)
    {
        this.apiClient = apiClient;
    }

    public async Task<Order> CreateOrderAsync()
    {
        var response = await apiClient.PostAsync<Order>("api/ClientOrder", null);

        if (response.Success)
        {
            return response.Data;
        }
        else
        {
            throw new Exception(response.Message);
        }
    }

    public async Task<Order?> GetOrderAsync()
    {
        var resposne = await apiClient.GetAsync<Order>("api/ClientOrder", null);

        if (resposne.Success)
        {
            return resposne.Data;
        }
        else return null;
    }

    public async Task DeleteOrderAsync()
    {
        var response = await apiClient.DeleteAsync<Order>("api/ClientOrder", null);

        if (!response.Success)
        {
            throw new Exception(response.Message);
        }
    }

    public async Task<Order> GetOrderByCashierAsync(int tableId)
    {
        var response = await apiClient.GetAsync<Order>($"api/CashierOrder/{tableId}", null);

        if (response.Success)
        {
            return response.Data;
        }
        else
        {
            return null;
        }
    }

    public async Task<IEnumerable<Order>> GetCurrentOrdersByCashierAsync(int? zoneId)
    {
        ApiResponse<IEnumerable<Order>> response;

        if (zoneId == null)
        {
            response = await apiClient.GetAsync<IEnumerable<Order>>($"api/CashierOrder", null);
        }
        else
        {
            response = await apiClient.GetAsync<IEnumerable<Order>>($"api/CashierOrder/zones/{zoneId}", null);
        }

        if (response.Success)
        {
            return response.Data;
        }
        else
        {
            return new List<Order>();
        }
    }
}
