using Application.Shared;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Queries.PlaceholderQuery;


public class GetPlaceholderAlbumsQuery : Query<List<Albums>>
{
    public int userId { get; set; }

    public override async Task<QueryExecutionResult<List<Albums>>> Execute()
    {
        List<Albums> todoList = new List<Albums>();
        if (_appContext.Users.FirstOrDefault(x => x.Id == userId).IsNull()) return await Fail("„ჩანაწერი ვერ მოძებნა“");
        try
        {
            string apiUrl = $"https://jsonplaceholder.typicode.com/albums?userId={userId}";

            using (HttpClient httpClient = new HttpClient())
            {
                HttpResponseMessage response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();

                    // Deserialize the JSON response into a list of Todo objects
                    todoList = JsonSerializer.Deserialize<List<Albums>>(content);
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

public class Albums
{
    public int id { get; set; }
    public int userId { get; set; }
    public string title { get; set; }
}
