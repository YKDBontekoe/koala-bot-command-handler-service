using Koala.CommandHandlerService.Models;

namespace Koala.CommandHandlerService.Services.Handlers;

public interface ICommand
{
    string Execute(string[] args);
    
    IReadOnlySet<string> GetValidArgs();
}