using Application.Shared;
using Domain.Entities.UserProfileEntity;
using Shared;

namespace Application.Queries.UserProfileQuery
{
    public class GetOneUserProfileQuery : Query<UserProfile?>
    {
        public int UserId { get; set; }
        public override async Task<QueryExecutionResult<UserProfile?>> Execute()
        {
            if (_appContext.Users.FirstOrDefault(x=> x.Id ==UserId && x.IsActive == true).IsNull())
            {
                return await Fail("user not found");
            }
            var result = _appContext.UserProfiles.FirstOrDefault(x => x.Id == UserId);
            if (result.IsNull())
            {
                return await Fail("record not found");
            }

            return await Ok(result);

        }
    }
}
