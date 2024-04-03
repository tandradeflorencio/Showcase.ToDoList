namespace Showcase.ToDoList.Domain.Models.Responses
{
    public class ToDoListResponse : BaseResponse
    {
        public IList<ToDoResponse> ToDos { get; set; } = [];
    }
}