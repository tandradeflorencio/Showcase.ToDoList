using Microsoft.Extensions.DependencyInjection;
using Showcase.ToDoList.Configurations;

namespace Showcase.ToDoList.Test.Presentation.Configurations
{
    public class DependencyInjectionConfigurationTest : BaseTest
    {
        [Fact]
        public void ConfigureDependencies_WhenExecute_ShouldReturnWithoutErrors()
        {
            //Arrange
            var service = new ServiceCollection();

            //Act
            DependencyInjectionConfiguration.ConfigureDependencies(service);

            //Assert
            Assert.True(true);
        }

        [Fact]
        public void ConfigureLogs_WhenExecute_ShouldReturnWithoutErrors()
        {
            //Arrange
            var service = new ServiceCollection();

            //Act
            DependencyInjectionConfiguration.ConfigureLogs(service, _configuration);

            //Assert
            Assert.True(true);
        }
    }
}
