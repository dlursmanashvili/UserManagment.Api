using Application.Shared;
using Shared;

namespace Application.Commands.UserCommands
{
    public class RegistrationCommand : Command
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public override async Task<CommandExecutionResult> ExecuteAsync()
        {
            if (!PasswordHelper.IsValidEmail(Email))
            {
                return await Fail("Invalid email address");
            }

            return await userRepository.Registration(new Domain.Entities.UserEntity.User()
            {
                Username = Username,
                Password = Password,
                Email = Email
            });
        }

        
    }
}
