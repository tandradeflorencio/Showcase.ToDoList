namespace Showcase.ToDoList.Domain.Models.Responses
{
    public class ToDoResponse : BaseResponse
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public bool Completed { get; set; }
    }
}