using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;

namespace SpaceInvaders.Tests.Entities
{
    public class InvaderTests
    {
        [Theory]
        [InlineData(InvaderType.Small, 30)]
        [InlineData(InvaderType.Medium, 20)]
        [InlineData(InvaderType.Large, 10)]
        public void Constructor_ShouldSetCorrectPointValue(
            InvaderType type, int expectedPoints)
        {
            // Arrange & Act
            var invader = new Invader(Vector2.Zero, type);

            // Assert
            Assert.Equal(expectedPoints, invader.PointValue);
        }

        [Fact]
        public void Move_ShouldUpdatePosition()
        {
            // Arrange
            var invader = new Invader(new Vector2(10, 10), InvaderType.Small);
            var movement = new Vector2(5, 0);

            // Act
            invader.Move(movement);

            // Assert
            Assert.Equal(15, invader.Position.X);
            Assert.Equal(10, invader.Position.Y);
        }

        [Fact]
        public void Bounds_ShouldBeCorrectForType()
        {
            // Arrange
            var position = new Vector2(10, 20);
            var invader = new Invader(position, InvaderType.Small);

            // Act
            var bounds = invader.Bounds;

            // Assert
            Assert.Equal(10, bounds.X);
            Assert.Equal(20, bounds.Y);
            Assert.Equal(8, bounds.Width);
            Assert.Equal(8, bounds.Height);
        }
    }
}
