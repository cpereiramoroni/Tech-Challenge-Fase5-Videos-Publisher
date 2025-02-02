using App.Domain.Validations;
using System;
using Xunit;

namespace App.Tests.Validations
{
    public class AssertionConcernTests
    {
        [Theory(DisplayName = "AssertArgumentNotEmpty Throws ArgumentException for Empty or Null Strings")]
        [InlineData(null, "String cannot be null or empty.")]
        [InlineData("", "String cannot be null or empty.")]
        [InlineData("   ", "String cannot be null or empty.")]
        public void AssertArgumentNotEmpty_ThrowsArgumentException_WhenStringIsEmptyOrNull(string stringValue, string expectedMessage)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                AssertionConcern.AssertArgumentNotEmpty(stringValue, expectedMessage));

            Assert.Equal(expectedMessage, exception.Message);
        }

        [Fact(DisplayName = "AssertArgumentNotEmpty Does Not Throw for Non-Empty Strings")]
        public void AssertArgumentNotEmpty_DoesNotThrow_WhenStringIsNotEmpty()
        {
            // Arrange
            string stringValue = "Valid String";
            string message = "String cannot be null or empty.";

            // Act & Assert
            var exception = Record.Exception(() =>
                AssertionConcern.AssertArgumentNotEmpty(stringValue, message));

            Assert.Null(exception); // No exception should be thrown
        }

        [Fact(DisplayName = "AssertArgumentNotNull Throws ArgumentException for Null Object")]
        public void AssertArgumentNotNull_ThrowsArgumentException_WhenObjectIsNull()
        {
            // Arrange
            object object1 = null;
            string message = "Object cannot be null.";

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
                AssertionConcern.AssertArgumentNotNull(object1, message));

            Assert.Equal(message, exception.Message);
        }

        [Fact(DisplayName = "AssertArgumentNotNull Does Not Throw for Non-Null Object")]
        public void AssertArgumentNotNull_DoesNotThrow_WhenObjectIsNotNull()
        {
            // Arrange
            object object1 = new object();
            string message = "Object cannot be null.";

            // Act & Assert
            var exception = Record.Exception(() =>
                AssertionConcern.AssertArgumentNotNull(object1, message));

            Assert.Null(exception); // No exception should be thrown
        }



        [Fact(DisplayName = "AssertArgumentNotNullZero Does Not Throw for Non-Zero Decimal")]
        public void AssertArgumentNotNullZero_DoesNotThrow_WhenDecimalIsNotZero()
        {
            // Arrange
            decimal value = 100.50m;
            string message = "Decimal cannot be null or zero.";

            // Act & Assert
            var exception = Record.Exception(() =>
                AssertionConcern.AssertArgumentNotNullZero(value, message));

            Assert.Null(exception); // No exception should be thrown
        }
    }
}
