using System.Linq.Expressions;

namespace Koala.CommandHandlerService.Repositories.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAsync(string containerName, Expression<Func<T, bool>> activityPredicate, Expression<Func<T, object>>? orderBy = null, Expression<Func<T, object>>? groupBy = null,
        bool isDescending = false);

}