using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Shared.Abstraction
{
    public interface IQuery<TResponse> : IRequest<TResponse> where TResponse : IResult<TResponse> { }
}