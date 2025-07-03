
using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Input;

namespace SpaceInvaders.Tests.Entities
{
    public class PlayerTests
    {
        // Mock input handler for testing
        private class MockInputHandler : IInputHandler
        {
            public bool IsLeftPressed { get; set; }
            public bool IsRightPressed { get; set; }
            public bool IsFirePressed { get; set; }
            public bool WasFireJustPressed { get; set; }

            public void Update() { }
        }

        [Fact]
        public void Constructor_ShouldThrowOnNullInput()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() 
                => new Player(new Vector2(0, 0), null!));
        }

        [Fact]
        public void Update_ShouldMoveLeft_WhenLeftPressed()
        {
            // Arrange
            var input = new MockInputHandler { IsLeftPressed = true };
            var player = new Player(new Vector2(100, 200), input);
            var initialX = player.Position.X;

            // Act
            player.Update(0.1f);

            // Assert
            Assert.True(player.Position.X < initialX);
            Assert.Equal(200, player.Position.Y); // Y should remain unchanged
        }

        [Fact]
        public void Update_ShouldNotMoveOutOfLeftBound()
        {
            // Arrange
            var input = new MockInputHandler { IsLeftPressed = true };
            var player = new Player(new Vector2(5, 200), input);

            // Act
            player.Update(1.0f);

            // Assert
            Assert.Equal(0, player.Position.X); // Should stop at left edge
        }

        [Fact]
        public void Update_ShouldNotMoveOutOfRightBound()
        {
            // Arrange
            var input = new MockInputHandler { IsRightPressed = true };
            var rightEdge = GameConstants.GAME_WIDTH - 13; // Player width is 13
            var player = new Player(new Vector2(rightEdge - 5, 200), input);

            // Act
            player.Update(1.0f);

            // Assert
            Assert.Equal(rightEdge, player.Position.X);
        }

        [Fact]
        public void Update_ShouldNotMove_WhenBothKeysPressed()
        {
            // Arrange
            var input = new MockInputHandler { 
                IsLeftPressed = true, 
                IsRightPressed = true 
            };
            var player = new Player(new Vector2(100, 200), input);
            var initialPosition = player.Position;
            
            // Act
            player.Update(0.1f);

            // Assert
            Assert.Equal(initialPosition, player.Position);
        }

        [Fact]
        public void FireRequested_ShouldTriggerWhenFirePressed()
        {
            // Arrange
            var input = new MockInputHandler { WasFireJustPressed = true };
            var player = new Player(Vector2.Zero, input);
            bool eventFired = false;
            player.FireRequested += (sender, args) => eventFired = true;

            // Act
            player.Update(0.1f);

            // Assert
            Assert.True(eventFired);
        }

        [Fact]
        public void CanFire_ShouldBeFalseAfterFiring()
        {
            // Arrange
            var input = new MockInputHandler { WasFireJustPressed = true };
            var player = new Player(Vector2.Zero, input);

            // Act
            player.Update(0.1f); // This should trigger the fire event

            // Assert
            Assert.False(player.CanFire);
            Assert.True(player.FireCooldownRemaining > 0);
        }

        [Fact]
        public void CanFire_ShouldBeTrueAfterCooldown()
        {
            // Arrange
            var input = new MockInputHandler { WasFireJustPressed = true };
            var player = new Player(Vector2.Zero, input);
            player.Update(0.1f);

            // Act
            input.WasFireJustPressed = false;
            player.Update(GameConstants.PLAYER_FIRE_COOLDOWN + 0.1f); // Wait for cooldown

            // Assert
            Assert.True(player.CanFire);
            Assert.Equal(0, player.FireCooldownRemaining);
        }
    }
}
