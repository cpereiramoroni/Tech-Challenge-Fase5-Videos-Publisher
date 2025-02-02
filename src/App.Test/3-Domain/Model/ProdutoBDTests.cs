using App.Domain.Models;
using System;
using Xunit;

namespace App.Tests.Models
{
    public class ProdutoBDTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsAreProvided()
        {
            // Arrange
            int categoriaId = 1;
            string nome = "Produto Teste";
            decimal preco = 100.50m;
            bool status = true;

            // Act
            var produto = new ProdutoBD(categoriaId, nome, preco, status);

            // Assert
            Assert.Equal(categoriaId, produto.CategoriaId);
            Assert.Equal(nome, produto.Nome);
            Assert.Equal(preco, produto.Preco);
            Assert.Equal(status, produto.Status);
        }

        [Theory]
        [InlineData(null, "O nome não pode estar vazio!")]
        [InlineData("", "O nome não pode estar vazio!")]
        public void ValidateEntity_ShouldThrowException_WhenNomeIsInvalid(string nome, string expectedMessage)
        {
            // Arrange
            int categoriaId = 1;
            decimal preco = 100.50m;
            bool status = true;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new ProdutoBD(categoriaId, nome, preco, status)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData(0, "O Preco não pode estar vazio e tem que ser maior que zero!")]

        public void ValidateEntity_ShouldThrowException_WhenPrecoIsInvalid(decimal preco, string expectedMessage)
        {
            // Arrange
            int categoriaId = 1;
            string nome = "Produto Teste";
            bool status = true;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new ProdutoBD(categoriaId, nome, preco, status)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Theory]
        [InlineData(0, "A Categoria não pode estar vazio e tem que ser maior que zero!")]
        public void ValidateEntity_ShouldThrowException_WhenCategoriaIdIsInvalid(int categoriaId, string expectedMessage)
        {
            // Arrange
            string nome = "Produto Teste";
            decimal preco = 100.50m;
            bool status = true;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new ProdutoBD(categoriaId, nome, preco, status)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact]
        public void ValidateEntity_ShouldNotThrowException_WhenAllArgumentsAreValid()
        {
            // Arrange
            int categoriaId = 1;
            string nome = "Produto Teste";
            decimal preco = 100.50m;
            bool status = true;

            // Act & Assert
            var exception = Record.Exception(() => new ProdutoBD(categoriaId, nome, preco, status));
            Assert.Null(exception); // No exception should be thrown
        }

        [Fact]
        public void ValidateEntity_ShouldValidateExistingInstance_WhenMethodIsCalledDirectly()
        {
            // Arrange
            var produto = new ProdutoBD(1, "Produto Teste", 100.50m, true);
            produto.Nome = "";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => produto.ValidateEntity());
            Assert.Equal("O nome não pode estar vazio!", exception.Message);
        }
    }
}
