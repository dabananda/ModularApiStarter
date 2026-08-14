using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Shared.Abstraction
{
    public interface ISender
    {
        Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            where TResponse : IResult<TResponse>;
    }
}