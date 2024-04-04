using Microsoft.AspNetCore.Mvc;
using Showcase.ToDoList.Application.Services.Interfaces;
using Showcase.ToDoList.Domain.Models.Requests;
using Showcase.ToDoList.Domain.Models.Responses;

namespace Showcase.ToDoList.Presentation
{
    internal static class TodoEndpoints
    {
        public static void MapTodoEndpoints(this IEndpointRouteBuilder app)
        {
            MapPost(app);

            MapGet(app);

            MapPut(app);

            MapDelete(app);
        }

        private static void MapDelete(IEndpointRouteBuilder app)
        {
            app.MapDelete("api/todos/{id:guid}",
                        [ProducesResponseType(StatusCodes.Status204NoContent)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
            async ([FromRoute] Guid id, CancellationToken cancellationToken, IToDoService service) 
            =>
            {
                var result = await service.DeleteAsync(id, cancellationToken);

                if (result is null)
                    return Results.UnprocessableEntity();

                if (result.Code == StatusCodes.Status204NoContent)
                    return Results.NoContent();

                if (result.Code == StatusCodes.Status404NotFound)
                    return Results.NotFound(result);

                return Results.BadRequest(result);
            })
            .WithTags("ToDo")
            .WithDescription("Deletes a ToDo if it exists.")
            .WithOpenApi();
        }

        private static void MapPut(IEndpointRouteBuilder app)
        {
            app.MapPut("api/todos/{id:guid}",
                        [ProducesResponseType(typeof(CreateToDoResponse), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
            async ([FromRoute] Guid id, [FromBody] UpdateToDoRequest request, CancellationToken cancellationToken, IToDoService service)
            =>
            {
                var result = await service.UpdateAsync(id, request, cancellationToken);

                if (result is null)
                    return Results.UnprocessableEntity();

                if (result.Code == StatusCodes.Status200OK)
                    return Results.Ok(result);

                if (result.Code == StatusCodes.Status404NotFound)
                    return Results.NotFound(result);

                return Results.BadRequest(result.Message);
            })
            .WithTags("ToDo")
            .WithDescription("Updates a ToDo item if exists.")
            .WithOpenApi();
            }

        private static void MapPost(IEndpointRouteBuilder app)
        {
            app.MapPost("api/todos",
                        [ProducesResponseType(typeof(CreateToDoResponse), StatusCodes.Status201Created)]
            [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
            async ([FromBody] CreateToDoRequest request, CancellationToken cancellationToken, IToDoService service) 
            =>
            {
                var result = await service.CreateAsync(request, cancellationToken);

                if (result is null)
                    return Results.UnprocessableEntity();

                if (result.Code == StatusCodes.Status201Created)
                    return Results.CreatedAtRoute("GetToDoById", result);

                return Results.BadRequest(result.Message);
            })
            .WithTags("ToDo")
            .WithDescription("Creates a new ToDo item.")
            .WithOpenApi();
        }

        private static void MapGet(IEndpointRouteBuilder app)
        {
            app.MapGet("api/todos",
            [ProducesResponseType(typeof(ToDoResponse), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
            async ([AsParameters] ListToDoRequest listToDoRequest, CancellationToken cancellationToken, IToDoService service) 
            =>
            {
                var result = await service.ListAsync(listToDoRequest, cancellationToken);

                if (result is null)
                    return Results.UnprocessableEntity();

                return Results.Ok(result.ToDos);
            })
            .WithTags("ToDo")
            .WithDescription("List the ToDos.")
            .WithOpenApi();

            app.MapGet("api/todos/{id:guid}",
            [ProducesResponseType(typeof(ToDoResponse), StatusCodes.Status200OK)]
            [ProducesResponseType(StatusCodes.Status404NotFound)]
            [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
            async ([FromRoute] Guid id, CancellationToken cancellationToken, IToDoService service) 
            =>
            {
                var result = await service.GetAsync(id, cancellationToken);

                if (result is null)
                    return Results.UnprocessableEntity();

                if (result.Code == StatusCodes.Status200OK)
                    return Results.Ok(result);

                if (result.Code == StatusCodes.Status404NotFound)
                    return Results.NotFound(result);

                return Results.BadRequest(result);
            })
            .WithTags("ToDo")
            .WithDescription("Get a ToDo if exists.")
            .WithName("GetToDoById")
            .WithOpenApi();
        }
    }
}