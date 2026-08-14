using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Shared.Abstraction
{
    public interface IRequestHandler<TRequest, TResponse> 
        where TRequest : IRequest<TResponse>
        where TResponse : IResult<TResponse>
    {
        Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken);
    }
}