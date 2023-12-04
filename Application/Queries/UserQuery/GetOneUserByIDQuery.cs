using Application.Shared;
using Shared;

namespace Application.Queries.UserQuery;

public class GetOneUserByIDQuery : Query<GetOneUserByIDQueryResult>
{
    public int UserID { get; set; }
    public override async Task<QueryExecutionResult<GetOneUserByIDQueryResult>> Execute()
    {
        var result = _appContext.Users.FirstOrDefault(x => x.Id ==UserID);

        if (result.IsNull())
        {
            return await Fail("user not found");
        }
        else
        {
            return await Ok(new GetOneUserByIDQueryResult()
            {
                Email = result.Email,
                IsActive = result.IsActive,
                Username = result.Username
            });
        }


    }
}

public class GetOneUserByIDQueryResult : UserQueryResultItem
{
}