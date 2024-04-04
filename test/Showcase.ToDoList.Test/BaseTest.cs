using AutoFixture;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using Serilog;
using Showcase.ToDoList.Application.Services.Interfaces;
using Showcase.ToDoList.Infrastructure.Repositories.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace Showcase.ToDoList.Test
{
    [ExcludeFromCodeCoverage]
    public class BaseTest
    {
        protected readonly Fixture _fixture;

        protected readonly IConfiguration _configuration;

        protected readonly Serilog.ILogger _logger;

        protected readonly ITodoRepository _todoRepository;
        
        protected readonly IToDoService _toDoService;

        protected readonly IUnitOfWork _unitOfWork;        

        public BaseTest()
        {
            _fixture = new Fixture();
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            _todoRepository = Substitute.For<ITodoRepository>();
            _toDoService = Substitute.For<IToDoService>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _logger = Substitute.For<Serilog.ILogger>();
        }
    }
}
