using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Services.Http;

public class PublicApiClient : BaseApiClient, IPublicApiClient
{
    public PublicApiClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
