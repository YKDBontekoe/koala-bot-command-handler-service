namespace Koala.CommandHandlerService.Options;

public class ServiceBusOptions
{
    public const string ServiceBus = "ServiceBus";
    
    public string ConnectionString { get; set; } = string.Empty;
    public string CommandQueueName { get; set; } = string.Empty;
}