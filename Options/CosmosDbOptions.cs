namespace Koala.CommandHandlerService.Options;

public class CosmosDbOptions
{
    public const string CosmosDb = "CosmosDb";
    
    public string DatabaseName { get; set; } = string.Empty;
    public string MessagesContainerName { get; set; } = string.Empty;
    public string ActivitiesContainerName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
}