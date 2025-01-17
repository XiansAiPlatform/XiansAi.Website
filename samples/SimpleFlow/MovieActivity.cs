using System.Text.Json;
using XiansAi.Activity;
public class MovieActivity : BaseActivity, IMovieActivity 
{
    private readonly HttpClient _client = new HttpClient();

    private static string URL = "https://freetestapi.com/api/v1/movies/{0}";

    public async Task<string?> GetMovieAsync(string? userName)
    {
        var randonInt = Random.Shared.Next(1, 10);
        var response = await _client.GetStringAsync(string.Format(URL, randonInt));
        var result = JsonSerializer.Deserialize<JsonDocument>(response);
        return result?.RootElement.GetProperty("title").GetString();
    }
}