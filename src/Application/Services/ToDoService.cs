using Mapster;
using Showcase.ToDoList.Application.Services.Interfaces;
using Showcase.ToDoList.Domain.Models.Entities;
using Showcase.ToDoList.Domain.Models.Requests;
using Showcase.ToDoList.Domain.Models.Responses;
using Showcase.ToDoList.Infrastructure.Repositories.Interfaces;
using System.Text.Json;

namespace Showcase.ToDoList.Application.Services
{
    public class ToDoService(Serilog.ILogger logger, ITodoRepository todoRepository, IUnitOfWork unitOfWork) : IToDoService
    {
        public async Task<BaseResponse> CreateAsync(CreateToDoRequest request, CancellationToken cancellationToken)
        {
            logger.Information($"{nameof(ToDoService)} {nameof(CreateAsync)} - Received a new request ({JsonSerializer.Serialize(request)})");

            if (request is null || string.IsNullOrWhiteSpace(request.Title))
                return new BaseResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = $"Invalid {nameof(CreateToDoRequest.Title)}"
                };

            var todo = request.Adapt<ToDo>();

            await todoRepository.CreateAsync(todo, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = todo.Adapt<CreateToDoResponse>();

            response.Code = StatusCodes.Status201Created;

            return response;
        }

        public async Task<BaseResponse> DeleteAsync(Guid id, CancellationToken cancellationToken)
        {
            logger.Information($"{nameof(ToDoService)} {nameof(DeleteAsync)} - ({id}) Received a new request.");

            var todo = await todoRepository.GetAsync(id, cancellationToken);

            if (todo is null)
                return new BaseResponse
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = $"ToDo with ID {id} was not found."
                };

            await todoRepository.DeleteAsync(todo, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = todo.Adapt<ToDoResponse>();

            response.Code = StatusCodes.Status204NoContent;

            return response;
        }

        public async Task<BaseResponse> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            logger.Information($"{nameof(ToDoService)} {nameof(GetAsync)} - ({id}) Received a new request.");

            var todo = await todoRepository.GetAsync(id, cancellationToken);

            if (todo is null)
                return new BaseResponse
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = $"ToDo with ID {id} was not found."
                };

            var response = todo.Adapt<ToDoResponse>();

            response.Code = StatusCodes.Status200OK;

            return response;
        }

        public async Task<ToDoListResponse> ListAsync(int pageSize, CancellationToken cancellationToken)
        {
            logger.Information($"{nameof(ToDoService)} {nameof(ListAsync)} - ({pageSize}) Received a new request.");

            if (pageSize < 1)
                pageSize = int.MaxValue;

            var toDos = await todoRepository.ListAsync(pageSize, cancellationToken);

            var response = new ToDoListResponse
            {
                Code = StatusCodes.Status200OK
            };

            foreach (var toDo in toDos)
            {
                response.ToDos.Add(toDo.Adapt<ToDoResponse>());
            }

            return response;
        }

        public async Task<BaseResponse> UpdateAsync(Guid id, UpdateToDoRequest request, CancellationToken cancellationToken)
        {
            logger.Information($"{nameof(ToDoService)} {nameof(UpdateToDoRequest)} - ({id}) Received a new request ({JsonSerializer.Serialize(request)}).");

            if (request is null || string.IsNullOrWhiteSpace(request.Title))
                return new BaseResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = $"Invalid {nameof(UpdateToDoRequest.Title)}"
                };

            var todo = await todoRepository.GetAsync(id, cancellationToken);

            if (todo is null)
                return new BaseResponse
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = $"ToDo with ID {id} was not found."
                };

            todo.Title = request.Title;
            todo.Completed = request.Completed;

            await todoRepository.UpdateAsync(todo, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = todo.Adapt<ToDoResponse>();

            response.Code = StatusCodes.Status200OK;

            return response;
        }
    }
}