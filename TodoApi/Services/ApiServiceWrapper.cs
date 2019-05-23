using ExLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Services
{
    public class ApiServiceWrapper
    {
        private readonly IApiService _apiService;

        public ApiServiceWrapper(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<string> getApiHelloService()
        {
            string apiResult = _apiService.getHelloWorldService();
            return await Task<string>.FromResult(apiResult);
        }
         
    }
}
