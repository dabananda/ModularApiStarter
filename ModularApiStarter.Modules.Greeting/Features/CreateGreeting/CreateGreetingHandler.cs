using ModularApiStarter.Shared.Abstraction;
using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Modules.Greeting.Features.CreateGreeting
{
    public class CreateGreetingHandler(GreetingStore store)
        : IRequestHandler<CreateGreetingCommand, Result<CreateGreetingResponse>>
    {
        public Task<Result<CreateGreetingResponse>> Handle(CreateGreetingCommand request, CancellationToken cancellationToken)
        {
            var greeting = store.Add(new Entities.Greeting
            {
                Id = Guid.NewGuid(),
                Message = $"Hello, {request.Name}! Welcome to ModularApiStarter."
            });

            var response = new CreateGreetingResponse(greeting.Id, greeting.Message, greeting.CreatedAt);

            return Task.FromResult(Result<CreateGreetingResponse>.Success(response, "Greeting created"));
        }
    }
}
