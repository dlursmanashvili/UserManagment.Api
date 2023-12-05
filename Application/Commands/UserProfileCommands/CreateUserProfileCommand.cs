using Application.Shared;
using Shared;

namespace Application.Commands.UserProfileCommands
{
    public class CreateUserProfileCommand : Command
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PersonalNumber { get; set; }
        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            return await userProfileRepository.CreateUserProfileAsync(new Domain.Entities.UserProfileEntity.UserProfile()
            {
                UserId = UserId,
                FirstName = FirstName,
                LastName = LastName,
                PersonalNumber = PersonalNumber,
            });


        }
    }
}
