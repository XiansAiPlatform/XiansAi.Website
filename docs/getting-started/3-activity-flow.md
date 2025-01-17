# Flow Activities

## What are Activities?

Activities are the building blocks of a flow which perform IO operations with the external world. They are the steps that are executed in a flow. Activities are defined as interfaces and implemented by classes.

The `Flow` class defines the flow and orchestrates the activities. It calls the activities that are executed in the flow. However, the `Flow` class is limited in it capabilities. It cannot perform any IO such as reading from a file or database. You can consider it as a `configuration` of the flow, just that we have a full power of a programming language at our disposal.

!!! info "Flow Constraints"
    Flow.ai uses the [Temporal](https://temporal.io/) services to run flows. Temporal is an workflow engine that is capable of running long-lived, robust and reliable flows. You can read more about Flow constraints [here](https://docs.temporal.io/workflows).

## A Flow with 2 Activities

We will create a flow that demonstrates activity usage by integrating with two external APIs to propose movie suggestions for a users.

## Workflow Class

`> MovieSuggestionFlow.cs`

```csharp
using Temporalio.Workflows;
using Temporalio.Activities;
using XiansAi.Flow;

[Workflow("Movie Suggestion Flow")]
public class MovieSuggestionFlow: FlowBase
{
    [WorkflowRun]
    public async Task<object[]> Run(string commaSeparatedUserIds)
    {
        // Initialize a list to store the results
        var result = new List<object>();

        // Split the comma separated user ids into an array of integers
        var userIds = commaSeparatedUserIds.Split(',').Select(int.Parse).ToArray();

        // Iterate over each user id
        foreach (var id in userIds)
        {
            // Step 1: Fetch user details from JSONPlaceholder
            var userName = await RunActivityAsync(
                    (IUserActivity a) => a.GetUserNameAsync(id));
        
            // Step 2: Get an activity suggestion from Bored API
            var movie = await RunActivityAsync(
                    (IMovieActivity a) => a.GetMovieAsync(userName));

            // Add the result to the list
            result.Add(new { User = userName, Movie = movie });
        }

        // Return the results as an array
        return result.ToArray();
    }
}

public interface IUserActivity
{
    [Activity]
    Task<string?> GetUserNameAsync(int id);
}


public interface IMovieActivity
{
    [Activity]
    Task<string?> GetMovieAsync(string? userName);
}
```

## Activity UserActivity

`> UserActivity.cs`

```csharp
using System.Text.Json;
using XiansAi.Activity;

public class UserActivity : BaseAgentStub, IUserActivity
{
    private readonly HttpClient _client = new HttpClient();

    private static string URL = "https://jsonplaceholder.typicode.com/users/{0}";

    public async Task<string?> GetUserNameAsync(int id)
    {
        var response = await _client.GetStringAsync(string.Format(URL, id));

        return JsonSerializer.Deserialize<JsonDocument>(response)?.RootElement.GetProperty("name").GetString();
    }
}
```

## Activity MovieActivity

`> MovieActivity.cs`

```csharp
using System.Text.Json;
using XiansAi.Activity;

public class MovieActivity : BaseAgentStub, IMovieActivity 
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
```

## Program

`> Program.cs`

```csharp
using XiansAi.Flow;
using DotNetEnv;
using Microsoft.Extensions.Logging;

// Env config via DotNetEnv
Env.Load(); // OR Manually set the environment variables

FlowRunnerService.SetLoggerFactory(LoggerFactory.Create(builder => 
    builder
        .SetMinimumLevel(LogLevel.Debug)
        .AddConsole()
));

// Cancellation token cancelled on ctrl+c
var tokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>{ tokenSource.Cancel(); eventArgs.Cancel = true;};

// Define the flow
var flowInfo = new FlowInfo<MovieSuggestionFlow>();
flowInfo.AddActivities<IUserActivity>(new UserActivity());
flowInfo.AddActivities<IMovieActivity>(new MovieActivity());

try
{
    var runner = new FlowRunnerService();
    // Run the flow by passing the flow info to the FlowRunnerService
    await runner.RunFlowAsync(flowInfo, tokenSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Application shutdown requested. Shutting down gracefully...");
}

```

Notice how the activities are defined as interfaces and implemented by classes.

## Run the Flow

```bash
dotnet build
dotnet run
```
