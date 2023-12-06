using Application.Queries.PlaceholderQuery;
using Application.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared;

namespace UserManagment.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlaceholderContoller : ControllerBase
    {

        private readonly ICommandExecutor _commandExecutor;
        private readonly IQueryExecutor _queryExecutor;

        public PlaceholderContoller(
            ICommandExecutor commandExecutor,
            IQueryExecutor queryExecutor)
        {
            _commandExecutor = commandExecutor;
            _queryExecutor = queryExecutor;
        }


        [Authorize]
        [Route("GetTodo")]
        [HttpGet]
        public async Task<QueryExecutionResult<List<Todo>>> GetTodo([FromQuery] GetPlaceholderTodoQuery query) =>
        await _queryExecutor.Execute<GetPlaceholderTodoQuery, List<Todo>>(query);

        [Authorize]
        [Route("GetAlbums")]
        [HttpGet]
        public async Task<QueryExecutionResult<List<Albums>>> GetAlbums([FromQuery] GetPlaceholderAlbumsQuery query) =>
        await _queryExecutor.Execute<GetPlaceholderAlbumsQuery, List<Albums>>(query);

        [Authorize]
        [Route("GetPhotos")]
        [HttpGet]
        public async Task<QueryExecutionResult<List<Photo>>> GetPhotos([FromQuery] GetPlaceholderPhotosQuery query) =>
        await _queryExecutor.Execute<GetPlaceholderPhotosQuery, List<Photo>>(query);


        [Authorize]
        [Route("GetPosts")]
        [HttpGet]
        public async Task<QueryExecutionResult<List<Post>>> GetPosts([FromQuery] GetPlaceholderPostsQuery query) =>
        await _queryExecutor.Execute<GetPlaceholderPostsQuery, List<Post>>(query);
    }
}
