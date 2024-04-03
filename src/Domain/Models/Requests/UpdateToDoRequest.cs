namespace Showcase.ToDoList.Domain.Models.Requests
{
    public class UpdateToDoRequest
    {
        public string? Title { get; set; }

        public bool Completed { get; set; }
    }
}