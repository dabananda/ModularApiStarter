using ModularApiStarter.Shared.Abstraction;
using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Modules.Greeting.Features.GetGreetings
{
    public record GetGreetingsQuery : IQuery<Result<List<GreetingResponse>>>;

    public record GreetingResponse(Guid Id, string Message, DateTime CreatedAt);
}
