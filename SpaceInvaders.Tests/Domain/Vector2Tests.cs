﻿using SpaceInvaders.Game.Domain;
using Xunit;

namespace SpaceInvaders.Tests.Domain
{
    public class Vector2Tests
    {
        [Fact]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange and act
            Vector2 vector = new Vector2(3.5f, 4.5f);

            // Assert
            Assert.Equal(3.5f, vector.X);
            Assert.Equal(4.5f, vector.Y);
        }

        [Fact]
        public void Zero_ShouldReturnOrigin()
        {
            // Arrange and act
            Vector2 zeroVector = Vector2.Zero;
            // Assert
            Assert.Equal(0f, zeroVector.X);
            Assert.Equal(0f, zeroVector.Y);
        }

        [Theory]
        [InlineData(1, 2, 3, 4, 4, 6)]
        [InlineData(-1, -2, 1, 2, 0, 0)]
        [InlineData(0, 0, 0, 0, 0, 0)]
        public void Addition_ShouldReturnCorrectResult(
            float x1, float y1, float x2, float y2,
            float expectedX, float expectedY)
        {
            // Arrange
            var a = new Vector2(x1, y1);
            var b = new Vector2(x2, y2);

            // Act
            Vector2 result = a + b;

            // Assert
            Assert.Equal(expectedX, result.X);
            Assert.Equal(expectedY, result.Y);
        }

        [Fact]
        public void Equality_ShouldWorkCorrectly()
        {
            // Arrange
            var a = new Vector2(1, 2);
            var b = new Vector2(1, 2);
            var c = new Vector2(2, 3);

            // Assert
            Assert.Equal(a, b);
            Assert.NotEqual(a, c);
            Assert.True(a.Equals(b));
            Assert.False(a.Equals(c));
        }
    }
}
