using System.Linq.Expressions;
using Koala.CommandHandlerService.Models.Activity;

namespace Koala.CommandHandlerService.Repositories.Interfaces;

public interface IActivitiesRepository
{
    Task<IEnumerable<Activity>> GetActivitiesAsync(Expression<Func<Activity, bool>> activityPredicate, Expression<Func<Activity, object>>? orderBy = null, Expression<Func<Activity, object>>? groupBy = null,
                                                           bool isDescending = false);
}