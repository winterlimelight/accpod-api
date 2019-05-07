using System;
using System.Threading.Tasks;

namespace Business.CommandHandlers
{
    /// <summary>
    /// Interface used by any writing operations.
    /// </summary>
    /// <typeparam name="T">Command data-object being processed by this handler</typeparam>
    public interface ICommandHandler<T> where T : Commands.ICommand
    {
        // Optional Guid allows the ID of a newly created object to be returned
        Task<Guid?> Handle(T command);
    }
}
