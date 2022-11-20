using Koala.CommandHandlerService.Services;
using Koala.CommandHandlerService.Services.Interfaces;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Koala.CommandHandlerService;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        var host = Host
            .CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddAzureClients(builder =>
                {
                    builder.AddServiceBusClient(hostContext.Configuration["ServiceBus:ConnectionString"]);
                });

                services.AddSingleton<IServiceBusHandler, ServiceBusHandler>();
                services.AddScoped<ICommandHandler, CommandHandler>();
                services.AddHostedService<CommandHandlerWorker>();
            })
            .UseConsoleLifetime()
            .Build();

        await host.RunAsync();
    }
}