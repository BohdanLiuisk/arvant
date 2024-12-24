using Arvant.Entity.SelectQuery.Models;

namespace Arvant.Entity.Abstract;

public interface ISelectQueryExecutor
{
    Task<string> ExecuteAsync(SelectQueryConfig selectConfig);

    Task<SelectQueryResult<T>?> ExecuteAsync<T>(SelectQueryConfig selectConfig);
}
