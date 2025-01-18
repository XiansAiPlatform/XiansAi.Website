using Temporalio.Activities;
using Temporalio.Workflows;
using XiansAi.Activity;
using XiansAi.Flow;

[Workflow("LinkedIn Reader")]
public class LinkedInReaderFlow: FlowBase
{
    [WorkflowRun]
    public async Task<string?> Run(string companyName)
    {
        var linkedInUrl = await RunActivityAsync((IUrlSearchActivity activity) => activity.FindLinkedInUrl(companyName));
        return linkedInUrl;
    }
}

[Agent("XiansAi.Agent.GoogleSearch", AgentType.Package)]
public interface IUrlSearchActivity
{
    [Activity]
    Task<string?> FindLinkedInUrl(string companyName);
}