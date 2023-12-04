using Infrastructure.Db;
using Shared;

namespace Application.Shared
{
    public abstract class Query<TQueryResult> where TQueryResult : class
    {
        protected ApplicationDbContext _appContext;
        protected IServiceProvider? ServiceProvider;

        protected ApplicationDbContext ApplicationContext
        {
            get { return _appContext; }
        }
        public abstract Task<QueryExecutionResult<TQueryResult>> Execute();


        protected Task<QueryExecutionResult<TQueryResult>> Ok(TQueryResult data)
        {
            var result = new QueryExecutionResult<TQueryResult>
            {
                Data = data,
                Success = true
            };

            return Task.FromResult(result);
        }

        public void Resolve(
         ApplicationDbContext appContext,
         IServiceProvider serviceProvider)
        {

            _appContext = appContext;
            ServiceProvider = serviceProvider;
        }
        protected Task<QueryExecutionResult<TQueryResult>> Fail(params string[] errorMessages)
        {
            var result = new QueryExecutionResult<TQueryResult>
            {
                Success = false
            };

            if (errorMessages != null)
            {
                result.Errors = errorMessages.Select(x => new Error
                {
                    Code = 0,
                    Message = x
                });
            }

            return Task.FromResult(result);
        }
    }
}
