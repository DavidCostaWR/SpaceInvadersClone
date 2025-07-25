using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;

namespace SpaceInvaders.Game.Managers
{
    /// <summary>
    /// Handles collision detection between game entities.
    /// Centralized collision logic for maintainability
    /// </summary>
    public class CollisionManager
    {
        private readonly ShieldManager _shieldManager;

        public event EventHandler<CollisionEventArgs>? CollisionDetected;

        public CollisionManager(ShieldManager shieldManager)
        {
            _shieldManager = shieldManager ?? throw new ArgumentNullException(nameof(shieldManager));
        }

        public void CheckCollisions(
            IEnumerable<Bullet> bullets,
            IEnumerable<Invader> invaders,
            Player player)
        {
            CheckBulletsVsShields(bullets);
            CheckPlayerBulletsVsInvaders(bullets, invaders);
            CheckInvaderBulletsVsPlayer(bullets, player);
            _shieldManager.CheckInvaderCollisions(invaders);
        }

        private void CheckBulletsVsShields(IEnumerable<Bullet> bullets)
        {
            foreach (var bullet in bullets.Where(b => b.IsActive))
            {
                if (_shieldManager.CheckBulletCollision(bullet))
                {
                    bullet.Destroy();
                }
            }
        }

        private void CheckPlayerBulletsVsInvaders(
            IEnumerable<Bullet> bullets, 
            IEnumerable<Invader> invaders)
        {
            var playerBullets = bullets
                .Where(b => b.IsActive && b.Type == BulletType.Player)
                .ToList();

            foreach (var bullet in playerBullets)
            {
                foreach (var invader in invaders.Where(i => i.IsActive))
                {
                    if (bullet.Bounds.Intersects(invader.Bounds))
                    {
                        // Hit detected!
                        OnCollision(bullet, invader, CollisionType.PlayerBulletHitInvader);
                        break;
                    }
                }
            }
        }

        private void CheckInvaderBulletsVsPlayer(
            IEnumerable<Bullet> bullets, 
            Player player)
        {
            if (!player.IsActive || !player.IsVulnerable) return;

            var invaderBullets = bullets
                .Where(b => b.IsActive && b.Type == BulletType.Invader)
                .ToList();

            foreach (var bullet in invaderBullets)
            {
                if (bullet.Bounds.Intersects(player.Bounds))
                {
                    OnCollision(bullet, player, CollisionType.InvaderBulletHitPlayer);
                    break;
                }
            }
        }

        private void OnCollision(Entity entity1, Entity entity2, CollisionType type)
        {
            var args = new CollisionEventArgs(entity1, entity2, type);
            CollisionDetected?.Invoke(this, args);
        }
    }

    public class CollisionEventArgs : EventArgs
    {
        public Entity Entity1 { get; }
        public Entity Entity2 { get; }
        public CollisionType Type { get; }

        public CollisionEventArgs(Entity entity1, Entity entity2, CollisionType type)
        {
            Entity1 = entity1;
            Entity2 = entity2;
            Type = type;
        }
    }

    public enum CollisionType
    {
        PlayerBulletHitInvader,
        InvaderBulletHitPlayer,
        InvaderReachedPlayer
    }
}
