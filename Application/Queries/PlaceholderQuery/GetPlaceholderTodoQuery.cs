using Application.Shared;
using Shared;
using System.Text.Json;

namespace Application.Queries.PlaceholderQuery
{
    public class GetPlaceholderTodoQuery : Query<List<Todo>>
    {
        public int userId { get; set; }

        public override async Task<QueryExecutionResult<List<Todo>>> Execute()
        {
            List<Todo> todoList = new List<Todo>();
            if (_appContext.Users.FirstOrDefault(x => x.Id == userId).IsNull()) return await Fail("„ჩანაწერი ვერ მოძებნა“ ");
            try
            {
                string apiUrl = $"https://jsonplaceholder.typicode.com/todos?userId={userId}";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response into a list of Todo objects
                        todoList = JsonSerializer.Deserialize<List<Todo>>(content);
                        return await Ok(todoList);
                    }
                    else
                    {
                        return await Fail($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
            }
            catch (Exception ex)
            {
                    return await Fail(ex.Message);
            }
            // Return an empty result if there's an error
            //return await Ok(todoList);
        }
    }

    public class Todo
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public bool completed { get; set; }
    }
}
