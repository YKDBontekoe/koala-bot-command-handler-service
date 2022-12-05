using Koala.CommandHandlerService.DTOs;
using Koala.CommandHandlerService.Services.Handlers.Commands.Interfaces;

namespace Koala.CommandHandlerService.Services.Handlers.Interfaces;

public interface IHandlerService
{
    Task<OptionDto<string>> HandleAsync(string command, IReadOnlyDictionary<string, string> argsAndValues);
    
    IEnumerable<ICommand> GetCommands();
}