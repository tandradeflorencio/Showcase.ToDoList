using Showcase.ToDoList.Domain.Models.Entities;

namespace Showcase.ToDoList.Infrastructure.Repositories.Interfaces
{
    public interface ITodoRepository
    {
        Task CreateAsync(ToDo toDo, CancellationToken cancellationToken);

        Task<ToDo> GetAsync(Guid id, CancellationToken cancellationToken);

        Task<IList<ToDo>> ListAsync(int pageSize, CancellationToken cancellationToken);

        Task UpdateAsync(ToDo toDo, CancellationToken cancellationToken);

        Task DeleteAsync(ToDo toDo, CancellationToken cancellationToken);
    }
}