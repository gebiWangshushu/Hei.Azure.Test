namespace Hei.Azure.Test
{
    public static class Bootstrapper
    {
        public static IServiceCollection InjectService(this IServiceCollection services)
        {
            services.AddScoped<IAzureStorageApi, AzureStorageApi>();
            return services;
        }
    }
}