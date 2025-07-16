using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;

namespace SpaceInvaders.Game.Managers
{
    /// <summary>
    /// Manages all projectiles in the game.
    /// Enforces bullet limits and handles lifecycle.
    /// </summary>
    public class BulletManager
    {
        private readonly List<Bullet> _bullets;
        private readonly int _maxPlayerBullets;

        public IEnumerable<Bullet> Bullets => _bullets.Where(b => b.IsActive);
        public IEnumerable<Bullet> PlayerBullets =>
            _bullets.Where(b => b.IsActive && b.Type == BulletType.Player);
        public IEnumerable<Bullet> InvaderBullets =>
            _bullets.Where(b => b.IsActive && b.Type == BulletType.Invader);

        public int ActivePlayerBulletCount =>
            _bullets.Count(b => b.IsActive && b.Type == BulletType.Player);

        public BulletManager(int maxPlayerBullets = GameConstants.MAX_PLAYER_BULLETS)
        {
            _bullets = new List<Bullet>();
            _maxPlayerBullets = maxPlayerBullets;
        }

        public bool TryFirePlayerBullet(Vector2 position)
        {
            // Enforce bullet limit
            if (ActivePlayerBulletCount >= _maxPlayerBullets)
                return false;

            var bullet = new Bullet(
                position,
                GameConstants.BULLET_SPEED,
                BulletType.Player
                );

            _bullets.Add(bullet);
            return true;
        }

        public void FireInvaderBullet(Vector2 position)
        {
            var bullet = new Bullet(
                position,
                -GameConstants.INVADER_BULLET_SPEED,
                BulletType.Invader
                );

            _bullets.Add(bullet);
        }

        public void Update(float deltaTime)
        {
            // Update all active bullets
            foreach (var bullet in _bullets.Where(b => b.IsActive))
                bullet.Update(deltaTime);

            // Clean up inactive bullets periodically
            if (_bullets.Count > 50) // Arbitrary threshold
                RemoveInactiveBullets();
        }

        public void DestroyBullet(Bullet bullet) => bullet.Destroy();

        public void Clear() => _bullets.Clear();

        private void RemoveInactiveBullets() => _bullets.RemoveAll(b => !b.IsActive);
    }
}
