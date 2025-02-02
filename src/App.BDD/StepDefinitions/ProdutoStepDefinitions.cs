using Api.Controllers;
using App.Application.Interfaces;
using App.Application.ViewModels.Response;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace App.BDD.StepDefinitions
{
    [Binding]
    public class ProdutoStepDefinitions
    {

        public VideosController _controller;
        private readonly Mock<IVideosService> _appServive = new();
        private readonly List<Produto> _fakeProdutos = new List<Produto>();

        public ProdutoStepDefinitions()
        {
            _controller = new VideosController(_appServive.Object);

            for (int i = 1; i < 10; i++)
            {
                Produto produto = new Produto
                {
                    IdProduto = i,
                    NomeProduto = $"Produto {i}",
                    IdCategoria = 1,
                    ValorProduto = 10.00M,
                };
                _fakeProdutos.Add(produto);

        }
    }
    [Given(@"\[que existe um produto com ID (.*)]")]
    public void GivenQueExisteUmProdutoComID(int p0)
    {
        Assert.True(_fakeProdutos.Find(f => f.IdProduto == p0) != default);
    }

    [When(@"\[eu buscar o produto pelo IDCategoria (.*)]")]
    public void WhenEuBuscarOProdutoPeloIDCategoria(int p0)
    {
        // Arrange
        _appServive.Setup(service => service.GetProdutoByCategoria(p0))
            .ReturnsAsync(_fakeProdutos);
        // Act
        var result = _controller.GetProdutos(p0).GetAwaiter().GetResult();

        Assert.Equal(200, ((OkObjectResult)result).StatusCode);
    }

    [Then(@"o produto com ID (.*) deve ser retornado")]
    public void ThenOProdutoComIDDeveSerRetornado(int p0)
    {
        _appServive.Setup(service => service.GetProdutoByCategoria(p0))
      .ReturnsAsync(_fakeProdutos);
        var result = _controller.GetProdutos(p0).GetAwaiter().GetResult();

        // Assert
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        var produtos = okResult.Value as List<Produto>;
        Assert.NotNull(produtos);
        Assert.Contains(produtos, p => p.IdProduto == p0);
    }
}
}
