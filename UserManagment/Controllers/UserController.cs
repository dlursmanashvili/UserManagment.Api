using Application.Commands.UserCommands;
using Application.Queries.UserQuery;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;
using static Application.Queries.UserQuery.LoginQuery;

namespace UserManagment.Controllers;

//[Authorize]
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
    [Authorize]
    [Route("GetAllUsers")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetAllUserQueryResult>> GetAllUsers([FromQuery] GetAllUserQuery query) =>
         await _queryExecutor.Execute<GetAllUserQuery, GetAllUserQueryResult>(query);
    
    [Authorize]
    [Route("GetOneUserByID")]
    [HttpGet]
    public async Task<QueryExecutionResult<GetOneUserByIDQueryResult>> GetOneUserByID([FromQuery] GetOneUserByIDQuery query) =>
     await _queryExecutor.Execute<GetOneUserByIDQuery, GetOneUserByIDQueryResult>(query);


    [Route("Login")]
    [HttpGet]
    public async Task<QueryExecutionResult<LoginQueryResult>> Login([FromQuery] LoginQuery query) =>
     await _queryExecutor.Execute<LoginQuery, LoginQueryResult>(query);


    #endregion

    #region Commands
    [Route("Registration")]
    [HttpPost]
    public async Task<CommandExecutionResult> Registration([FromBody] RegistrationCommand command) =>
         await _commandExecutor.Execute(command);

    [Authorize]
    [Route("DeleteUser")]
    [HttpDelete]
    public async Task<CommandExecutionResult> DeleteUser([FromBody] UserDeleteCommand command) =>
          await _commandExecutor.Execute(command);

    [Authorize]
    [Route("UpdateUser")]
    [HttpPost]
    public async Task<CommandExecutionResult> UpdateUser([FromBody] UserUpdateCommand command) =>
            await _commandExecutor.Execute(command);
    #endregion
}
