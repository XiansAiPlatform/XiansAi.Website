# Xians.ai Agents

Agents are **reusable** software components that perform specific tasks within your business process flows. They serve as building blocks that can be composed together to create complex workflows.

## Activities vs Agents

Activities sit in between the Flow and the Agent. The activity is responsible of following:

- Initiating the agent by setting required parameters
- Calling the agent and monitoring its progress
- Collecting and formatting the agent's output to the Flow

## Common Agent Use Cases

Agents can handle various tasks including:

- Natural language processing using LLMs
- Communication via platforms like MS Teams or Slack  
- Code review automation
- Integration with external systems (e.g. HubSpot)
- Web scraping and data collection
- Email automation
- Web search operations

## Agent Implementations

Agents can be implemented using different approaches:

- Docker image
- As an External API (REST/gRPC)
- As a Software Package (NuGet)

## Agent Types

### 1. Package Agents

Package agents are distributed as NuGet packages and integrate directly into Xians.ai activities through a .NET SDK interface.

**Advantages:**

- Minimal setup required
- Efficient resource usage
- Direct integration with activity code
- Strong type safety

**Disadvantages:**

- Requires .NET compatibility
- Tighter coupling with Xians.ai activities

### 2. Docker Agents

Docker agents run in isolated containers, making them ideal for tasks requiring specific environments or dependencies. The Flow activity executes these agents via command line and waits for completion.

**Advantages:**

- Environment isolation
- No local software installation needed
- Support for non-.NET technologies
- Automatic cleanup with --rm flag

**Disadvantages:**

- Data transfer complexity (sometimes requires volume mounting)
- Additional Docker runtime overhead
- Dependency on Docker runtime

### 3. Remote Agents

Remote agents expose HTTP or gRPC endpoints that Xians.ai activities can communicate with. The platform provides basic connectivity validation but requires custom integration code.

**Advantages:**

- Language/platform agnostic
- Scalable deployment options
- Independent lifecycle management

**Disadvantages:**

- Requires custom integration code
- Network dependency
- More complex error handling

### 4. Custom Agents

Custom agents provide flexibility for unique integration needs that don't fit the standard agent types. They're typically used for:

- Legacy system integration
- Proprietary protocols
- Complex multi-system orchestration
- Special security requirements

**Note:** Custom agents require more development effort but offer maximum flexibility for specific use cases.
