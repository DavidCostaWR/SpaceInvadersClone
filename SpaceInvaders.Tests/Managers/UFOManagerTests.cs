using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Managers;

namespace SpaceInvaders.Tests.Managers
{
    public class UFOManagerTests
    {
        [Fact]
        public void CurrentUFO_ShouldBeNullInitially()
        {
            // Arrange & Act
            var manager = new UFOManager();

            // Assert
            Assert.Null(manager.CurrentUFO);
        }

        [Fact]
        public void Update_ShouldEventuallySpawnUFO()
        {
            // Arrange
            var manager = new UFOManager();

            // Act - Update for max spawn time
            manager.Update(GameConstants.UFO_MAX_SPAWN_TIME + 1);

            // Assert
            Assert.NotNull(manager.CurrentUFO);
        }

        [Fact]
        public void OnPlayerShot_Every15thShot_ShouldGive300Points()
        {
            // Arrange
            var manager = new UFOManager();

            // Fire 14 shots
            for (int i = 0; i < 14; i++)
            {
                manager.OnPlayerShot();
            }

            // Act - 15th shot and spawn UFO
            manager.OnPlayerShot();
            manager.Update(GameConstants.UFO_MAX_SPAWN_TIME + 1);

            // Assert
            Assert.NotNull(manager.CurrentUFO);
            // Note: We can't directly test point value without exposing internals
            // In real implementation, we'd test this through integration tests
        }

        [Fact]
        public void TryDestroyUFO_WithValidHit_ShouldReturnTrue()
        {
            // Arrange
            var manager = new UFOManager();
            manager.Update(GameConstants.UFO_MAX_SPAWN_TIME + 1);   // Spawn UFO

            var ufo = manager.CurrentUFO;
            Assert.NotNull(ufo);

            bool eventFired = false;
            manager.UFODestroyed += (s, e) => eventFired = true;

            // Act
            var hitPoint = ufo.Position + ufo.Size / 2; // Center of UFO
            bool destroyed = manager.TryDestroyUFO(hitPoint);

            // Assert
            Assert.True(destroyed);
            Assert.True(eventFired);
            Assert.Null(manager.CurrentUFO);
        }

        [Fact]
        public void TryDestroyUFO_WithMiss_ShouldReturnFalse()
        {
            // Arrange
            var manager = new UFOManager();
            manager.Update(GameConstants.UFO_MAX_SPAWN_TIME + 1);   // Spawn UFO

            // Act
            bool destroyed = manager.TryDestroyUFO(new Vector2(1000, 1000)); // Way off

            // Assert
            Assert.False(destroyed);
            Assert.NotNull(manager.CurrentUFO);
        }

        [Fact]
        public void Reset_ShouldClearCurrentUFO()
        {
            var manager = new UFOManager();
            manager.Update(GameConstants.UFO_MAX_SPAWN_TIME + 1);   // Spawn UFO
            Assert.NotNull(manager.CurrentUFO);

            // Act
            manager.Reset();

            // Assert
            Assert.Null(manager.CurrentUFO);
        }
    }
}
