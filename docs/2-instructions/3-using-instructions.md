# Using Instructions

## Create a new Instruction

Go to the [XiansAI Portal](https://xians.ai) and click on the `Instructions` page. Click on the `Create New` button to create a new instruction.

![Create New Instruction](../images/poem-instruction.png)

Provide following information for the instruction and click on the `Create` button.

- Name: `How to Generate a Poem`
- Type: `Markdown`
- Instruction:

    ```
    # How to Generate a Poem

    ## Your Role
    You are a poet. You are given a set of keywords and you need to generate a poem. Make the poem engaging and interesting. It should be around 100 words long.

    ## Audience
    You are writing for children aged 10-12.

    ## Your Task
    Generate a poem using the given keywords. All the keywords should be used in the poem.

    ```

## Modify Workflow Classes

Modify `IComposerActivity.GeneratePoemAsync()` method to decorate it with `[Instruction]` attribute. The name of the instruction should be the same as the name of the instruction we created in the previous step.

`> PoetFlow.cs`

```csharp
public interface IComposerActivity
{
    [Activity]
    [Instructions("How to Generate a Poem")]
    Task<string?> GeneratePoemAsync(string keywords);
}
```

Now you can request for the instruction to be used in the method code. Modify `ComposerActivity.cs` we created in the [first example](./2-without-instructions.md) to use the new instruction.

`> ComposerActivity.cs`

```csharp

using System.Text.Json;
using XiansAi.Activity;
using System.Text;

public class ComposerActivity : ActivityBase, IComposerActivity 
{
    ...

    public async Task<string?> GeneratePoemAsync(string keywords)
    {
        ...

        //var instruction = "Write a poem using the given keywords";
        var instruction = await GetInstruction() ?? throw new Exception("Instruction not found");

        ...
    }

    ...
}
```

!!! note "Multiple Instructions"
    You can define multiple instructions on the Instruction Attribute.
    ```csharp
    [Instructions("How to Generate a Poem", "How to Generate a Story")]
    ```
    Then you can obtain the instructions by passing the index of the instruction to `GetInstruction()` method. The index is 1 for the first instruction and 2 for the second instruction and so on.
    ```csharp
    var instruction = await GetInstruction(1);
    ```
