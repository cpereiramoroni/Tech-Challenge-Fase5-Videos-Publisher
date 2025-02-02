using App.Domain.Models;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace App.Tests.Context
{
    public class MySQLContextTests
    {
        private readonly DbContextOptions<MySQLContext> _dbContextOptions;

        public MySQLContextTests()
        {
            // Configure In-Memory Database
            _dbContextOptions = new DbContextOptionsBuilder<MySQLContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public void MySQLContext_ShouldConfigureDbSet()
        {
            // Arrange & Act
            using var context = new MySQLContext(_dbContextOptions);

            // Assert
            Assert.NotNull(context.Produtos);
        }



        [Fact]
        public void MySQLContext_ShouldApplyModelMappings()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);

            // Act
            var model = context.Model.FindEntityType(typeof(ProdutoBD));

            // Assert
            Assert.NotNull(model); // Model should exist
            Assert.NotNull(model.FindProperty("Id")); // Ensure 'Id' is mapped
            Assert.NotNull(model.FindProperty("Nome")); // Ensure 'Nome' is mapped
        }
    }
}
