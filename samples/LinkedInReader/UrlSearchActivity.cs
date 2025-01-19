using XiansAi.Activity;
using XiansAi.Agent.GoogleSearch;

public class UrlSearchActivity: ActivityBase, IUrlSearchActivity
{
    private readonly SearchEngine _searchEngine;

    public UrlSearchActivity()
    {
        var apiKey = Environment.GetEnvironmentVariable("VALUESERP_API_KEY") 
            ?? throw new Exception("VALUESERP_API_KEY is not set");

        _searchEngine = new SearchEngine(apiKey);
    }

    public async Task<string?> FindLinkedInUrl(string companyName)
    {
        var query = $"site:linkedin.com {companyName} AS Norway";
        var results = await _searchEngine.SearchAsync(query, 1);
        return results.Items.FirstOrDefault()?.Link;
    }
}