using Application.Shared;
using Shared;

namespace Application.Commands.UserCommands;

public class LoginCommand : Command
{
    public string Password { get; set; }
    public string Email { get; set; }
    public override async Task<CommandExecutionResult> ExecuteAsync()
    {
            return await userRepository.Login(Email, Password);
    }
}
