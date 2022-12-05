using Koala.CommandHandlerService.Models;

namespace Koala.CommandHandlerService.Services.Interfaces;

public interface IServiceBusHandler
{
    Task InitializeAsync();
    Task? CloseQueueAsync();
    Task? DisposeAsync();
}