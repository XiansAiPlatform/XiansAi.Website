# Flow Without Instructions

To try instructions, lets create a new flow that generates a poem using the Gemini API.

## 1. Create a new flow

Create a new PoetFlow.cs file and add the following code to it.

`> PoetFlow.cs`

```csharp
using Temporalio.Workflows;
using Temporalio.Activities;
using XiansAi.Flow;

[Workflow("Poet Flow")]
public class PoetFlow: FlowBase
{
    [WorkflowRun]
    public async Task<string?> Run(string keywords)
    {
        var geminiActivity = new ComposerActivity();
        var poem = await RunActivityAsync(
                    (IComposerActivity c) => c.GeneratePoemAsync(keywords));
        return poem;
    }
}

public interface IComposerActivity
{
    [Activity]
    Task<string?> GeneratePoemAsync(string keywords);
}

```

## 2. Add activity to the flow

Create a new activity that will be used to generate the poem.

`> ComposerActivity.cs`

```csharp
using System.Text.Json;
using XiansAi.Activity;
using System.Text;

public class ComposerActivity : BaseActivity, IComposerActivity 
{
    private static string URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={0}";

    public async Task<string?> GeneratePoemAsync(string keywords)
    {
        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
        var url = string.Format(URL, apiKey);

        var instruction = "Write a poem using the given keywords";

        var jsonPayload = CreateJsonPayload(instruction, keywords);
        var jsonContent = new StringContent(JsonSerializer.Serialize(jsonPayload), Encoding.UTF8, "application/json");
        var response = await new HttpClient().PostAsync(url, jsonContent);
        var responseContent = await response.Content.ReadAsStringAsync();
        return ExtractPoemFromResponse(responseContent);
    }

    private object CreateJsonPayload(string instruction, string keywords)
    {
        return new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new { text = instruction },
                        new { text = keywords }
                    }
                }
            }
        };
    }

    private string? ExtractPoemFromResponse(string responseContent)
    {
        var result = JsonSerializer.Deserialize<JsonDocument>(responseContent);

        // Navigate through the JSON structure to get the poem text
        var candidates = result?.RootElement.GetProperty("candidates");
        if (candidates.HasValue && candidates.Value.GetArrayLength() > 0)
        {
            var content = candidates.Value[0].GetProperty("content");
            var parts = content.GetProperty("parts");
            if (parts.GetArrayLength() > 0)
            {
                return parts[0].GetProperty("text").GetString();
            }
        }
        return null;
    }

    
}
```

## Gemini API Key

Obtain a Gemini API key from the [Gemini API Key](https://aistudio.google.com/apikey) page.

update your .env file with the Gemini API key.

```.env
GEMINI_API_KEY=<your-gemini-api-key>
```

## 3. Run the flow

Create a new program.cs file and add the following code to it.

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
var flowInfo = new FlowInfo<PoetFlow>();
flowInfo.AddActivities<IComposerActivity>(new ComposerActivity());

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

## 4. Start the FlowRunnerService

Run the program by executing the following command in your terminal.

```bash
dotnet run
```

## 5. Create New Flow on XIans AI Portal

- Navigate to the XiansAI portal
- Go to 'Flow Definitions' section
- Find your 'PoetFlow'
- Provide comma separated keywords for the poem e.g. "love, romance, moon, stars"
- Click 'Start New' to begin execution

## 6. View the Flow Execution

- Navigate to the XiansAI portal
- Go to 'Flow Runs' section
- Find your 'PoetFlow'
- Click on the execution to view the details
