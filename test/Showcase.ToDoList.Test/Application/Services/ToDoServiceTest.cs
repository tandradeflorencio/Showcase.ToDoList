using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using AutoFixture;
using Showcase.ToDoList.Application.Services;
using Showcase.ToDoList.Domain.Models.Entities;
using System.Diagnostics.CodeAnalysis;

namespace Showcase.ToDoList.Test.Application.Services
{
    [ExcludeFromCodeCoverage]
    public class ToDoServiceTest : BaseTest
    {
        [Fact]
        public async Task GetAsync_WhenToDoNotFound_ShouldReturnStatus404NotFound()
        {
            //Arrange
            var idNotFound = Guid.Empty;
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.GetAsync(idNotFound, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task GetAsync_WhenToDoFound_ShouldReturnStatus200Ok()
        {
            //Arrange
            var idFound = Guid.NewGuid();
            var toDoStored = _fixture.Build<ToDo>()
                .With(t => t.Id, idFound)
                .Create();
            var cancellationToken = new CancellationToken();
            _todoRepository.GetAsync(idFound, cancellationToken).Returns(toDoStored);

            var service = GetToDoService();

            //Act
            var result = await service.GetAsync(idFound, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task DeleteAsync_WhenToDoNotFound_ShouldReturnStatus404NotFound()
        {
            //Arrange
            var idNotFound = Guid.Empty;
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.DeleteAsync(idNotFound, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task DeleteAsync_WhenToDoFound_ShouldReturnStatus204NoContent()
        {
            //Arrange
            var idFound = Guid.NewGuid();
            var toDoStored = _fixture.Build<ToDo>()
                .With(t => t.Id, idFound)
                .Create();
            var cancellationToken = new CancellationToken();
            _todoRepository.GetAsync(idFound, cancellationToken).Returns(toDoStored);

            var service = GetToDoService();

            //Act
            var result = await service.DeleteAsync(idFound, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status204NoContent);
        }

        [Fact]
        public async Task ListAsync_WhenRecordsNotFound_ShouldReturnStatus200OK()
        {
            //Arrange
            var randomPageSize = new Random().Next();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.ListAsync(randomPageSize, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task ListAsync_WhenRecordsFound_ShouldReturnThoseRecords()
        {
            //Arrange
            var toDosStored = _fixture.Create<List<ToDo>>();
            var randomPageSize = new Random().Next();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();
            _todoRepository.ListAsync(randomPageSize, cancellationToken).Returns(toDosStored);

            //Act
            var result = await service.ListAsync(randomPageSize, cancellationToken);

            //Assert
            result.ToDos.Count.Should().Be(toDosStored.Count);
        }

        private ToDoService GetToDoService()
        {
            return new ToDoService(_logger, _todoRepository, _unitOfWork);
        }
    }
}
