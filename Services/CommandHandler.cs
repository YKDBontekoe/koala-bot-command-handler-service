using Koala.CommandHandlerService.Models;
using Koala.CommandHandlerService.Services.Handlers;
using Koala.CommandHandlerService.Services.Interfaces;

namespace Koala.CommandHandlerService.Services;

public class CommandHandler : ICommandHandler
{
    public Task<SendMessage> HandleCommandAsync(Message message)
    {
        var commandWithArgs = message.Content[1..].ToUpper().Split(" ");

        if (commandWithArgs.Length == 0)
            return Task.FromResult(CreateMessage("No command specified", message.Channel.Id));
        
        
        var command = commandWithArgs[0].ToUpper();
        if (!RegisteredCommands.Commands.ContainsKey(command))
            return Task.FromResult(CreateMessage($"Command {command} not found", message.Channel.Id));
        
        var commandCreator = RegisteredCommands.Commands[command];
        var commandInstance = commandCreator.CreateCommand();
        var commandResult = commandInstance.Execute(commandWithArgs[1..]);
        
        return Task.FromResult(CreateMessage(commandResult, message.Channel.Id));
    }
    
    private static SendMessage CreateMessage(string content, ulong channelId)
    {
        return new SendMessage
        {
            ChannelId = channelId,
            Content = content
        };
    }
}