using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;

namespace SpaceInvaders.Tests.Entities
{
    public class UFOTests
    {
        [Fact]
        public void Constructur_MovingRight_ShouldStartOffScreenLeft()
        {
            // Arrange & Act
            var ufo = new UFO(movingRight: true, pointValue: 100);

            // Assert
            Assert.True(ufo.Position.X < 0);
            Assert.Equal(GameConstants.UFO_Y_POSITION, ufo.Position.Y);
            Assert.True(ufo.IsMovingRight);
        }

        [Fact]
        public void Constructor_MovingLeft_ShouldStartOffScreenRight()
        {
            // Arrange & Act
            var ufo = new UFO(movingRight: false, pointValue: 100);

            // Assert
            Assert.True(ufo.Position.X >= GameConstants.GAME_WIDTH);
            Assert.Equal(GameConstants.UFO_Y_POSITION, ufo.Position.Y);
            Assert.False(ufo.IsMovingRight);
        }

        [Fact]
        public void Update_ShouldMoveInCorrectDirection()
        {
            // Arrange
            var ufoRight = new UFO(movingRight: true, pointValue: 100);
            var ufoLeft = new UFO(movingRight: false, pointValue: 100);
            var initialXRight = ufoRight.Position.X;
            var initialXLeft = ufoLeft.Position.X;

            // Act
            ufoRight.Update(0.1f);
            ufoLeft.Update(0.1f);

            // Assert
            Assert.True(ufoRight.Position.X > initialXRight);
            Assert.True(ufoLeft.Position.X < initialXLeft);
        }

        [Fact]
        public void Update_ShouldDestroyWhenOffScreen()
        {
            // Arrange
            var ufo = new UFO(movingRight: true, pointValue: 100);

            // Act - Update with large delta to move off screen
            ufo.Update(10f);

            // Assert
            Assert.False(ufo.IsActive);
        }

        [Theory]
        [InlineData(50)]
        [InlineData(100)]
        [InlineData(150)]
        [InlineData(300)]
        public void PointValue_ShouldBeSetCorrectly(int points)
        {
            // Arrange & Act
            var ufo = new UFO(movingRight: true, pointValue: points);

            // Assert
            Assert.Equal(points, ufo.PointValue);
        }
    }
}
