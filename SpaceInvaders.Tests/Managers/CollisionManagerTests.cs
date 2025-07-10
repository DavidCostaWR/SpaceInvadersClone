using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Input;
using SpaceInvaders.Game.Managers;

namespace SpaceInvaders.Tests.Managers
{
    public class CollisionManagerTests
    {
        private class MockInputHandler : IInputHandler
        {
            public bool IsLeftPressed => false;
            public bool IsRightPressed => false;
            public bool IsFirePressed => false;
            public bool WasFireJustPressed => false;
            public void Update() { }
        }

        [Fact]
        public void CheckCollisions_ShouldDetectPlayerBulletHittingInvader()
        {
            // Arrange
            var manager = new CollisionManager();
            var bullet = new Bullet(new Vector2(10, 10), 100, BulletType.Player);
            var invader = new Invader(new Vector2(10, 10), InvaderType.Small);
            var player = new Player(Vector2.Zero, new MockInputHandler());

            bool collisionDetected = false;
            CollisionType? detectedType = null;

            manager.CollisionDetected += (s, e) =>
            {
                collisionDetected = true;
                detectedType = e.Type;
            };

            // Act
            manager.CheckCollisions(new[] { bullet }, new[] { invader }, player);

            // Assert
            Assert.True(collisionDetected);
            Assert.Equal(CollisionType.PlayerBulletHitInvader, detectedType);
        }

        [Fact]
        public void CheckCollisions_ShouldDetectInvaderBulletHittingPlayer()
        {
            // Arrange
            var manager = new CollisionManager();
            var bullet = new Bullet(new Vector2(10, 10), -100, BulletType.Invader);
            var player = new Player(new Vector2(10, 10), new MockInputHandler());

            bool collisionDetected = false;
            CollisionType? detectedType = null;

            manager.CollisionDetected += (s, e) =>
            {
                collisionDetected = true;
                detectedType = e.Type;
            };

            // Act
            manager.CheckCollisions(new[] { bullet }, new Invader[] { }, player);

            // Assert
            Assert.True(collisionDetected);
            Assert.Equal(CollisionType.InvaderBulletHitPlayer, detectedType);
        }

        [Fact]
        public void CheckCollisions_ShouldIgnoreInvaderBullets_WhenPlayerNotVulnerable()
        {
            // Arrange
            var manager = new CollisionManager();
            var bullet = new Bullet(new Vector2(10, 10), -100, BulletType.Invader);
            var player = new Player(new Vector2(10, 10), new MockInputHandler());

            // Make player invulnerable
            player.Hit();   // Player is now dying/not vulnerable

            bool collisionDetected = false;

            manager.CollisionDetected -= (s, e) => collisionDetected = true;

            // Act
            manager.CheckCollisions(new[] { bullet }, new Invader[] { }, player);

            // Assert
            Assert.False(collisionDetected);
        }

        [Fact]
        public void CheckCollision_ShouldOnlyDetectOneHitPerFrame()
        {
            // Arrange
            var manager = new CollisionManager();
            var bullet1 = new Bullet(new Vector2(10, 10), -100, BulletType.Invader);
            var bullet2 = new Bullet(new Vector2(10, 10), -100, BulletType.Invader);
            var player = new Player(new Vector2(10, 10), new MockInputHandler());

            int collisionCount = 0;
            manager.CollisionDetected += (s, e) => collisionCount++;

            // Act
            manager.CheckCollisions([bullet1, bullet2], [], player);

            // Assert
            Assert.Equal(1, collisionCount);
        }

        [Fact]
        public void CheckCollisions_ShouldIgnoreInactiveEntities()
        {
            // Arrange
            var manager = new CollisionManager();
            var bullet = new Bullet(new Vector2(10, 10), -100, BulletType.Invader);
            var invader = new Invader(new Vector2(10, 10), InvaderType.Small);
            var player = new Player(Vector2.Zero, new MockInputHandler());

            // Destroy invader
            invader.Destroy();

            bool collisionDetected = false;
            manager.CollisionDetected += (s, e) => collisionDetected = true;

            // Act
            manager.CheckCollisions([bullet], [invader], player);

            // Assert
            Assert.False(collisionDetected);

        }
    }
}
