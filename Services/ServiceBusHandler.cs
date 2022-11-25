using Azure.Messaging.ServiceBus;
using Koala.CommandHandlerService.Models;
using Koala.CommandHandlerService.Options;
using Koala.CommandHandlerService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Koala.CommandHandlerService.Services;

public class ServiceBusHandler : IServiceBusHandler
{
    private readonly ServiceBusProcessor? _processor;
    private readonly ServiceBusClient _serviceBusClient;
    private readonly ICommandHandler _commandHandler;
    private readonly ServiceBusOptions _serviceBusOptions;

    public ServiceBusHandler(ServiceBusClient serviceBusServiceBusClient, ICommandHandler commandHandler, IOptions<ServiceBusOptions> serviceBusOptions)
    {
        _commandHandler = commandHandler;
        _serviceBusClient = serviceBusServiceBusClient;
        _serviceBusOptions = serviceBusOptions != null ? serviceBusOptions.Value : throw new ArgumentNullException(nameof(serviceBusOptions));
        _processor = serviceBusServiceBusClient.CreateProcessor(serviceBusOptions.Value.CommandQueueName, new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = true,
            MaxConcurrentCalls = 1
        });
    }

    public async Task InitializeAsync()
    {
        try
        {
            // add handler to process messages
            _processor.ProcessMessageAsync += MessageHandler;

            // add handler to process any errors
            _processor.ProcessErrorAsync += ErrorHandler;
            await _processor.StartProcessingAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task CloseQueueAsync()
    {
        if (_processor != null) await _processor.CloseAsync();
    }

    public async Task DisposeAsync()
    {
        if (_processor != null) await _processor.DisposeAsync();
    }
    
    private async Task MessageHandler(ProcessMessageEventArgs args)
    {
        var body = args.Message.Body.ToString();
        if (string.IsNullOrEmpty(body)) return;
        
        var message = JsonConvert.DeserializeObject<Message>(body);
        if (message == null) return;
        
        var result = await _commandHandler.HandleCommandAsync(message);
        
        // complete the message. message is deleted from the queue. 
        var sender = _serviceBusClient.CreateSender(_serviceBusOptions.SendMessageQueueName);
        await sender.SendMessageAsync(new ServiceBusMessage(JsonConvert.SerializeObject(result)));
    }
    
    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}