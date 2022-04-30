using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;

namespace Hei.Azure.Test
{
    public class AzureStorageApi : IAzureStorageApi
    {
        private readonly IConfiguration _configuration;
        private readonly AzureStorageConfig _azureStorageConfig;

        public AzureStorageApi(IConfiguration configuration)
        {
            _configuration = configuration;
            _azureStorageConfig = _configuration.GetSection(nameof(AzureStorageConfig)).Get<AzureStorageConfig>();
        }

        public AzureStorageSASResult GenalrateSas()
        {
            var container = new BlobContainerClient(_azureStorageConfig.ConnectionString, _azureStorageConfig.ContainerName);

            if (!container.CanGenerateSasUri)
            {
                throw new Exception("The container can't generate SAS URI");
            }

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = container.Name,
                Resource = "c",
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(_azureStorageConfig.TokenExpirationMinutes)
            };

            //注意这里的 SAS 权限设置为All，在生产中，出于安全原因，请仅设置您需要的权限。对于文件上传，只需要 Create，Add，Write即可。
            sasBuilder.SetPermissions(BlobContainerSasPermissions.All);
            //sasBuilder.SetPermissions(BlobContainerSasPermissions.Create | BlobContainerSasPermissions.Add | BlobContainerSasPermissions.Write);

            var sasUri = container.GenerateSasUri(sasBuilder);

            return new AzureStorageSASResult
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
        }

        public async Task<Stream> BlobDownload()
        {
            var sas = GenalrateSas();
            BlobClient blobClient = new BlobClient(sas.SASUri, new BlobClientOptions
            {
            });

            // Download blob contents to a stream and read the stream.
            BlobDownloadInfo blobDownloadInfo = await blobClient.DownloadAsync();
            return blobDownloadInfo.Content;
        }
    }
}