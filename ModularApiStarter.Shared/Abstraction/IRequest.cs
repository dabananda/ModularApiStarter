using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Shared.Abstraction
{
    public interface IRequest<TResponse> where TResponse : IResult<TResponse> { }
}