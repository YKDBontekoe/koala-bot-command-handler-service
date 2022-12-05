using System.Linq.Expressions;
using Koala.ActivityConsumerService.Models;

namespace Koala.CommandHandlerService.Repositories.Interfaces;

public interface IActivitiesRepository
{
    Task<IEnumerable<Activity>> GetActivitiesAsync(Expression<Func<Activity, bool>> activityPredicate, Expression<Func<Activity, object>>? orderBy = null, Expression<Func<Activity, object>>? groupBy = null,
                                                           bool isDescending = false);
}