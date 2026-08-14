using ModularApiStarter.Shared.Common;

namespace ModularApiStarter.Shared.Abstraction
{
    public interface ICommand<TResponse> : IRequest<TResponse> where TResponse : IResult<TResponse> { }
}