namespace Koala.CommandHandlerService.Services.Handlers.Commands;

public class TestCommand : ICommand
{
    public string Execute(string[] args)
    {
        return "TestCommand";
    }

    public IReadOnlySet<string> GetValidArgs()
    {
        throw new NotImplementedException();
    }
}