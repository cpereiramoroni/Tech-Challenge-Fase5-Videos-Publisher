using App.Application.Interfaces;
using App.Application.ViewModels.Request;
using App.Domain.Interfaces;
using App.Domain.Models;
using Application.Services;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace App.Tests.Services
{
    public class VideosServiceTests
    {
        private readonly Mock<IVideosRepository> _mockRepository = new();
        private readonly Mock<IExternalService> _externalService=new();
        private readonly VideosService _service;

        public VideosServiceTests()
        {
            _mockRepository = new Mock<IVideosRepository>();
            _service = new VideosService(_mockRepository.Object, _externalService.Object);
        }

        [Fact]
        public async Task PostVideo_ShouldCallRepository_WithValidInput()
        {
            // Arrange
            var input = new PostVideo
            {
                Base64="dsfsd",
                Nome = "Video Teste",
            };

            _mockRepository.Setup(x => x.PostVideo(It.IsAny<VideoBD>())).Returns(Task.FromResult(1));
            _externalService.Setup(x => x.SaveFileS3(It.IsAny<PostVideo>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            _externalService.Setup(Task => Task.PublishedRabbit(It.IsAny<int>())).Returns(Task.CompletedTask);

            var result = await _service.Post(input);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_ShouldCallRepository_WithValidInput()
        {
            // Arrange
           
            _mockRepository.Setup(x => x.GetById(It.IsAny<int>())).ReturnsAsync(new VideoBD("Jose"));
            

            var result = await _service.GetById(1);

            // Assert
            Assert.True(result is not null);
        }



    }
}
