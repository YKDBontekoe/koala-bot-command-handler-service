using System.Reflection;
using Koala.CommandHandlerService.Services.Handlers.Commands.Interfaces;
using Koala.CommandHandlerService.Services.Handlers.Interfaces;
using Koala.CommandHandlerService.DTOs;

namespace Koala.CommandHandlerService.Services.Handlers;

public class HandlerService : IHandlerService
{
    private readonly IServiceProvider _serviceProvider;

    public HandlerService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<OptionDto<string>>  HandleAsync(string command, IReadOnlyDictionary<string, string> argsAndValues)
    {
        var handler = GetCommandTypeByName(command);
        if (!handler.HasValue)
        {
            return new OptionDto<string>(handler.Error);
        }
        
        var isValid = AreAllArgsValid(handler.Value, argsAndValues);
        if (!isValid.HasValue)
        {
            return new OptionDto<string>(isValid.Error);
        }

        return await handler.Value.ExecuteAsync(argsAndValues);
    }

    // Get all commands
    public IEnumerable<ICommand> GetCommands()
    {
        var types = Assembly.GetExecutingAssembly().GetTypes();
        var commandTypes = (from type in types where type.GetInterfaces().Contains(typeof(ICommand)) select (ICommand)Activator.CreateInstance(type, GetInjectedServices(type))).ToList();

        return commandTypes;
    }

    // Get a command type by name that implements ICommand
    private static OptionDto<bool> AreAllArgsValid(ICommand handler, IReadOnlyDictionary<string, string> argsAndValues)
    {
        var validArgs = handler.GetValidArgs();
        for (var i = 0; i < argsAndValues.Count; i++)
        {
            var arg = argsAndValues.ElementAt(i).Key;
            if (!validArgs.Any(x => x.Name.Equals(arg, StringComparison.InvariantCultureIgnoreCase)))
            {
                return new OptionDto<bool>(new Exception($"Argument {arg} is not valid for command {handler.GetCommandName()}"));
            }
        }
        
        return new OptionDto<bool>(true);
    }

    // Get the command handler from the service provider
    private OptionDto<ICommand> GetCommandTypeByName(string command)
    {
        var allCommands = GetCommands();
        var commandType = allCommands.FirstOrDefault(c => c.GetCommandName().Equals(command, StringComparison.OrdinalIgnoreCase));
        return commandType == null ? new OptionDto<ICommand>(new Exception($"Command not found: {command}")) : new OptionDto<ICommand>(commandType);
    }

    // get injected services from the constructor of the command by using _serviceProvider
    private object?[] GetInjectedServices(Type commandType)
    {
        var constructor = commandType.GetConstructors().First();
        var parameters = constructor.GetParameters();
        return parameters.Select(parameter => _serviceProvider.GetService(parameter.ParameterType)).ToArray();
    }
}