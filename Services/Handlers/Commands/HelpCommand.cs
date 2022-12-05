using Koala.CommandHandlerService.DTOs;
using Koala.CommandHandlerService.Services.Handlers.Commands.Interfaces;
using Koala.CommandHandlerService.Services.Handlers.Interfaces;

namespace Koala.CommandHandlerService.Services.Handlers.Commands;

public class HelpCommand : ICommand
{
    private readonly IReadOnlySet<CommandOptionDto> _validArguments = new HashSet<CommandOptionDto>();
    
    private readonly IHandlerService _handlerService;

    public HelpCommand(IHandlerService handlerService)
    {
        _handlerService = handlerService;
    }

    public Task<OptionDto<string>> ExecuteAsync(IReadOnlyDictionary<string, string> argsAndValues)
    {
        return Task.FromResult(new OptionDto<string>(_handlerService.GetCommands()
            .Where(c => !c.GetCommandName().Equals(GetCommandName(), StringComparison.OrdinalIgnoreCase))
            .Select(x => x.GetCommandName())
            .Aggregate((x, y) => $"{x}, {y}")));
    }

    public IReadOnlySet<CommandOptionDto> GetValidArgs()
    {
        return _validArguments;
    }

    public string GetCommandName()
    {
        return "help";
    }

    public string GetCommandDescription()
    {
        return "Returns a list of all available commands";
    }
}