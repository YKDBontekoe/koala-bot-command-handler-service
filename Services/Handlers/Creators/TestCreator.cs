using Koala.CommandHandlerService.Services.Handlers.Commands;

namespace Koala.CommandHandlerService.Services.Handlers.Creators;

public class TestCreator : Creator
{
    public override ICommand CreateCommand() => new TestCommand();
}