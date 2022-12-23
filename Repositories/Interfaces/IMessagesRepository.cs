using System.Linq.Expressions;
using Koala.CommandHandlerService.Models;
using Koala.CommandHandlerService.Models.Message;

namespace Koala.CommandHandlerService.Repositories.Interfaces;

public interface IMessagesRepository
{
    Task<IEnumerable<Message>> GetMessagesAsync(Expression<Func<Message, bool>> activityPredicate, Expression<Func<Message, object>>? orderBy = null, Expression<Func<Message, object>>? groupBy = null,
        bool isDescending = false);
}