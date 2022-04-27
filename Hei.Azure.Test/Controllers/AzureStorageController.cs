using Azure.Storage.Blobs;
using Azure.Storage.Sas;
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

        public AzureStorageController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [ProducesResponseType(typeof(PassportApiResult<AzureStorageSASResult>), 200)]
        public IActionResult GenerateSAS()
        {
            var azureStorageConfig = _configuration.GetSection(nameof(AzureStorageConfig)).Get<AzureStorageConfig>();
            BlobContainerClient container = new(azureStorageConfig.ConnectionString, azureStorageConfig.ContainerName);

            if (!container.CanGenerateSasUri)
            {
                return Fail("The container can't generate SAS URI");
            }

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = container.Name,
                Resource = "c",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(azureStorageConfig.TokenExpirationMinutes)
            };

            //注意这里的 SAS 权限设置为All，在生产中，出于安全原因，请仅设置您需要的权限。对于文件上传，只需要 Create，Add，Write即可。
            sasBuilder.SetPermissions(BlobContainerSasPermissions.All);

            var sasUri = container.GenerateSasUri(sasBuilder);

            var result = new AzureStorageSASResult
            {
                AccountName = container.AccountName,
                AccountUrl = $"{container.Uri.Scheme}://{container.Uri.Host}",
                ContainerName = container.Name,
                ContainerUri = container.Uri,
                SASUri = sasUri,
                SASToken = sasUri.Query.TrimStart('?'),
                SASPermission = sasBuilder.Permissions,
                SASExpire = sasBuilder.ExpiresOn
            };

            return Success("Generate Azure Storage Access Signature sucessed.", result);
        }
    }
}