using BistroQ.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Contracts.Services;

public interface IAuthService
{
    Task<(bool Success, string Message)> LoginAsync(string username, string password);

    Task RefreshTokenAsync();

    Task LogoutAsync();
}
