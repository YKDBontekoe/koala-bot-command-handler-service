using Koala.CommandHandlerService.DTOs;

namespace Koala.CommandHandlerService.Services.Handlers.Commands.Interfaces;

public interface ICommand
{
    Task<OptionDto<string>> ExecuteAsync(IReadOnlyDictionary<string, string> argsAndValues);
    
    IReadOnlySet<CommandOptionDto> GetValidArgs();
    
    string GetCommandName();
    
    string GetCommandDescription();
}