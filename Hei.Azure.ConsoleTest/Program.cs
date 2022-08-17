using Azure.Messaging.EventGrid;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Extensions;
using Microsoft.Extensions.Configuration.Json;

class Program
{
    private const string AppConfigurationConnectionStringEnvVarName = "AppConfigurationConnectionString";
    // e.g. Endpoint=https://{store_name}.azconfig.io;Id={id};Secret={secret}

    private const string ServiceBusConnectionStringEnvVarName = "ServiceBusConnectionString";
    // e.g. Endpoint=sb://{service_bus_name}.servicebus.windows.net/;SharedAccessKeyName={key_name};SharedAccessKey={key}

    private const string ServiceBusTopicEnvVarName = "ServiceBusTopic";
    private const string ServiceBusSubscriptionEnvVarName = "ServiceBusSubscription";

    private static IConfigurationRefresher _refresher = null;

    static async Task Main(string[] args)
    {
        string appConfigurationConnectionString = Environment.GetEnvironmentVariable(AppConfigurationConnectionStringEnvVarName);

        IConfiguration configuration = new ConfigurationBuilder()
            .AddAzureAppConfiguration(options =>
            {
                options.Connect(appConfigurationConnectionString);
                options.ConfigureRefresh(refresh =>
                    refresh
                        .Register("TestApp:Settings:Message")
                        .SetCacheExpiration(TimeSpan.FromDays(10))  // Important: Reduce poll frequency
                );

                _refresher = options.GetRefresher();
            }).Build();

        RegisterRefreshEventHandler();
        var message = configuration["TestApp:Settings:Message"];
        Console.WriteLine($"Initial value: {configuration["TestApp:Settings:Message"]}");

        while (true)
        {
            await _refresher.TryRefreshAsync();

            if (configuration["TestApp:Settings:Message"] != message)
            {
                Console.WriteLine($"New value: {configuration["TestApp:Settings:Message"]}");
                message = configuration["TestApp:Settings:Message"];
            }

            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }

    private static void RegisterRefreshEventHandler()
    {
        string serviceBusConnectionString = Environment.GetEnvironmentVariable(ServiceBusConnectionStringEnvVarName);
        string serviceBusTopic = Environment.GetEnvironmentVariable(ServiceBusTopicEnvVarName);
        string serviceBusSubscription = Environment.GetEnvironmentVariable(ServiceBusSubscriptionEnvVarName);
        SubscriptionClient serviceBusClient = new SubscriptionClient(serviceBusConnectionString, serviceBusTopic, serviceBusSubscription);

        serviceBusClient.RegisterMessageHandler(
            handler: (message, cancellationToken) =>
            {
                // Build EventGridEvent from notification message
                EventGridEvent eventGridEvent = EventGridEvent.Parse(BinaryData.FromBytes(message.Body));

                // Create PushNotification from eventGridEvent
                eventGridEvent.TryCreatePushNotification(out PushNotification pushNotification);

                // Prompt Configuration Refresh based on the PushNotification
                _refresher.ProcessPushNotification(pushNotification);

                return Task.CompletedTask;
            },
            exceptionReceivedHandler: (exceptionargs) =>
            {
                Console.WriteLine($"{exceptionargs.Exception}");
                return Task.CompletedTask;
            });
    }
}