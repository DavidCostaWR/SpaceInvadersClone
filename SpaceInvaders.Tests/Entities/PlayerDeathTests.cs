
using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Input;

namespace SpaceInvaders.Tests.Entities
{
    public class PlayerDeathTests
    {
        private class MockInputHandler : IInputHandler
        {
            public bool IsLeftPressed { get; set; }
            public bool IsRightPressed { get; set; }
            public bool IsFirePressed { get; set; }
            public bool WasFireJustPressed { get; set; }
            public void Update() { }
        }

        [Fact]
        public void Hit_ShouldChangeStateToDying()
        {
            // Arrange
            var input = new MockInputHandler();
            var player = new Player(Vector2.Zero, input);

            // Act
            player.Hit();

            // Assert
            Assert.Equal(PlayerState.Dying, player.State);
            Assert.False(player.IsVulnerable);
        }

        [Fact]
        public void Hit_WhenAlreadyDying_ShouldIgnore()
        {
            // Arrange
            var input = new MockInputHandler();
            var player = new Player(Vector2.Zero, input);
            player.Hit();   // First hit

            // Act
            player.Hit();   // Second hit

            // Assert
            Assert.Equal(PlayerState.Dying, player.State);
        }

        [Fact]
        public void Update_WhenDying_ShouldTransitionToDead()
        {
            // Arrange
            var input = new MockInputHandler();
            var player = new Player(Vector2.Zero, input);
            bool eventFired = false;
            player.DeathAnimationComplete += (s, e) => eventFired = true;

            player.Hit();

            // Act - Update for full death animation duration
            player.Update(2.0f);

            // Assert
            Assert.Equal(PlayerState.Dead, player.State);
            Assert.True(eventFired);
        }

        [Fact]
        public void Respawn_ShouldSetStateToRespawning()
        {
            // Arrange
            var input = new MockInputHandler();
            var player = new Player(Vector2.Zero, input);
            var newPosition = new Vector2(100, 200);

            // Act
            player.Respawn(newPosition);

            // Assert
            Assert.Equal(PlayerState.Respawning, player.State);
            Assert.Equal(newPosition, player.Position);
            Assert.False(player.IsVulnerable);
        }

        [Fact]
        public void Update_WhenRespawning_ShouldTransitionToAlive()
        {
            // Arrange
            var input = new MockInputHandler();
            var player = new Player(Vector2.Zero, input);
            player.Respawn(Vector2.Zero);

            // Act - Update for full respawn duration
            player.Update(3.0f);

            // Assert
            Assert.Equal(PlayerState.Alive, player.State);
            Assert.True(player.IsVulnerable);
        }

        [Fact]
        public void CanFire_ShouldBeFalse_WhenNotAlive()
        {
            // Arrange
            var input = new MockInputHandler();
            var player = new Player(Vector2.Zero, input);

            // Act & Assert - Dying state
            player.Hit();
            Assert.False(player.CanFire);

            // Act & Assert - Respawning State
            player.Respawn(Vector2.Zero);
            Assert.False(player.CanFire);
        }

        [Fact]
        public void ShouldRender_ShouldBlink_WhenRespawning()
        {
            // Arrange
            var input = new MockInputHandler();
            var player = new Player(Vector2.Zero, input);
            player.Respawn(Vector2.Zero);

            // Act & Assert - Check blinking behavior
            var renderStates = new List<bool>();
            for (int i = 0; i < 10; i++)
            {
                player.Update(0.1f);    // Update by blink interval
                renderStates.Add(player.ShouldRender);
            }

            // Should have both true and false values (blinking)
            Assert.Contains(true, renderStates);
            Assert.Contains(false, renderStates);
        }

        [Fact]
        public void Movement_ShouldBeDisabled_WhenNotAlive()
        {
            // Arrange
            var input = new MockInputHandler();
            var player = new Player(Vector2.Zero, input);
            var initialPosition = player.Position;
            input.IsLeftPressed = true;

            // Act - Try to move while dying
            player.Hit();
            player.Update(0.1f);

            // Assert
            Assert.Equal(initialPosition, player.Position);
        }
    }
}
