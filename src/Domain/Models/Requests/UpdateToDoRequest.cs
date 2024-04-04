namespace Showcase.ToDoList.Domain.Models.Requests
{
    public class UpdateToDoRequest : CreateToDoRequest
    {
        public bool Completed { get; set; }
    }
}