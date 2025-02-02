using Api.Controllers;
using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Xunit;

namespace App.Test._1_WebAPI.Controllers
{
    public class ProdutosControllerTests
    {
        public VideosController _controller;
        private readonly Mock<IVideosService> _appServive = new();
        private readonly List<Produto> _fakeProdutos = new List<Produto>();

        public ProdutosControllerTests()
        {
            _controller = new VideosController(_appServive.Object);


            for (int i = 1; i < 10; i++)
            {
                _fakeProdutos.Add(new Produto(new ProdutoBD(1, "teste" + i,
                    i, true)));
            }
        }



        #region [GET]
        [Fact(DisplayName = "BuscarListaProdutos Ok")]
        public async Task GetProdutos_Returns_Ok()
        {
            // Arrange
            _appServive.Setup(service => service.GetProdutoByCategoria(1))
                .ReturnsAsync(_fakeProdutos);

            // Act
            var result = await _controller.GetProdutos(1);

            // Assert
            Assert.True(result is OkObjectResult);


        }
        [Fact(DisplayName = "BuscarListaProdutos Empty")]
        public async Task GetProdutos_Returns_Empty()
        {
            // Arrange
            _appServive.Setup(service => service.GetProdutoByCategoria(10))
                .ReturnsAsync(new List<Produto>());

            // Act
            var result = await _controller.GetProdutos(10);

            // Assert
            Assert.True(result is NoContentResult);


        }


        #endregion

        #region [POST]

        [Fact(DisplayName = "PostProdutos Ok")]
        public async Task PostProdutos_ReturnsOkResult()
        {
            // Arrange
            var item = new PostVideo
            {
                Ativo = true,
                IdCategoria = 1,
                NomeProduto = "Teste Post",
                ValorProduto = 1
            };
            _appServive.Setup(service => service.PostProduto(It.IsAny<PostVideo>()));

            // Act
            var result = await _controller.Post(item);
            // Assert
            Assert.True(result is StatusCodeResult);
        }




        #endregion

        #region [PATCH]



        [Fact(DisplayName = "PatchProdutos OK")]
        public async Task PatchProdutos_Returns_OK()
        {
            // Arrange
            var item = new PatchProduto
            {
                Ativo = true,
                IdCategoria = 1,
                NomeProduto = "Teste Post",
                ValorProduto = 1
            };
            _appServive.Setup(service => service.UpdateProdutoById(It.IsAny<int>(), It.IsAny<PatchProduto>()));

            // Act
            var result = await _controller.Patch(10, item);
            // Assert
            Assert.True(result is OkObjectResult);
        }


        #endregion



        #region [DELETE]

        [Fact(DisplayName = "DeleteProdutos OkResult")]
        public async Task DeleteProdutos_ReturnsOkResult()
        {
            // Arrange
            _appServive.Setup(service => service.DeleteProdutoById(It.IsAny<int>()));

            // Act
            var result = await _controller.DeleteProdutos(10);

            // Assert
            Assert.True(result is OkObjectResult);
        }


        [Fact(DisplayName = "DeleteProdutos Throws ValidationException When Produto Not Found")]
        public async Task DeleteProdutos_ThrowsValidationException_WhenProdutoNotFound()
        {
            // Arrange
            string expectedErrorMessage = "Produto Zero.";
            _appServive.Setup(service => service.DeleteProdutoById(It.IsAny<int>()))
                .Throws(new ValidationException(expectedErrorMessage));

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ValidationException>(() => _controller.DeleteProdutos(5));
            Assert.Equal(expectedErrorMessage, exception.Message);
        }


        #endregion
    }
}
