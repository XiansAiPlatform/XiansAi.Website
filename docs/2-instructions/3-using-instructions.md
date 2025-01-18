# Using Instructions

## Creating Instructions

To create a new instruction:

1. Navigate to the [XiansAI Portal](https://xians.ai)
2. Go to the `Instructions` page
3. Click `Create New`

![Create New Instruction](../images/poem-instruction.png)

Fill in the following fields:

- **Name**: A unique identifier (e.g. `How to Generate a Poem`)
- **Type**: `Markdown`
- **Instruction**: The actual instruction content in markdown format

Example instruction content:

```markdown
# How to Generate a Poem

## Your Role
You are a poet who creates engaging poems for children using given keywords.

## Audience
Children aged 10-12 years old.

## Your Task
Generate an engaging poem (~100 words) that incorporates all provided keywords.
```

## Using Instructions in Code

### 1. Decorate Activity Methods

Add the `[Instructions]` attribute to methods that should use instructions:

`> PoetFlow.cs`

```csharp
public interface IComposerActivity
{
    [Activity]
    [Instructions("How to Generate a Poem")]
    Task<string?> GeneratePoemAsync(string keywords);
}
```

### 2. Load Instructions in Activity Implementation

Use the `GetInstruction()` helper method to load instructions at runtime:

`> ComposerActivity.cs`

```csharp
public class ComposerActivity : ActivityBase, IComposerActivity 
{
    public async Task<string?> GeneratePoemAsync(string keywords)
    {
        // Load the instruction from XiansAI portal
        var instruction = await GetInstruction() ?? throw new Exception("Instruction not found");
        
        // Use the instruction...
    }
}
```

## Working with Multiple Instructions

You can specify multiple instructions for a single method:

```csharp
[Instructions("How to Generate a Poem", "How to Generate a Story")]
```

Load specific instructions by index (1-based):

```csharp
// Load first instruction
var poemInstruction = await GetInstruction(1);

// Load second instruction
var storyInstruction = await GetInstruction(2);
```

!!! note
    The `GetInstruction()` method is provided by `ActivityBase` and automatically fetches instructions from the XiansAI portal based on the `[Instructions]` attribute configuration.


## Next Steps

Now we are able to use instructions in our code. Next, we will learn how to use `Agents`' to create more complex workflows.

[Next: Using Agents](../3-agents/1-agent-types.md)
