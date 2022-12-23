using Koala.CommandHandlerService.Models;
using Koala.CommandHandlerService.Models.Message;

namespace Koala.CommandHandlerService.Services.Interfaces;

public interface ICommandHandler
{
    Task<SendMessage> HandleCommandAsync(Message message);
}