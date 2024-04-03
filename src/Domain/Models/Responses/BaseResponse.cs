using System.Text.Json.Serialization;

namespace Showcase.ToDoList.Domain.Models.Responses
{
    public class BaseResponse
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Message { get; set; }

        [JsonIgnore]
        public int Code { get; set; }
    }
}