using Showcase.ToDoList.Domain.Models.Requests;
using Showcase.ToDoList.Domain.Models.Responses;

namespace Showcase.ToDoList.Application.Services.Interfaces
{
    public interface IToDoService
    {
        Task<BaseResponse> CreateAsync(CreateToDoRequest request, CancellationToken cancellationToken);

        Task<BaseResponse> GetAsync(Guid id, CancellationToken cancellationToken);

        Task<ToDoListResponse> ListAsync(int pageSize, CancellationToken cancellationToken);

        Task<BaseResponse> UpdateAsync(Guid id, UpdateToDoRequest request, CancellationToken cancellationToken);

        Task<BaseResponse> DeleteAsync(Guid id, CancellationToken cancellationToken);
    }
}