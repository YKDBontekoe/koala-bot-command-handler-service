using Koala.CommandHandlerService.Options;
using Koala.CommandHandlerService.Repositories;
using Koala.CommandHandlerService.Repositories.Interfaces;
using Koala.CommandHandlerService.Services;
using Koala.CommandHandlerService.Services.Handlers;
using Koala.CommandHandlerService.Services.Handlers.Interfaces;
using Koala.CommandHandlerService.Services.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Koala.CommandHandlerService;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment;

                    builder
                        .SetBasePath(env.ContentRootPath)
                        .AddJsonFile("appsettings.json", true, true)
                        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();
                }
            )
            .ConfigureServices((hostContext, services) =>
            {
                ConfigureOptions(services, hostContext.Configuration);
                ConfigureCosmosDb(services);
                ConfigureServiceBus(services);
                ConfigureServices(services);
            })
            .UseConsoleLifetime()
            .Build();

        await host.RunAsync();
    }
    
    // Configure options for the application to use in the services
    private static void ConfigureOptions(IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions();
        services.Configure<ServiceBusOptions>(configuration.GetSection(ServiceBusOptions.ServiceBus));
        services.Configure<CosmosDbOptions>(configuration.GetSection(CosmosDbOptions.CosmosDb));
        services.Configure<DiscordOptions>(configuration.GetSection(DiscordOptions.Discord));
    }

    // Configure the Azure Service Bus client with the connection string
    private static void ConfigureServiceBus(IServiceCollection services)
    {
        services.AddAzureClients(builder =>
        {
            builder.AddServiceBusClient(services.BuildServiceProvider().GetRequiredService<IOptions<ServiceBusOptions>>().Value.ConnectionString);
        });
    }

    // Configure the Azure Cosmos DB client with the connection string
    private static void ConfigureCosmosDb(IServiceCollection services)
    {
        services.AddDbContext<CosmosDbContext>(options =>
        {
            options.UseCosmos(services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbOptions>>().Value.EndpointUri,
                services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbOptions>>().Value.AccountKey,
                services.BuildServiceProvider().GetRequiredService<IOptions<CosmosDbOptions>>().Value.DatabaseName);
        });
    }
    
    // Configure services for the application
    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(CosmosDbGenericRepository<>));
        services.AddScoped<IActivitiesRepository, CosmosDbActivitiesRepository>();
        services.AddScoped<IMessagesRepository, CosmosDbMessagesRepository>();
        services.AddScoped<IHandlerService, HandlerService>();
        services.AddScoped<ICommandHandler, CommandHandler>();
        services.AddTransient<IServiceBusHandler, ServiceBusHandler>();

        services.AddScoped<IHostedService, CommandHandlerWorker>();
    }
}