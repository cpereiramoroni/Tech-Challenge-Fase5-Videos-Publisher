using App.Application.ViewModels.Request;
using App.Domain.Interfaces;
using App.Domain.Models;
using Application.Services;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace App.Tests.Services
{
    public class ProdutosServiceTests
    {
        private readonly Mock<IProdutosRepository> _mockRepository;
        private readonly ProdutosService _service;

        public ProdutosServiceTests()
        {
            _mockRepository = new Mock<IProdutosRepository>();
            _service = new ProdutosService(_mockRepository.Object);
        }

        [Fact]
        public async Task PostProduto_ShouldCallRepository_WithValidInput()
        {
            // Arrange
            var input = new PostVideo
            {
                IdCategoria = 1,
                NomeProduto = "Produto Teste",
                ValorProduto = 100.50m,
                Ativo = true
            };

            // Act
            await _service.PostProduto(input);

            // Assert
            _mockRepository.Verify(r => r.PostProduto(It.Is<ProdutoBD>(p =>
                p.CategoriaId == input.IdCategoria &&
                p.Nome == input.NomeProduto &&
                p.Preco == input.ValorProduto &&
                p.Status == input.Ativo)), Times.Once);
        }

        [Fact]
        public async Task UpdateProdutoById_ShouldUpdateProduct_WhenProductExists()
        {
            // Arrange
            int produtoId = 1;
            var input = new PatchProduto
            {
                NomeProduto = "Novo Nome",
                ValorProduto = 150.75m,
                Ativo = false
            };
            var existingProduto = new ProdutoBD(1, "Produto Antigo", 100.50m, true);

            _mockRepository.Setup(r => r.GetProdutoById(produtoId)).ReturnsAsync(existingProduto);

            // Act
            await _service.UpdateProdutoById(produtoId, input);

            // Assert
            _mockRepository.Verify(r => r.UpdateProduto(It.Is<ProdutoBD>(p =>
                p.CategoriaId == existingProduto.CategoriaId &&
                p.Nome == input.NomeProduto &&
                p.Preco == input.ValorProduto &&
                p.Status == input.Ativo)), Times.Once);
        }

        [Fact]
        public async Task UpdateProdutoById_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            int produtoId = 1;
            var input = new PatchProduto { NomeProduto = "Novo Nome" };

            _mockRepository.Setup(r => r.GetProdutoById(produtoId)).ReturnsAsync((ProdutoBD)null);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.UpdateProdutoById(produtoId, input));
        }

        [Fact]
        public async Task DeleteProdutoById_ShouldCallRepository_WhenProductExists()
        {
            // Arrange
            int produtoId = 1;
            var existingProduto = new ProdutoBD(1, "Produto Teste", 100.50m, true);

            _mockRepository.Setup(r => r.GetProdutoById(produtoId)).ReturnsAsync(existingProduto);

            // Act
            await _service.DeleteProdutoById(produtoId);

            // Assert
            _mockRepository.Verify(r => r.DeleteProduto(existingProduto), Times.Once);
        }

        [Fact]
        public async Task DeleteProdutoById_ShouldThrowException_WhenProductDoesNotExist()
        {
            // Arrange
            int produtoId = 1;

            _mockRepository.Setup(r => r.GetProdutoById(produtoId)).ReturnsAsync((ProdutoBD)null);

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _service.DeleteProdutoById(produtoId));
        }

        [Fact]
        public async Task GetProdutoByCategoria_ShouldReturnProducts_WhenProductsExist()
        {
            // Arrange
            int? idCategoria = 1;
            var produtos = new List<ProdutoBD>
            {
                new ProdutoBD(1, "Produto 1", 100.00m, true),
                new ProdutoBD(1, "Produto 2", 200.00m, true)
            };

            _mockRepository.Setup(r => r.GetProdutosByIdCategoria(idCategoria)).ReturnsAsync(produtos);

            // Act
            var result = await _service.GetProdutoByCategoria(idCategoria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(produtos.Count, result.Count);
            Assert.Equal("Produto 1", result[0].NomeProduto);
            Assert.Equal("Produto 2", result[1].NomeProduto);
        }

        [Fact]
        public async Task GetProdutoByCategoria_ShouldReturnNull_WhenNoProductsExist()
        {
            // Arrange
            int? idCategoria = 1;

            _mockRepository.Setup(r => r.GetProdutosByIdCategoria(idCategoria)).ReturnsAsync((IList<ProdutoBD>)null);

            // Act
            var result = await _service.GetProdutoByCategoria(idCategoria);

            // Assert
            Assert.Null(result);
        }
    }
}
