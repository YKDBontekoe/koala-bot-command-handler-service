namespace Koala.CommandHandlerService.Services.Interfaces;

public interface IServiceBusHandler
{
    public Task InitializeAsync();
    Task? CloseQueueAsync();
    Task? DisposeAsync();
}