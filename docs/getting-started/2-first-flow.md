# Your First Flow

## Creating a Flow

To create a new flow, create a class that inherits from `XiansAi.Flow.FlowBase`.

!!! note "Tip"
    This example demonstrates a simple flow. In later sections, we'll explore more complex flows using `Agents` and `Instructions`.

`SimpleFlow.cs >`

```csharp
using Temporalio.Workflows;
using XiansAi.Flow;

[Workflow]
public class SimpleFlow: FlowBase
{
    [WorkflowRun]
    public async Task<string> Run(string name)
    {
        var output = "Hello";
        await DelayAsync(TimeSpan.FromSeconds(10));
        output += " World " + name;
        return await Task.FromResult(output + $" {name}!");
    }
}
```

!!! warning "Important"
    Each workflow name must be unique within your organization. You can view existing workflow definitions in the XiansAI portal. To customize a workflow's name, use the [Workflow] attribute:
    ```csharp
    [Workflow("My New Named Flow")]
    public class SimpleFlow: FlowBase
    ```

!!! abstract "Did you know?"
    Xians.ai supports long-running (persistent) flows. This means your workflow can be paused for days or months and will automatically resume when the delay period ends.

## Enabling flow visualization

To enable flow visualization, you need to bundle the flow's source code into the assembly. Add the following XML to your `.csproj` file:

```xml
  <ItemGroup>
    <!-- Embed the flow source files -->
    <EmbeddedResource Include="SimpleFlow.cs">
        <LogicalName>%(Filename)%(Extension)</LogicalName>
    </EmbeddedResource>
  </ItemGroup>
```

This configuration embeds the SimpleFlow.cs file as a resource in your assembly.

!!! note "Tip"
    If your flow file is in a subdirectory, specify the full path in the Include attribute. For example: Include="MyNamespace/SimpleFlow.cs"

## Registering the Flow Runner

To register your flow, add it to the Flow Runner in your Program.cs file:

`Program.cs >`

```csharp
using XiansAi.Flow;
using DotNetEnv;

// Env config via DotNetEnv
Env.Load(); // OR Manually set the environment variables

// Define the flow
var flowInfo = new FlowInfo<SimpleFlow>();

// Cancellation token cancelled on ctrl+c
var tokenSource = new CancellationTokenSource();
Console.CancelKeyPress += (_, eventArgs) =>{ tokenSource.Cancel(); eventArgs.Cancel = true;};

try
{
    // Run the flow by passing the flow info to the FlowRunnerService
    await new FlowRunnerService().RunFlowAsync(flowInfo, tokenSource.Token);
}
catch (OperationCanceledException)
{
    Console.WriteLine("Application shutdown requested. Shutting down gracefully...");
}

```

To start the flow runner:

```bash
dotnet build    
dotnet run
```

The Flow Runner will now wait for flow execution requests. To start a new flow, visit the 'Flow Definitions' section in the XiansAI portal and click the 'Start New' button for your SimpleFlow.

!!! bug "Duplicate Name Error"
    If you receive this error:
    ```bash
    > Bad Request: "Another user has already used this flow type name SimpleFlow. Please choose a different flow name."
    ```
    You'll need to choose a unique name using the [Workflow] attribute as shown earlier.

![Start New Flow](../images/start-new-flow.png)

You can view flow definition details and visualizations in the XiansAI portal.

![Flow Definition Details](../images/flow-visualization.png)

## Running the Flow

To execute your flow:

1. Navigate to the flow definitions page
2. Click the 'Start New' button
3. Monitor the 'Flow Runs' section to track your flow's execution

Note: It may take a few seconds for your flow run to appear. Refresh the page if needed.

![Flow Runs](../images/flow-runs.png)

## Next Steps

Now that you've created your first flow, learn how to create a Flow with Activities to explore more advanced flow capabilities.

[Create a Flow with Activities](3-activity-flow.md)
