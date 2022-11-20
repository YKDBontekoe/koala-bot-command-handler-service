using Koala.CommandHandlerService.Models;

namespace Koala.CommandHandlerService.Services.Interfaces;

public interface ICommandHandler
{
    Task<SendMessage> HandleCommandAsync(Message message);
}