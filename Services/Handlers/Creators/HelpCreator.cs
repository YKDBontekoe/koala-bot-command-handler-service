using Koala.CommandHandlerService.Services.Handlers.Commands;

namespace Koala.CommandHandlerService.Services.Handlers.Creators;

public class HelpCreator : Creator
{
    public override ICommand CreateCommand() => new HelpCommand();
}