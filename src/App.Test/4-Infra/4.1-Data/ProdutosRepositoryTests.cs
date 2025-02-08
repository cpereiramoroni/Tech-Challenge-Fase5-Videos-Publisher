using App.Domain.Interfaces;
using App.Domain.Models;
using App.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Xunit;

namespace App.Tests.Repositories
{
    public class VideosRepositoryTests
    {
        private readonly DbContextOptions<MySQLContext> _dbContextOptions;

        public VideosRepositoryTests()
        {
            _dbContextOptions = new DbContextOptionsBuilder<MySQLContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        [Fact]
        public async Task PostVideo_ShouldAddVideoToDatabase()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new VideosRepository(context);
            var Video = new VideoBD("Video Teste");

            // Act
            await repository.PostVideo(Video);

            // Assert
            var savedVideo = await context.Videos.FirstOrDefaultAsync(p => p.Nome == "Video Teste");
            Assert.NotNull(savedVideo);
            Assert.Equal("Video Teste", savedVideo.Nome);
            
        }

        [Fact]
        public async Task GetVideosByIdCategoria_ShouldReturnFilteredProducts_WhenCategoriaExists()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new VideosRepository(context);

            context.Videos.AddRange(
                new VideoBD("Video 1"),
                new VideoBD("Video 2"),
                new VideoBD("Video 3" )
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetById(1);

            // Assert
            Assert.NotNull(result);

            
        }


        [Fact]
        public async Task GetVideoById_ShouldReturnProduct_WhenProductExists()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new VideosRepository(context);

            var Video = new VideoBD("Video Teste");
            context.Videos.Add(Video);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetById(Video.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(Video.Nome, result.Nome);
        }

        [Fact]
        public async Task GetVideoById_ShouldReturnNull_WhenProductDoesNotExist()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new VideosRepository(context);

            // Act
            var result = await repository.GetById(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task AddVideo_ShouldModifyProductInDatabase()
        {
            // Arrange
            using var context = new MySQLContext(_dbContextOptions);
            var repository = new VideosRepository(context);

            var Video = new VideoBD("Video Original");
            

            // Act
            await repository.PostVideo(Video);

            // Assert
            var updatedVideo = await context.Videos.FirstOrDefaultAsync(p => p.Id == Video.Id);
            Assert.NotNull(updatedVideo);
            
        }

       
    }
}
