using System.Linq.Expressions;
using Koala.CommandHandlerService.Models.Activity;
using Koala.CommandHandlerService.Options;
using Koala.CommandHandlerService.Repositories.Interfaces;
using Microsoft.Extensions.Options;

namespace Koala.CommandHandlerService.Repositories;

public class CosmosDbActivitiesRepository : IActivitiesRepository
{
    private readonly IGenericRepository<Activity> _genericRepository;
    private readonly CosmosDbOptions _options;

    public CosmosDbActivitiesRepository(IOptions<CosmosDbOptions> cosmosDbOptions, IGenericRepository<Activity> genericRepository)
    {
        _genericRepository = genericRepository;
        _options = cosmosDbOptions != null ? cosmosDbOptions.Value : throw new ArgumentNullException(nameof(cosmosDbOptions));
    }
    
    public async Task<IEnumerable<Activity>> GetActivitiesAsync(Expression<Func<Activity, bool>> activityPredicate, Expression<Func<Activity, object>>? orderBy = null, Expression<Func<Activity, object>>? groupBy = null,
        bool isDescending = false) =>
        await _genericRepository.GetAsync(_options.MessagesContainerName, activityPredicate, orderBy, groupBy, isDescending);
}