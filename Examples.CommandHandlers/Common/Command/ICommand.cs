using MediatR;

namespace Common.Command
{
    public interface ICommand<T> : IRequest<T>
    {
    }
}