using Application.Commands.UserCommands;
using Application.Queries.UserQuery;
using Application.Shared;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UserManagment.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ICommandExecutor _commandExecutor;
    private readonly IQueryExecutor _queryExecutor;

    public UserController(
        ICommandExecutor commandExecutor,
        IQueryExecutor queryExecutor)
    {
        _commandExecutor = commandExecutor;
        _queryExecutor = queryExecutor;
    }

    #region Queries
    [Route("GetAllUsers")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetAllUserQueryResult>> GetAllUsers([FromQuery] GetAllUserQuery query) =>
         await _queryExecutor.Execute<GetAllUserQuery, GetAllUserQueryResult>(query);

    [Route("GetOneUserByID")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetOneUserByIDQueryResult>> GetOneUserByID([FromQuery] GetOneUserByIDQuery query) =>
     await _queryExecutor.Execute<GetOneUserByIDQuery, GetOneUserByIDQueryResult>(query);
    #endregion

    #region Commands
    [Route("Registration")]
    [HttpPost]
    public async Task<CommandExecutionResult> Registration([FromBody] RegistrationCommand command) =>
         await _commandExecutor.Execute(command);

    [Route("DeleteUser")]
    [HttpDelete]
    public async Task<CommandExecutionResult> DeleteUser([FromBody] UserDeleteCommand command) =>
          await _commandExecutor.Execute(command);


    [Route("Login")]
    [HttpPost]
    public async Task<CommandExecutionResult> Login([FromBody] LoginCommand command) =>
          await _commandExecutor.Execute(command);

    [Route("UpdateUser")]
    [HttpPost]
    public async Task<CommandExecutionResult> UpdateUser([FromBody] UserUpdateCommand command) =>
            await _commandExecutor.Execute(command);
    #endregion
}
