using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Dtos;

public class LoginResult
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string UserId { get; set; }
    public string Role { get; set; }
}
