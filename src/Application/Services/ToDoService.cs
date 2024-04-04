using Mapster;
using Showcase.ToDoList.Application.Services.Interfaces;
using Showcase.ToDoList.Domain.Models.Entities;
using Showcase.ToDoList.Domain.Models.Requests;
using Showcase.ToDoList.Domain.Models.Responses;
using Showcase.ToDoList.Infrastructure.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Showcase.ToDoList.Application.Services
{
    public class ToDoService(Serilog.ILogger logger, ITodoRepository todoRepository, IUnitOfWork unitOfWork) : IToDoService
    {
        public async Task<BaseResponse> CreateAsync(CreateToDoRequest request, CancellationToken cancellationToken)
        {
            logger.Information($"{nameof(ToDoService)} {nameof(CreateAsync)} - Received a new request ({JsonSerializer.Serialize(request)})");

            var validationMessage = ValidateRequest(request);

            if (!string.IsNullOrWhiteSpace(validationMessage))
                return new BaseResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = validationMessage
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

        public async Task<ToDoListResponse> ListAsync(ListToDoRequest listToDoRequest, CancellationToken cancellationToken)
        {
            logger.Information($"{nameof(ToDoService)} {nameof(ListAsync)} - ({listToDoRequest?.PageSize}) Received a new request.");

            const int DefaultPageSize = 10;

            var pageSize = DefaultPageSize;

            if(listToDoRequest is { } &&  listToDoRequest.PageSize > 0)
                pageSize = listToDoRequest.PageSize.Value;

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

            var validationMessage = ValidateRequest(request);

            if (!string.IsNullOrWhiteSpace(validationMessage))
                return new BaseResponse
                {
                    Code = StatusCodes.Status400BadRequest,
                    Message = validationMessage
                };

            var todo = await todoRepository.GetAsync(id, cancellationToken);

            if (todo is null)
                return new BaseResponse
                {
                    Code = StatusCodes.Status404NotFound,
                    Message = $"ToDo with ID {id} was not found."
                };

            todo.Title = request.Title!;
            todo.Completed = request.Completed;

            await todoRepository.UpdateAsync(todo, cancellationToken);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            var response = todo.Adapt<ToDoResponse>();

            response.Code = StatusCodes.Status200OK;

            return response;
        }

        private static string? ValidateRequest<T>(T request)
        {            
            if (request is null)
                return "Invalid request.";

            var validationResults = new List<ValidationResult>();

            if (!Validator.TryValidateObject(request, new ValidationContext(request), validationResults, validateAllProperties: true))
                return validationResults[0]?.ErrorMessage;

            return string.Empty;
        }
    }
}