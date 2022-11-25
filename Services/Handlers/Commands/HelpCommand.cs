using Koala.CommandHandlerService.Models;

namespace Koala.CommandHandlerService.Services.Handlers.Commands;

public class HelpCommand : ICommand
{
    private readonly IReadOnlySet<string> _validArguments = new HashSet<string> { "test" };
    
    public string Execute(string[] args)
    {
        return RegisteredCommands.Commands
            .Where(c => c.Key.Equals("help", StringComparison.OrdinalIgnoreCase) == false)
            .Select(x => x.Key)
            .Aggregate((x, y) => $"{x}, {y}");
    }

    public IReadOnlySet<string> GetValidArgs()
    {
        return _validArguments;
    }
}