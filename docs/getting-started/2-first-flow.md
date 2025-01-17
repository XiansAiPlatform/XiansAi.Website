# Your First Flow

## Creating a Flow

You can create a new flow by creating a new class that inherits from `XiansAi.Flow.FlowBase`.

!!! note "Tip"
    We are creating a simple flow. We will learn about more complex and meaningful flows that users `Agents` and `Instructions` in the next sections.

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
    The name workflow must be unique within your organization. You can see the list of workflow definitions on the XiansAI portal. You can change the name of the workflow by setting it at the `[Workflow]` attribute.

    ```csharp
    [Workflow("My New Named Flow")]
    public class SimpleFlow: FlowBase
    ```

!!! abstract "Did you know?"
    Did you know that Xians.ai is capable of long running (persistent) flows? This means you are able to delay a workflow for days or months as required. The workflow will be paused and resumed when the delay is over.

## Configuring flow visualization

We need to configure bundling the source code of the flow file into the assembly. Although this is not needed to execute flows, this is required for the flow visualization to work. Do this by adding the following xml to your `.csproj` file:

```xml
  <ItemGroup>
    <!-- Embed the flow source files -->
    <EmbeddedResource Include="SimpleFlow.cs">
        <LogicalName>%(Filename)%(Extension)</LogicalName>
    </EmbeddedResource>
  </ItemGroup>
```

Basically this tells the compiler to include the `SimpleFlow.cs` file in the assembly as an embedded resource. 

!!! note "Tip"
    If the file is in a different folder, you need to update the `Include` attribute to the full path of the file. Example: `Include="MyNamespace/SimpleFlow.cs"`

## Registering the Flow Runner

To register the flow, you need to add the new flow to Flow Runner on your `Program.cs` file. Update the `Program.cs` file with the following code:

`Program.cs >`

```csharp
using XiansAi.Flow;

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

To run the flow, you can run the following command:

```bash
dotnet build    
dotnet run
```

FlowRunner now waits to run the flows you are starting. You can start a new flow on the XiansAI portal by visiting the 'Flow Definitions' section and clicking on the 'Start New' button of your new flow Type (SimpleFlow).

!!! bug "Name not unique error"
    If you get the following error, you need to change the name of the flow. See above for how to change the name of the flow.

    ```bash
    > Bad Request: "Another user has already used this flow type name SimpleFlow. Please choose a different flow name."
    ```

![Start New Flow](../images/start-new-flow.png)

You can see the flow definition details and the flow visualization on the XiansAI portal.

![Flow Definition Details](../images/flow-visualization.png)

## Run the Flow

You can run the flow by clicking on the 'Start New' button on the flow definitions page. You should now see a new flow run on the 'Flow Runs' section of the XiansAI portal. Refresh if your run is not visible as it may require a few seconds to start.

![Flow Runs](../images/flow-runs.png)
