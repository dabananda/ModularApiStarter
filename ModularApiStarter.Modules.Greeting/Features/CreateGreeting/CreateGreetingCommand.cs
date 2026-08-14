using ModularApiStarter.Shared.Abstraction;
using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Modules.Greeting.Features.CreateGreeting
{
    public record CreateGreetingCommand(string Name) : ICommand<Result<CreateGreetingResponse>>;

    public record CreateGreetingResponse(Guid Id, string Message, DateTime CreatedAt);
}
