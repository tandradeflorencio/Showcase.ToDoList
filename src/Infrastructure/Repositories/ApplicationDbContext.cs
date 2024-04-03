using Microsoft.EntityFrameworkCore;
using Showcase.ToDoList.Infrastructure.Repositories.Interfaces;

namespace Showcase.ToDoList.Infrastructure.Repositories
{
    public sealed class ApplicationDbContext(DbContextOptions options) : DbContext(options), IUnitOfWork
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder) =>
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}