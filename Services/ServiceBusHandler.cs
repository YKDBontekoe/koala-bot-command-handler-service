using Azure.Messaging.ServiceBus;
using Koala.CommandHandlerService.Models;
using Koala.CommandHandlerService.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Koala.CommandHandlerService.Services;

public class ServiceBusHandler : IServiceBusHandler
{
    private readonly IConfiguration _configuration;
    private readonly ServiceBusClient _serviceBusClient;
    private ServiceBusProcessor? _processor;
    private readonly ICommandHandler _commandHandler;

    public ServiceBusHandler(IConfiguration configuration, ServiceBusClient serviceBusClient, ICommandHandler commandHandler)
    {
        _configuration = configuration;
        _serviceBusClient = serviceBusClient;
        _commandHandler = commandHandler;
    }

    public async Task InitializeAsync()
    {
        _processor = _serviceBusClient.CreateProcessor(_configuration["ServiceBus:CommandQueueName"], new ServiceBusProcessorOptions
        {
            AutoCompleteMessages = true,
            MaxAutoLockRenewalDuration = TimeSpan.FromMinutes(15),
            PrefetchCount = 100,
        });
        
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
        await args.CompleteMessageAsync(args.Message);
    }
    
    private static Task ErrorHandler(ProcessErrorEventArgs args)
    {
        Console.WriteLine(args.Exception.ToString());
        return Task.CompletedTask;
    }
}