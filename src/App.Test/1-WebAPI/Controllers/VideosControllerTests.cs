using Api.Controllers;
using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Application.ViewModels.Response;
using App.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace App.Test._1_WebAPI.Controllers
{
    public class VideosControllerTests
    {
        public VideosController _controller;
        private readonly Mock<IVideosService> _appServive = new();
        private readonly List<Video> _fakeVideos = new List<Video>();

        public VideosControllerTests()
        {
            _controller = new VideosController(_appServive.Object);


            for (int i = 1; i < 10; i++)
            {
                _fakeVideos.Add(new Video(new VideoBD("teste" + i)));
            }
        }



        #region [GET]
        [Fact(DisplayName = "BuscarListaVideos Ok")]
        public async Task GetVideos_Returns_Ok()
        {
            // Arrange
            _appServive.Setup(service => service.GetById(1))
                .ReturnsAsync(_fakeVideos.FirstOrDefault());

            // Act
            var result = await _controller.GetVideos(1);

            // Assert
            Assert.True(result is OkObjectResult);


        }     

        #endregion

        #region [POST]

        [Fact(DisplayName = "PostVideos Ok")]
        public async Task PostVideos_ReturnsOkResult()
        {
            // Arrange
            var item = new PostVideo
            {
                Base64="teste"
               
            };
            _appServive.Setup(service => service.Post(It.IsAny<PostVideo>()));

            // Act
            var result = await _controller.Post(item);
            
            // Assert
            Assert.NotNull(result);
        }




        #endregion

       
    }
}
