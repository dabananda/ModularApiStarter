using FluentValidation;
using ModularApiStarter.Shared.Abstraction;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace ModularApiStarter.Shared.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRequestHandlers(this IServiceCollection services, Assembly assembly)
        {
            var handlerType = typeof(IRequestHandler<,>);

            var handlers = assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface)
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType));

            foreach (var handler in handlers)
            {
                foreach (var service in handler.GetInterfaces().Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerType))
                {
                    services.AddScoped(service, handler);
                }
            }

            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services, Assembly assembly)
        {
            services.AddValidatorsFromAssembly(assembly);
            return services;
        }

        public static IServiceCollection AddPipelineBehaviors(this IServiceCollection services)
        {
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Behaviors.ValidationBehavior<,>));
            return services;
        }
    }
}