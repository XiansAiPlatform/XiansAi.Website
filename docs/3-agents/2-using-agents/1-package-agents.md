# Using Package Agents

Package agents are distributed as NuGet packages and integrate directly into Xians.ai activities through a .NET SDK interface. Lets look at an example package agent usage.

We will use `XiansAi.Agent.GoogleSearch` as an example.

## Step 1: Install the NuGet Package

To use the `XiansAi.Agent.GoogleSearch` package, you need to install it into your project. You can do this by running the following command in your 'Flow' project where your activity implementation is defined.

```bash
dotnet add package XiansAi.Agent.GoogleSearch
```

## Step 2: Add the Agent to the Activity

