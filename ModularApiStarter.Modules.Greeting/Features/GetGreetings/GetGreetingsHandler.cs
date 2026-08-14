using ModularApiStarter.Shared.Abstraction;
using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Modules.Greeting.Features.GetGreetings
{
    public class GetGreetingsHandler(GreetingStore store)
        : IRequestHandler<GetGreetingsQuery, Result<List<GreetingResponse>>>
    {
        public Task<Result<List<GreetingResponse>>> Handle(GetGreetingsQuery request, CancellationToken cancellationToken)
        {
            var response = store.GetAll()
                .Select(g => new GreetingResponse(g.Id, g.Message, g.CreatedAt))
                .ToList();

            return Task.FromResult(Result<List<GreetingResponse>>.Success(response));
        }
    }
}
