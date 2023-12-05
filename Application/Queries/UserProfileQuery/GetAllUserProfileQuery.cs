using Application.Shared;
using Domain.Entities.UserProfileEntity;
using Shared;

namespace Application.Queries.UserProfileQuery
{
    public class GetAllUserProfileQuery : Query<List<UserProfile>?>
    {
        public override async Task<QueryExecutionResult<List<UserProfile>?>> Execute()
        {
            if (!_appContext.UserProfiles.Any() || !_appContext.Users.Any())
            {
                return await Fail("records not found");
            }
            
            var activeUserProfiles = _appContext.Users
            .Where(user => user.IsActive)?
            .Select(user => user.Profile)
            ?.ToList();

            return await Ok(activeUserProfiles);
        }
    }
}
