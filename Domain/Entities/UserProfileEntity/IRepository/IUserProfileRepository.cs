using Domain.Entities.UserEntity;
using Shared;

namespace Domain.Entities.UserProfileEntity.IRepository
{
    public interface  IUserProfileRepository
    {
        Task<UserProfile> GetUserProfileByIdAsync(int id);
        Task<IEnumerable<UserProfile>> GetAllUserProfilesAsync();
        Task<CommandExecutionResult> CreateUserProfileAsync(UserProfile userProfile);
        Task<CommandExecutionResult> UpdateUserProfileAsync(UserProfile userProfile);
    }
}
