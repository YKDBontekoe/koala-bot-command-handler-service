using Koala.CommandHandlerService.Models;
using Koala.CommandHandlerService.Models.Message;
using Koala.CommandHandlerService.Services.Handlers;
using Koala.CommandHandlerService.Services.Handlers.Interfaces;
using Koala.CommandHandlerService.Services.Interfaces;

namespace Koala.CommandHandlerService.Services;

public class CommandHandler : ICommandHandler
{
    private readonly IHandlerService _handlerService;

    public CommandHandler(IHandlerService handlerService)
    {
        _handlerService = handlerService;
    }

    public async Task<SendMessage> HandleCommandAsync(Message message)
    {
        var commandWithArgs = message.Content[1..].ToUpper().Split(" ");

        if (commandWithArgs.Length is 0)
            return CreateMessage("No command specified", message.Channel.Id);

        var inputCommand = commandWithArgs[0];
        var command = _handlerService.GetCommands().SingleOrDefault(x => x.GetCommandName().Equals(inputCommand, StringComparison.InvariantCultureIgnoreCase));
        if (command is null)
            return CreateMessage($"Command {inputCommand} not found", message.Channel.Id);
        
        var result = await _handlerService.HandleAsync(command.GetCommandName(), GetAllArgumentsWithParameters(commandWithArgs[1..]));
        return CreateMessage(result.HasValue ? result.Value : result.Error.Message , message.Channel.Id);
    }
    
    // Creates a message to send to the send message queue
    private static SendMessage CreateMessage(string content, ulong channelId)
    {
        return new SendMessage
        {
            ChannelId = channelId,
            Content = content
        };
    }
    
    // Splits the arguments into a dictionary with the parameter as key and the value as value
    private static IReadOnlyDictionary<string, string> GetAllArgumentsWithParameters(IReadOnlyList<string> args)
    {
        var argumentsWithParameters = new Dictionary<string, string>();
        for (var i = 0; i < args.Count; i++)
        {
            if (!args[i].StartsWith("-")) continue;
            var argument = args[i].TrimStart('-');
            var parameter = args[i + 1].StartsWith('-') ? string.Empty : args[i + 1];
            argumentsWithParameters.Add(argument, parameter);
        }

        return argumentsWithParameters;
    }
}