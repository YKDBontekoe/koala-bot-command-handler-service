using System.Linq.Expressions;
using Koala.CommandHandlerService.Options;
using Koala.CommandHandlerService.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;

namespace Koala.CommandHandlerService.Repositories;

public class CosmosDbGenericRepository<T> : IGenericRepository<T> where T : class

{
    private readonly CosmosClient _database;
    private readonly CosmosDbOptions _options;

    public CosmosDbGenericRepository(IOptions<CosmosDbOptions> cosmosDbOptions, CosmosClient database)
    {
        _database = database;
        _options = cosmosDbOptions != null ? cosmosDbOptions.Value : throw new ArgumentNullException(nameof(cosmosDbOptions));
    }

    public async Task<IEnumerable<T>> GetAsync(string containerName, Expression<Func<T, bool>> activityPredicate, Expression<Func<T, object>>? orderBy = null, Expression<Func<T, object>>? groupBy = null,
        bool isDescending = false)
    {
        var query = _database.GetContainer(_options.DatabaseName, containerName)
            .GetItemLinqQueryable<T>(true).AsEnumerable().AsQueryable();
           // .Where(activityPredicate);

        if (orderBy != null) 
            query = OrderBy(query, orderBy, isDescending);

        var result =  await query.ToFeedIterator()
                .ReadNextAsync();

        if (groupBy == null)
            return await query.ToFeedIterator()
                .ReadNextAsync();
        
        var groupQuery = result.GroupBy(groupBy.Compile());
        return OrderGroupedBy(groupQuery, isDescending);
    }

    private static IEnumerable<T> OrderGroupedBy(IEnumerable<IGrouping<object, T>> groupedQuery, bool isDescending)
    {
        var sortedGroup =  isDescending ? groupedQuery.OrderByDescending((x) => x.Count()) : groupedQuery.OrderBy((x) => x.Count());
        return sortedGroup.SelectMany(x => x);
    }
    
    private static IQueryable<T> OrderBy(IQueryable<T> query, Expression<Func<T, object>> orderBy, bool isDescending)
    {
        return isDescending ? query.OrderByDescending(orderBy) : query.OrderBy(orderBy);
    }
}