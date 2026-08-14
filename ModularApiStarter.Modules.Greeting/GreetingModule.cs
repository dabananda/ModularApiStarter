using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ModularApiStarter.Shared.Abstraction;
using ModularApiStarter.Shared.Extensions;

namespace ModularApiStarter.Modules.Greeting
{
    // Sample module — copy this folder's structure as the starting point for
    // your own modules (Entities / Features / Controllers / a *Module.cs file).
    public class GreetingModule : IModule
    {
        public IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(GreetingModule).Assembly;

            services.AddRequestHandlers(assembly);
            services.AddValidators(assembly);
            services.AddSingleton<GreetingStore>();

            // Controllers live in a separate assembly from the Api project, so they
            // need to be registered as an application part to be discovered by
            // app.MapControllers().
            services.AddControllers().AddApplicationPart(assembly);

            return services;
        }

        public IEndpointRouteBuilder MapEndpoints(IEndpointRouteBuilder endpoints)
        {
            // Nothing to do here for MVC controllers — they're mapped by the
            // shared app.MapControllers() call once registered as an application
            // part above. Use this method for minimal-API style endpoints instead.
            return endpoints;
        }
    }
}
