using Microsoft.AspNetCore.Mvc;
using ModularApiStarter.Modules.Greeting.Features.CreateGreeting;
using ModularApiStarter.Modules.Greeting.Features.GetGreetings;
using ModularApiStarter.Shared.Abstraction;
using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Modules.Greeting.Controllers
{
    // Route resolves to api/v1/greetings via [Route("api/v1/[controller]")] on BaseController.
    public class GreetingsController(ISender sender) : BaseController
    {
        [HttpPost]
        public async Task<IActionResult> Create(CreateGreetingCommand command, CancellationToken cancellationToken)
        {
            var result = await sender.Send(command, cancellationToken);
            return Handle(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await sender.Send(new GetGreetingsQuery(), cancellationToken);
            return Handle(result);
        }
    }
}
