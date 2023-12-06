using Application.Shared;
using Domain.Entities.UserEntity;
using Shared;
using System.Text.Json;

namespace Application.Queries.PlaceholderQuery
{

    public class GetPlaceholderPhotosQuery  : Query<List<Photo>>
    {
        public int albumId { get; set; }

        public override async Task<QueryExecutionResult<List<Photo>>> Execute()
        {
            List<Photo> todoList = new List<Photo>();
            try
            {
                string apiUrl = $"https://jsonplaceholder.typicode.com/photos?albumId={albumId}";

                using (HttpClient httpClient = new HttpClient())
                {
                    HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();

                        // Deserialize the JSON response into a list of Todo objects
                        todoList = JsonSerializer.Deserialize<List<Photo>>(content);
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
        }
    }

    public class Photo
    {
        public int albumId { get; set; }
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }
        public string thumbnailUrl { get; set; }
    }
}
