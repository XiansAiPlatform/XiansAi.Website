# Using Instructions

## Create a new Instruction

Go to the [XiansAI Portal](https://xians.ai) and click on the `Instructions` page. Click on the `Create New` button to create a new instruction.

![Create New Instruction](../images/create-instruction.png)

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

Modify `IComposerActivity` interface to decorate it with `[Instruction]` attribute.

`> PoetFlow.cs`

```csharp
using XiansAi.Flow;

[Instruction("How to Generate a Poem")]
public interface IComposerActivity
{
    Task<string> Compose(string keywords);
}
```

Modify `ComposerActivity.cs` we created in the [first example](./2-without-instructions.md) to use the new instruction.

`> ComposerActivity.cs`

```csharp
```