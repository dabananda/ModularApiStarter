using ModularApiStarter.Shared.Abstraction;
using ModularApiStarter.Shared.Common;
using ModularApiStarter.Shared.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace ModularApiStarter.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedDI(this IServiceCollection services)
        {
            services.AddScoped<ISender, Sender>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();
            services.AddPipelineBehaviors();

            return services;
        }
    }
}