using Koala.CommandHandlerService.Services.Handlers.Creators;

namespace Koala.CommandHandlerService.Services.Handlers;

public static class RegisteredCommands
{
    public static readonly IReadOnlyDictionary<string, Creator> Commands = new Dictionary<string, Creator>()
    {
        { "HELP", new HelpCreator() },
        { "TEST", new TestCreator()}
    };
}