using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ModularApiStarter.Shared.Abstraction
{
    public interface IModule
    {
        IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration);
        IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints);
    }
}
