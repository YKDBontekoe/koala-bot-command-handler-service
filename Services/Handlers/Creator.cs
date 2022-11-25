using Koala.CommandHandlerService.Models;

namespace Koala.CommandHandlerService.Services.Handlers;

public abstract class Creator
{
    public abstract ICommand CreateCommand();
    
    public string Execute(string[] args)
    {
        var command = CreateCommand();
        return command.Execute(args);
    }
}