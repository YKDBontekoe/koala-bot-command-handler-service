using System.Linq.Expressions;
using Koala.CommandHandlerService.Models;
using Koala.CommandHandlerService.Models.Message;
using Koala.CommandHandlerService.Options;
using Koala.CommandHandlerService.Repositories.Interfaces;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Microsoft.Extensions.Options;

namespace Koala.CommandHandlerService.Repositories;

public class CosmosDbMessagesRepository : IMessagesRepository
{
    private readonly IGenericRepository<Message> _genericRepository;
    private readonly CosmosDbOptions _options;

    public CosmosDbMessagesRepository(IOptions<CosmosDbOptions> cosmosDbOptions, IGenericRepository<Message> genericRepository)
    {
        _genericRepository = genericRepository;
        _options = cosmosDbOptions != null ? cosmosDbOptions.Value : throw new ArgumentNullException(nameof(cosmosDbOptions));
    }

    public async Task<IEnumerable<Message>> GetMessagesAsync(Expression<Func<Message, bool>> messagePredicate, Expression<Func<Message, object>>? orderBy = null, Expression<Func<Message, object>>? groupBy = null,
        bool isDescending = false) =>
        await _genericRepository.GetAsync(_options.MessagesContainerName, messagePredicate, orderBy, groupBy, isDescending);
}