using Application.Shared;
using Shared;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Application.Queries.PlaceholderQuery;

public class GetPlaceholderPostsQuery : Query<List<Post>>
{
    public int userId { get; set; }
    public override async Task<QueryExecutionResult<List<Post>>> Execute()
    {
        List<Post> posts = new();
        List<Comment>? comments = new();
        if (_appContext.Users.FirstOrDefault(x => x.Id == userId).IsNull()) return await Fail("„ჩანაწერი ვერ მოძებნა“");

        try
        {
            string apiUrl = $"https://jsonplaceholder.typicode.com/posts?userId={userId}";

            using (HttpClient httpClient = new())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into a list of Todo objects
                    posts = JsonSerializer.Deserialize<List<Post>>(content);

                }
                else
                {
                    return await Fail($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                }
            }
            if (posts.IsNotNull())
            {
                foreach (var postitem in posts)
                {
                    var postId = postitem.id;
                    apiUrl = $"https://jsonplaceholder.typicode.com/comments?postId={postId}";

                    using (HttpClient httpClient = new())
                    {
                        HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                        if (response.IsSuccessStatusCode)
                        {
                            string content = await response.Content.ReadAsStringAsync();
                            // Deserialize the JSON response into a list of Todo objects
                            comments = JsonSerializer.Deserialize<List<Comment>>(content);
                            postitem.comments = comments;
                        }
                        else
                        {
                            return await Fail($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                        }
                    }
                }
            }


            return await Ok(posts);
        }
        catch (Exception ex)
        {
            return await Fail(ex.Message);
        }

        throw new NotImplementedException();
    }
}

public class Post
{
    [JsonPropertyName("id")]
    public int id { get; set; }

    [JsonPropertyName("userId")]
    public int userId { get; set; }

    [JsonPropertyName("body")]
    public string body { get; set; }

    [JsonPropertyName("title")]
    public string title { get; set; }

    [JsonPropertyName("comments")]
    public List<Comment>? comments { get; set; }
}

public class Comment
{
    public int id { get; set; }
    public int postId { get; set; }
    public string name { get; set; }
    public string email { get; set; }
    public string body { get; set; }
}
