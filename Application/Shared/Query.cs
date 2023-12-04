using Infrastructure.Db;
using Shared;

namespace Application.Shared
{
    public abstract class Query<TQueryResult> : ResponseHelper where TQueryResult : class
    {
        protected ApplicationDbContext _appContext;

        protected ApplicationDbContext ApplicationContext
        {
            get { return _appContext; }
        }
        public abstract Task<QueryExecutionResult<TQueryResult>> Execute();
    }
}
