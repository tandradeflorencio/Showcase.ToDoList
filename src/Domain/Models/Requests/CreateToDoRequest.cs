using System.ComponentModel.DataAnnotations;

namespace Showcase.ToDoList.Domain.Models.Requests
{
    public class CreateToDoRequest
    {
        [Length(1, 200, ErrorMessage = "Invalid Title")]
        public string? Title { get; set; }
    }
}