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

## Configuring the Flow Runner

You can find the settings required to run the Flow Runner on the XiansAi portal's `Settings` section.

![Settings](../images/portal-settings.png)

To configure the Flow Runner, you need to add the following code to your `Program.cs`. Add new file `Program.cs` to your project and add the following code:

`Program.cs >`

```csharp
using XiansAi.Flow;

var config = new Config {
    AppServerUrl = "", // Obtain value from the XiansAI portal
    AppServerCertPath = "", // Download the certificate file from the XiansAI portal. Set file path to the location of the file.
    AppServerCertPwd = "", // This is the password you give for the App Server certificate file generation.
    FlowServerUrl = "", // Obtain value from the XiansAI portal
    FlowServerNamespace = "", // Obtain value from the XiansAI portal
    FlowServerCertPath = "", // Download the certificate file from the XiansAI portal. Set file path to the location of the file.
    FlowServerPrivateKeyPath = "" // Download and save the private key file from the XiansAI portal
};
var flowRunner = new FlowRunnerService(config);

// Register the flow (see the next section for more details)
```

Use the values from the XiansAI portal to configure the `Config` object. Set the local path to the certificate files you downloaded from the XiansAI portal.

