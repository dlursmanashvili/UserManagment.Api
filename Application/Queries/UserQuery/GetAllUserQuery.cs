using Application.Shared;
using Shared;

namespace Application.Queries.UserQuery
{
    public class GetAllUserQuery : Query<GetAllUserQueryResult>
    {
        public override async Task<QueryExecutionResult<GetAllUserQueryResult>> Execute()
        {

            var result = _appContext.Users?.Select(x => new UserQueryResultItem()
            {
                Email = x.Email,
                IsActive = x.IsActive,
                Username = x.Username
            })?.ToList();


            var response = new GetAllUserQueryResult();
            response.Result = result;
            return await Ok(response);
        }
    }

    public class UserQueryResultItem
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }

    public class GetAllUserQueryResult
    {
        public List<UserQueryResultItem>? Result { get; set; }
    }
}
