# Setting Up a Flow Project

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)

## Creating a DotNet Project

Workflow Projects are Executable DotNet Projects that can be run locally and deployed to any server environment.

To create a new DotNet Console Project, you can use the following command:

```bash
dotnet new console -n MyNewXiansAiFlow
cd MyNewXiansAiFlow
```

## Adding the Xians.ai SDK

To add the Xians.ai SDK to your project, you can use the following command:

```bash
dotnet add package XiansAi.Lib
```

!!! note "Expert Tip"
    If you are using locally cloned XiansAi.Lib repository, add a Project Reference instead of the Nuget Package. E.g.,
    ```xml
    <ItemGroup>
        <ProjectReference Include="..\XiansAi.Lib\XiansAi.Lib.csproj" />
    </ItemGroup>
    ```

## Create a new flow

You will create a new flow and the necessary activities that will be used in the flow. These flows will run your local machine while development, and you may deploy to any server environment.

XiansAi.Lib is the library that connects your flow with the XiansAI platform. There are 2 parts in the XiansAi platform:

1. App Server: This is the server that allows you to manage your flows and instructions. It provides visibility into the flow execution and allows you to manage your flows and instructions.
2. Flow Server: This is the server that will run your flow. It is the server that will be deployed to the XiansAI platform.

Next we will download the necessary certificates to configure the Flow Runner.

## Download Necessary Certificates

To create a new flow, we need a create a `FlowRunnerService` instance and provide it with the 'flow' and 'activity' classes.

To configure the `FlowRunnerService` with XiansAi platform, add the following code to your `Program.cs`.

`Program.cs >`

```csharp
using XiansAi.Flow;

Environment.SetEnvironmentVariable("APP_SERVER_URL", "<your-app-server-url>");
Environment.SetEnvironmentVariable("APP_SERVER_CERT_PATH", "<your-app-server-cert-path>");
Environment.SetEnvironmentVariable("APP_SERVER_CERT_PWD", "<your-app-server-cert-pwd>");
Environment.SetEnvironmentVariable("FLOW_SERVER_URL", "<your-flow-server-url>");
Environment.SetEnvironmentVariable("FLOW_SERVER_NAMESPACE", "<your-flow-server-namespace>");
Environment.SetEnvironmentVariable("FLOW_SERVER_CERT_PATH", "<your-flow-server-cert-path>");
Environment.SetEnvironmentVariable("FLOW_SERVER_PRIVATE_KEY_PATH", "<your-flow-server-private-key-path>");

var flowRunner = new FlowRunnerService(config);

// Register the flow (see the next section for more details)
```

You can find the settings required to run the `FlowRunnerService` on the XiansAi portal's `Settings` section.

![Settings](../images/portal-settings.png)

Use the values from the XiansAI portal to configure the Environment variables. Set the local path to the certificate files you downloaded from the XiansAI portal.

!!! note "Expert Tip"
    You can use a package like [DotNetEnv](https://github.com/tonerdo/dotnet-env) to load the environment variables from the `.env` file without hardcoding them in your code.

    `.env file >`

    ``` .env
    # Platform environment variables
    FLOW_SERVER_URL=tenant-99x.ozqzb.tmprl.cloud:7233
    FLOW_SERVER_NAMESPACE=tenant-99x.ozqzb
    FLOW_SERVER_CERT_PATH=./.cert/FlowServerCert-1736928807581.crt
    FLOW_SERVER_PRIVATE_KEY_PATH=./.cert/FlowServerPrivateKey-1736928808385.key

    APP_SERVER_URL=https://api.xians.ai
    APP_SERVER_CERT_PATH=./.cert/AppServerCert-1736979268735.pfx
    APP_SERVER_CERT_PWD=test
    ```

    `Program.cs >`

    ```csharp
    using DotNetEnv;
    ...
    Env.Load();
    ```

## Validate the configuration

To validate the configuration, lets temporarily call `TestMe()` method from the `FlowRunnerService` instance.

```csharp
...
var flowRunner = new FlowRunnerService(config);
flowRunner.TestMe(); // temp method to validate the configuration
```

Now run the following command:

```bash
dotnet run
```

If you do not get errors, your configuration is correct. You can now remove the test call to `TestMe()` method in your code.

## Next Steps

Now that you have set up your environment, you can proceed to [create your first flow](2-first-flow.md).
