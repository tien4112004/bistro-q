using BistroQ.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Services.Http;

public class ApiClient : BaseApiClient, IApiClient
{
    public ApiClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
