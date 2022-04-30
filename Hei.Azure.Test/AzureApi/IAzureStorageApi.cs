namespace Hei.Azure.Test
{
    public interface IAzureStorageApi
    {
        AzureStorageSASResult GenalrateSas();

        Task<Stream> BlobDownload();
    }
}