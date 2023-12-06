using Application.Shared;
using Shared;
using static Application.Queries.UserQuery.LoginQuery;

namespace Application.Queries.UserQuery;

public class LoginQuery : Query<LoginQueryResult>
{
    public string Password { get; set; }
    public string Email { get; set; }

    public override async Task<QueryExecutionResult<LoginQueryResult>> Execute()
    {
        if (!PasswordHelper.IsValidEmail(Email))
        {
            return await Fail("Invalid email address");
        }
        var user = _appContext.Users.FirstOrDefault(x => x.Email == Email);
        if (user.IsNull())
        {
            return await Fail("user not found");
        }

        if (PasswordHelper.VerifyPassword(Password, user.Password))
        {            
            return await Ok(new LoginQueryResult(){Token =TokenHelper.GenerateToken(user)});            
        }
        else
        {
            return await Fail("Password incorrect" );
        }

    }
    public class LoginQueryResult
    {
        public string Token { get; set; }
    }
}
