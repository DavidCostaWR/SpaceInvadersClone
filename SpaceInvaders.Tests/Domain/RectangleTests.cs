using SpaceInvaders.Game.Domain;
using Xunit;

namespace SpaceInvaders.Tests.Domain
{
    public class RectangleTests
    {

        [Fact]
        public void Intersects_ShouldDetectCollision()
        {
            // Arrange
            var rect1 = new Rectangle(0, 0, 10, 10);
            var rect2 = new Rectangle(5, 5, 10, 10);
            var rect3 = new Rectangle(20, 20, 10, 10);

            // Act & Assert
            Assert.True(rect1.Intersects(rect2));
            Assert.False(rect1.Intersects(rect3));
        }

        [Fact]
        public void ToDrawingRectangle_ShouldConvertCorrectly()
        {
            // Arrange
            var rect = new Rectangle(10.5f, 20.7f, 30.2f, 40.9f);
            var expected = new System.Drawing.Rectangle(10, 20, 30, 40);    // truncated to int

            // Act
            var drawingRect = rect.ToDrawingRectangle();

            // Assert
            Assert.Equal(expected, drawingRect);
        }
    }
}
