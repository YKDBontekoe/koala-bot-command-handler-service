using Koala.CommandHandlerService.Services.Interfaces;
using Microsoft.Extensions.Hosting;

namespace Koala.CommandHandlerService;

public class CommandHandlerWorker : IHostedService
{
    private readonly IServiceBusHandler _serviceBusHandler;

    public CommandHandlerWorker(IServiceBusHandler serviceBusHandler)
    {
        _serviceBusHandler = serviceBusHandler;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _serviceBusHandler.InitializeAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _serviceBusHandler.DisposeAsync()!;
        await _serviceBusHandler.CloseQueueAsync()!;
    }
}