using BistroQ.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Contracts.Services;
public interface ITokenStorageService
{
    Task SaveTokensAsync(AuthResult result);

    Task<string> GetAccessToken();

    Task<string> GetRefreshToken();

    Task ClearTokensAsync();
}
