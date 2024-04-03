using Microsoft.EntityFrameworkCore;
using Showcase.ToDoList.Domain.Models.Entities;
using Showcase.ToDoList.Infrastructure.Repositories.Interfaces;

namespace Showcase.ToDoList.Infrastructure.Repositories
{
    public class TodoRepository(ApplicationDbContext context) : ITodoRepository
    {
        async Task ITodoRepository.CreateAsync(ToDo toDo, CancellationToken cancellationToken) =>
            await context.Set<ToDo>().AddAsync(toDo, cancellationToken);

        public async Task DeleteAsync(ToDo toDo, CancellationToken cancellationToken) =>
            await context.Set<ToDo>()
                .Where(t => t.Id == toDo.Id)
                .ExecuteDeleteAsync(cancellationToken);

        public async Task UpdateAsync(ToDo toDo, CancellationToken cancellationToken) =>
            await context.Set<ToDo>()
                .Where(t => t.Id == toDo.Id)
                .ExecuteUpdateAsync(s =>
                s.SetProperty(t => t.Title, toDo.Title)
                .SetProperty(t => t.Completed, toDo.Completed), cancellationToken);

        public async Task<ToDo> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            var todo = await context.Set<ToDo>()
                .AsNoTracking()
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken: cancellationToken);

            return todo!;
        }

        public async Task<IList<ToDo>> ListAsync(int pageSize, CancellationToken cancellationToken)
        {
            var todos = await context.Set<ToDo>()
                .AsNoTracking()
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return todos;
        }
    }
}