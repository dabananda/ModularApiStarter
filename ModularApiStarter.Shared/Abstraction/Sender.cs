using ModularApiStarter.Shared.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;
using System.Reflection;

namespace ModularApiStarter.Shared.Abstraction
{
    public class Sender(IServiceProvider provider) : ISender
    {
        private static readonly ConcurrentDictionary<Type, MethodInfo> DispatchMethodCache = new();
        private static readonly MethodInfo SendInternalDefinition = typeof(Sender).GetMethod(nameof(SendInternal), BindingFlags.NonPublic | BindingFlags.Instance)!;

        public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            where TResponse : IResult<TResponse>
        {
            ArgumentNullException.ThrowIfNull(request);

            var requestType = request.GetType();

            var method = DispatchMethodCache.GetOrAdd(requestType,
                rt => SendInternalDefinition.MakeGenericMethod(rt, typeof(TResponse)));

            return (Task<TResponse>)method.Invoke(this, [request, cancellationToken])!;
        }

        private async Task<TResponse> SendInternal<TRequest, TResponse>(TRequest request, CancellationToken cancellationToken)
            where TRequest : IRequest<TResponse>
            where TResponse : IResult<TResponse>
        {
            var handler = provider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();
            var behaviors = provider.GetServices<IPipelineBehavior<TRequest, TResponse>>().Reverse();

            RequestHandlerDelegate<TResponse> pipeline = () => handler.Handle(request, cancellationToken);

            foreach (var behavior in behaviors)
            {
                var next = pipeline;
                pipeline = () => behavior.Handle(request, next, cancellationToken);
            }

            return await pipeline();
        }
    }
}