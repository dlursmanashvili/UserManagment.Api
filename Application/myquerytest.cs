using Application.Shared;
using Shared;

namespace Application
{
    internal class myquerytest : Query<GetAsyncLogsQueryResult>
    {
        public override Task<QueryExecutionResult<GetAsyncLogsQueryResult>> Execute()
        {            
            throw new NotImplementedException();
        }
    }

    internal class GetAsyncLogsQueryResult
    {
    }
}
