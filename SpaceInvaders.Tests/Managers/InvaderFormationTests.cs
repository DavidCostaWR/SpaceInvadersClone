using SpaceInvaders.Game.Managers;
using SpaceInvaders.Game.Domain;
using Xunit;

namespace SpaceInvaders.Tests.Managers
{
    public class InvaderFormationTests
    {
        [Fact]
        public void Constructor_ShouldCreate55Invaders()
        {
            // Arrange & Act
            var formation = new InvaderFormation();

            // Assert
            Assert.Equal(55, formation.ActiveCount);
        }

        [Fact]
        public void Update_ShouldMoveInvadersHorizontally()
        {
            // Arrange
            var formation = new InvaderFormation();
            var firstInvader = formation.Invaders.First();
            var initialX = firstInvader.Position.X;

            // Act
            formation.Update(0.1f);

            // Assert
            var newX = firstInvader.Position.X;
            Assert.True(newX > initialX, "Invader should move right");
        }

        [Fact]
        public void DestroyInvaderAt_ShouldDecreaseActiveCount()
        {
            // Arrange
            var formation = new InvaderFormation();
            var targetInvader = formation.Invaders.First();
            var hitPosition = targetInvader.Position + new Vector2(4,4); // Center

            // Act
            formation.DestroyInvaderAt(hitPosition);

            // Assert
            Assert.Equal(54, formation.ActiveCount);
            Assert.False(targetInvader.IsActive, "Invader should be destroyed");
        }

        [Fact]
        public void InvaderDestroyed_EventSouldFire()
        {
            // Arrange
            var formation = new InvaderFormation();
            var eventFired = false;
            formation.InvaderDestroyed += (s, invader) => eventFired = true;

            var targetInvader = formation.Invaders.First();

            // Act
            formation.DestroyInvaderAt(targetInvader.Position);

            // Assert
            Assert.True(eventFired);
        }
    }
}
