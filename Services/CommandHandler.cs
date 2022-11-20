using Koala.CommandHandlerService.Models;
using Koala.CommandHandlerService.Services.Interfaces;

namespace Koala.CommandHandlerService.Services;

public class CommandHandler : ICommandHandler
{
    public Task<SendMessage> HandleCommandAsync(Message message)
    {
        throw new NotImplementedException();
    }
}