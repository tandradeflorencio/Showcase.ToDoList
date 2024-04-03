namespace Showcase.ToDoList.Domain.Models.Entities
{
    public class ToDo
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public bool Completed { get; set; }
    }
}