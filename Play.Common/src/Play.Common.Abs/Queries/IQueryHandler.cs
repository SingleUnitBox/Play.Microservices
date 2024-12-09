namespace Play.Common.Abs.Queries;

public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> QueryAsync(TQuery query);
}