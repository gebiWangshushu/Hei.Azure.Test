using Azure.Messaging.EventGrid;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Extensions;
using Passport.Infrastructure;

namespace Hei.Azure.Test
{
    public static class Bootstrapper
    {
        public static IServiceCollection InjectService(this IServiceCollection services)
        {
            services.AddScoped<IAzureStorageApi, AzureStorageApi>();
            return services;
        }

        /// <summary>
        /// 启用一个Service bus事件处理程序在配置更新时刷新 IConfiguration
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="refresher">The refresher.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">serviceBusConfig</exception>
        public static IApplicationBuilder UseAzureConfigChangeEventHandler(this IApplicationBuilder app, IConfigurationRefresher refresher)
        {
            var serviceBusConfig = PassportConfig.Get<AzureServiceBusConfig>(nameof(AzureServiceBusConfig));
            if (serviceBusConfig == null)
            {
                throw new ArgumentNullException(nameof(serviceBusConfig));
            }

            SubscriptionClient serviceBusClient = new SubscriptionClient(serviceBusConfig.ConnectionString, serviceBusConfig.TopicName, serviceBusConfig.SubscriptionName);

            serviceBusClient.RegisterMessageHandler(handler: (message, cancellationToken) =>
                {
                    // 构建一个 EventGridEvent
                    EventGridEvent eventGridEvent = EventGridEvent.Parse(BinaryData.FromBytes(message.Body));

                    // 创建PushNotification
                    eventGridEvent.TryCreatePushNotification(out PushNotification pushNotification);

                    // 刷新IConfiguration
                    refresher.ProcessPushNotification(pushNotification);
                    refresher.TryRefreshAsync();

                    return Task.CompletedTask;
                },
                exceptionReceivedHandler: (exceptionargs) =>
                {
                    Console.WriteLine($"{exceptionargs.Exception}");
                    return Task.CompletedTask;
                }
                );

            return app;
        }
    }
}