using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Passport.Infrastructure;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Hei.Azure.Test.Controllers
{
    [Route("api/azure/storage/[action]")]
    public class AzureStorageController : PassportApiController
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly AzureStorageConfig _azureStorageConfig;
        private readonly IAzureStorageApi _azureStorageApi;

        public AzureStorageController(IConfiguration configuration, IAzureStorageApi azureStorageApi)
        {
            _configuration = configuration;
            _azureStorageConfig = _configuration.GetSection(nameof(AzureStorageConfig)).Get<AzureStorageConfig>();
            _azureStorageApi = azureStorageApi;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PassportApiResult<AzureStorageSASResult>), 200)]
        public IActionResult GenerateSAS()
        {
            var result = _azureStorageApi.GenalrateSas();

            return Success("Generate Azure Storage Access Signature sucessed.", result);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> BlobDownload()
        {
            var result = await _azureStorageApi.BlobDownload();

            return File(result, "application/octet-stream");
        }
    }
}