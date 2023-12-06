using Application.Commands.UserProfileCommands;
using Application.Queries.UserProfileQuery;
using Application.Shared;
using Domain.Entities.UserProfileEntity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UserManagment.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserProfileController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public UserProfileController(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }

        #region Query
        [Authorize]
        [Route("GetAllUserProfile")]
        [HttpGet]
        public async Task<QueryExecutionResult<List<UserProfile>?>> GetAllUserProfile([FromQuery] GetAllUserProfileQuery query) =>
         await _queryExecutor.Execute<GetAllUserProfileQuery, List<UserProfile>?>(query);

        [Authorize]
        [Route("GetOneUserProfileByID")]
        [HttpGet]
        public async Task<QueryExecutionResult<UserProfile?>> GetOneUserProfileByID([FromQuery] GetOneUserProfileQuery query) =>
         await _queryExecutor.Execute<GetOneUserProfileQuery, UserProfile?>(query);
        #endregion

        #region commands
        [Authorize]
        [Route("CreateUserProfile")]
        [HttpPost]
        public async Task<CommandExecutionResult> CreateUserProfile([FromBody] CreateUserProfileCommand command) =>
              await _commandExecutor.Execute(command);

        [Authorize]
        [Route("UpdateUserProfile")]
        [HttpPost]
        public async Task<CommandExecutionResult> UpdateUserProfile([FromBody] UpdateUserProfileCommand command) =>
                await _commandExecutor.Execute(command);
        #endregion

    }
}
