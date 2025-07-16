using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Managers;

namespace SpaceInvaders.Tests.Managers
{
    public class BulletManagerTests
    {
        [Fact]
        public void TryFirePlayerBullet_ShouldRespectMaxLimit()
        {
            // Arrange
            var manager = new BulletManager(maxPlayerBullets: 1);

            // Act
            bool firstShot = manager.TryFirePlayerBullet(Vector2.Zero);
            bool secondShot = manager.TryFirePlayerBullet(Vector2.Zero);

            // Assert
            Assert.True(firstShot);
            Assert.False(secondShot);
            Assert.Equal(1, manager.ActivePlayerBulletCount);
        }

        [Fact]
        public void Update_ShouldRemoveBulletsOutOfBounds()
        {
            // Arrange
            var manager = new BulletManager();
            manager.TryFirePlayerBullet(new Vector2(50, 10));

            // Act
            manager.Update(10f);

            // Assert
            Assert.Equal(0, manager.ActivePlayerBulletCount);
        }

        [Fact]
        public void FireInvaderBullet_ShouldNotHaveLimit()
        {
            // Arrange
            var manager = new BulletManager();

            // Act - Fire multiple invader bullets
            for (int i = 0; i < 5; i++)
            {
                manager.FireInvaderBullet(new Vector2(i * 10, 100));
            }

            // Assert
            Assert.Equal(5, manager.InvaderBullets.Count());
        }

        [Fact]
        public void Clear_ShouldRemoveAllBullets()
        {
            // Arrange
            var manager = new BulletManager();
            manager.TryFirePlayerBullet(Vector2.Zero);
            manager.FireInvaderBullet(Vector2.Zero);

            // Act
            manager.Clear();

            // Assert
            Assert.Empty(manager.Bullets);
        }
    }
}
