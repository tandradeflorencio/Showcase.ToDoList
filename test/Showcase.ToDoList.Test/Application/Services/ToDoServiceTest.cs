using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Showcase.ToDoList.Application.Services;
using Showcase.ToDoList.Domain.Models.Entities;
using Showcase.ToDoList.Domain.Models.Requests;
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
            var positivePageSize = 0;
            var randomPageSize = new Random(positivePageSize).Next();
            var listToDoRequest = _fixture.Build<ListToDoRequest>()
                                    .With(r => r.PageSize, positivePageSize)
                                    .Create();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.ListAsync(listToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task ListAsync_WhenPageSizeLessThenZero_ShouldReturnStatus200OK()
        {
            //Arrange
            var lessThanZeroPageSize = -1;
            var listToDoRequest = _fixture.Build<ListToDoRequest>()
                                    .With(r => r.PageSize, lessThanZeroPageSize)
                                    .Create();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.ListAsync(listToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status200OK);
        }

        [Fact]
        public async Task ListAsync_WhenRecordsFound_ShouldReturnThoseRecords()
        {
            //Arrange
            var toDosStored = _fixture.Create<List<ToDo>>();
            var randomPageSize = new Random().Next();
            var listToDoRequest = _fixture.Build<ListToDoRequest>()
                                    .With(r => r.PageSize, randomPageSize)
                                    .Create();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();
            _todoRepository.ListAsync(randomPageSize, cancellationToken).Returns(toDosStored);

            //Act
            var result = await service.ListAsync(listToDoRequest, cancellationToken);

            //Assert
            result.ToDos.Count.Should().Be(toDosStored.Count);
        }

        [Fact]
        public async Task CreateAsync_WhenNullRequest_ShouldReturnStatus400BadRequest()
        {
            //Arrange
            CreateToDoRequest request = null!;
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.CreateAsync(request!, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateAsync_WhenTitleEmpty_ShouldReturnStatus400BadRequest()
        {
            //Arrange
            var createToDoRequest = _fixture.Build<CreateToDoRequest>()
                                        .With(c => c.Title, string.Empty)
                                        .Create();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.CreateAsync(createToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateAsync_WhenTitleOutOfRange_ShouldReturnStatus400BadRequest()
        {
            //Arrange
            var titleOutOfRange = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901";
            var createToDoRequest = _fixture.Build<CreateToDoRequest>()
                                        .With(c => c.Title, titleOutOfRange)
                                        .Create();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.CreateAsync(createToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task CreateAsync_WhenCreateSuccessful_ShouldReturnStatus201Created()
        {
            //Arrange
            var createToDoRequest = _fixture.Create<CreateToDoRequest>();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.CreateAsync(createToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status201Created);
        }

        [Fact]
        public async Task UpdateAsync_WhenTitleEmpty_ShouldReturnStatus400BadRequest()
        {
            //Arrange
            var storedId = Guid.NewGuid();
            var updateToDoRequest = _fixture.Build<UpdateToDoRequest>()
                                        .With(c => c.Title, string.Empty)
                                        .Create();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.UpdateAsync(storedId, updateToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WhenTitleOutOfRange_ShouldReturnStatus400BadRequest()
        {
            //Arrange
            var storedId = Guid.NewGuid();
            var titleOutOfRange = "123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901";
            var updateToDoRequest = _fixture.Build<UpdateToDoRequest>()
                                        .With(c => c.Title, titleOutOfRange)
                                        .Create();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.UpdateAsync(storedId, updateToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task UpdateAsync_WhenToDoNotFound_ShouldReturnStatus404NotFound()
        {
            //Arrange
            var storedId = Guid.NewGuid();
            var updateToDoRequest = _fixture.Create<UpdateToDoRequest>();
            var cancellationToken = new CancellationToken();
            var service = GetToDoService();

            //Act
            var result = await service.UpdateAsync(storedId, updateToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status404NotFound);
        }

        [Fact]
        public async Task UpdateAsync_WhenUpdateSuccessful_ShouldReturnStatus200OK()
        {
            //Arrange
            var storedId = Guid.NewGuid();
            var storedToDo = _fixture.Create<ToDo>();
            var updateToDoRequest = _fixture.Create<UpdateToDoRequest>();
            var cancellationToken = new CancellationToken();            
            var service = GetToDoService();

            _todoRepository.GetAsync(storedId, cancellationToken).Returns(storedToDo);

            //Act
            var result = await service.UpdateAsync(storedId, updateToDoRequest, cancellationToken);

            //Assert
            result.Code.Should().Be(StatusCodes.Status200OK);
        }

        private ToDoService GetToDoService()
        {
            return new ToDoService(_logger, _todoRepository, _unitOfWork);
        }
    }
}
