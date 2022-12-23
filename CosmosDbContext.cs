using Koala.CommandHandlerService.Models.Activity;
using Koala.CommandHandlerService.Models.Message;
using Koala.CommandHandlerService.Options;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Koala.CommandHandlerService;

public class CosmosDbContext : DbContext
{
    private readonly CosmosDbOptions _cosmosDbOptions;
    
    public CosmosDbContext(DbContextOptions<CosmosDbContext> options, IOptions<CosmosDbOptions> cosmosDbOptions) : base(options)
    {
        _cosmosDbOptions = cosmosDbOptions.Value;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>().ToContainer(_cosmosDbOptions.ActivitiesContainerName);
        modelBuilder.Entity<Message>().ToContainer(_cosmosDbOptions.MessagesContainerName);
    }

    public DbSet<Activity> Activities { get; set; }
    public DbSet<Message> Messages { get; set; }
    
    public DbSet<TEntity> GetDbSet<TEntity>() where TEntity : class
    {
        return Set<TEntity>();
    }
}