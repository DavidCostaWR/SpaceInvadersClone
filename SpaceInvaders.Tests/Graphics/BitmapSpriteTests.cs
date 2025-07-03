using SpaceInvaders.Game.Graphics;
using Xunit;

namespace SpaceInvaders.Tests.Graphics
{
    public class BitmapSpriteTests
    {
        [Fact]
        public void Constructor_ShouldSetCorrectDimensions()
        {
            // Arrange
            var pattern = new[]
            {
                "  XX  ",
                " XXXX ",
                "XXXXXX",
                "XX  XX"
            };

            // Act
            var sprite = new BitmapSprite(pattern);

            // Assert
            Assert.Equal(6, sprite.Width);
            Assert.Equal(4, sprite.Height);
        }

        [Fact]
        public void Constructor_ShouldThrowOnInconsistentWidth()
        {
            // Arrange
            var pattern = new[]
            {
                "  XX  ",
                " XXXX",  // Different width
                "XXXXXX"
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => new BitmapSprite(pattern));
        }

        [Fact]
        public void SmallInvaderSprite_ShouldHaveCorrectSize()
        {
            // Arrange & Act
            var sprite = new BitmapSprite(SpriteData.SmallInvaderFrame1);

            // Assert
            Assert.Equal(8, sprite.Width);
            Assert.Equal(8, sprite.Height);
        }

        [Fact]
        public void MediumInvaderSprite_ShouldHaveCorrectSize()
        {
            // Arrange & Act
            var sprite = new BitmapSprite(SpriteData.MediumInvaderFrame1);

            // Assert
            Assert.Equal(11, sprite.Width);
            Assert.Equal(8, sprite.Height);
        }

        [Fact]
        public void LargeInvaderSprite_ShouldHaveCorrectSize()
        {
            // Arrange & Act
            var sprite = new BitmapSprite(SpriteData.LargeInvaderFrame1);

            // Assert
            Assert.Equal(12, sprite.Width);
            Assert.Equal(8, sprite.Height);
        }
    }
}
