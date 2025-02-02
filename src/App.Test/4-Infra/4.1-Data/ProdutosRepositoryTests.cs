using App.Domain.Interfaces;
using App.Domain.Models;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace App.Tests.Repositories
{
    public class ProdutosRepositoryTests
    {
        private readonly DbContextOptions<MySQLContext> _dbContextOptions;

        public ProdutosRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MySQLContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task PostProduto_ShouldAddProdutoToDatabase()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new ProdutosRepository(context);
            var produto = new ProdutoBD(1, "Produto Teste", 100.50m, true);

            // Act
            await repository.PostProduto(produto);

            // Assert
            var savedProduto = await context.Produtos.FirstOrDefaultAsync(p => p.Nome == "Produto Teste");
            Assert.NotNull(savedProduto);
            Assert.Equal("Produto Teste", savedProduto.Nome);
            Assert.Equal(100.50m, savedProduto.Preco);
        }

        [Fact]
        public async Task GetProdutosByIdCategoria_ShouldReturnFilteredProducts_WhenCategoriaExists()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new ProdutosRepository(context);

            context.Produtos.AddRange(
                new ProdutoBD(1, "Produto 1", 100.00m, true),
                new ProdutoBD(2, "Produto 2", 200.00m, true),
                new ProdutoBD(1, "Produto 3", 150.00m, true)
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetProdutosByIdCategoria(1);

            // Assert
            Assert.NotNull(result);

            Assert.All(result, p => Assert.Equal(1, p.CategoriaId));
        }

        [Fact]
        public async Task GetProdutosByIdCategoria_ShouldReturnAllProducts_WhenCategoriaIsNull()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new ProdutosRepository(context);

            context.Produtos.AddRange(
                new ProdutoBD(1, "Produto 1", 100.00m, true),
                new ProdutoBD(2, "Produto 2", 200.00m, true)
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetProdutosByIdCategoria(null);

            // Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task GetProdutoById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new ProdutosRepository(context);

            var produto = new ProdutoBD(1, "Produto Teste", 100.50m, true);
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetProdutoById(produto.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(produto.Nome, result.Nome);
        }

        [Fact]
        public async Task GetProdutoById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new ProdutosRepository(context);

            // Act
            var result = await repository.GetProdutoById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProduto_ShouldModifyProductInDatabase()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new ProdutosRepository(context);

            var produto = new ProdutoBD(1, "Produto Original", 100.00m, true);
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            produto.Nome = "Produto Atualizado";
            produto.Preco = 150.00m;

            // Act
            await repository.UpdateProduto(produto);

            // Assert
            var updatedProduto = await context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);
            Assert.NotNull(updatedProduto);
            Assert.Equal("Produto Atualizado", updatedProduto.Nome);
            Assert.Equal(150.00m, updatedProduto.Preco);
        }

        [Fact]
        public async Task DeleteProduto_ShouldRemoveProductFromDatabase()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new ProdutosRepository(context);

            var produto = new ProdutoBD(1, "Produto Teste", 100.50m, true);
            context.Produtos.Add(produto);
            await context.SaveChangesAsync();

            // Act
            await repository.DeleteProduto(produto);

            // Assert
            var deletedProduto = await context.Produtos.FirstOrDefaultAsync(p => p.Id == produto.Id);
            Assert.Null(deletedProduto);
        }
    }
}
