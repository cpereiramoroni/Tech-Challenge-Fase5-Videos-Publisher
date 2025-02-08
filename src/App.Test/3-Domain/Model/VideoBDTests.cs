using App.Domain.Models;
using System;
using Xunit;

namespace App.Tests.Models
{
    public class VideoBDTests
    {
        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenValidArgumentsAreProvided()
        {
            // Arrange
            
            string nome = "Video Teste";
            
            // Act
            var Video = new VideoBD(nome);

            // Assert
            
            Assert.Equal(nome, Video.Nome);
            
        }

        [Theory]
        [InlineData(null, "O nome não pode estar vazio!")]
        [InlineData("", "O nome não pode estar vazio!")]
        public void ValidateEntity_ShouldThrowException_WhenNomeIsInvalid(string nome, string expectedMessage)
        {
            
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                new VideoBD(nome)
            );

            Assert.Equal(expectedMessage, exception.Message);
        }

      
    }
}
