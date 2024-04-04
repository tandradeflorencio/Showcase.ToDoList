namespace Showcase.ToDoList.Domain.Models.Entities
{
    public class ToDo
    {
        public Guid Id { get; set; }

        public required string Title { get; set; }

        public bool Completed { get; set; }
    }
}