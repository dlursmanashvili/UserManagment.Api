using Domain.Entities.UserProfileEntity;
using Domain.Entities.UserProfileEntity.IRepository;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Infrastructure.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(ApplicationDbContext applicationDbContext, IServiceProvider serviceProvider)
            : base(applicationDbContext, serviceProvider)
        {
        }

        public async Task<UserProfile> GetUserProfileByIdAsync(int id)
        {
            return await OfId<UserProfile>(id);
        }

        public async Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync()
        {
            return await _ApplicationDbContext.Set<UserProfile>().ToListAsync();
        }

        public async Task<CommandExecutionResult> CreateUserProfileAsync(UserProfile userProfile)
        {
            try
            {
                var userprofileinfo = _ApplicationDbContext.UserProfiles.FirstOrDefault(x => x.UserId == userProfile.UserId);
                var user = _ApplicationDbContext.Users.FirstOrDefault(x => x.Id == userProfile.UserId && x.IsActive == true);
                if (user.IsNull())
                {
                    return new CommandExecutionResult() { Success = false, ErrorMessage = "user not found" };
                }
                else if (userprofileinfo.IsNotNull())
                {
                    return new CommandExecutionResult() { Success = false, ErrorMessage = $"There is already a record for this user{userprofileinfo.Id}. Please view the record or update it" };
                }
                else
                {
                    await Insert(userProfile);
                    return new CommandExecutionResult() { Success = true};
                }
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<CommandExecutionResult> UpdateUserProfileAsync(UserProfile userProfile)
        {
            try
            {
                var userprofileinfo = _ApplicationDbContext.UserProfiles.FirstOrDefault(x => x.UserId == userProfile.UserId);

                var user = _ApplicationDbContext.Users.FirstOrDefault(x => x.Id == userProfile.UserId && x.IsActive == true);
                if (user.IsNull())
                {
                    return new CommandExecutionResult() { Success = false, ErrorMessage = "user not found" };
                }
                else if (userprofileinfo.IsNull())
                {
                    return new CommandExecutionResult() { Success = false, ErrorMessage = $"record not found" };
                }
                else
                {
                    await Update(userProfile);
                    return new CommandExecutionResult() { Success = true, };
                }
            }
            catch (Exception ex)
            {
                return new CommandExecutionResult()
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task DeleteUserProfileAsync(int id)
        {
            var userProfile = await OfId<UserProfile>(id);
            if (userProfile != null)
            {
                await Delete(userProfile);
            }
        }
    }
}
