using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Passport.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hei.Azure.Test.Controllers
{
    [Route("api/azure/config/[action]")]
    public class AzureConfigController : PassportApiController
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public AzureConfigController(IConfiguration configuration, IAzureStorageApi azureStorageApi)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get(string key)
        {
            var result = _configuration[key];

            return Success("get config success", result);
        }

        [HttpGet]
        public async Task<IActionResult> GetObj(string key)
        {
            var result = _configuration.GetSection(key).Get<AzureStorageConfig>();

            return Success("get config success", result);
        }
    }
}