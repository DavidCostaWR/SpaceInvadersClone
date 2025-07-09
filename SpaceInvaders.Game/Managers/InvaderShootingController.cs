using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;

namespace SpaceInvaders.Game.Managers
{
    /// <summary>
    /// Controls when and which invaders shoot.
    /// Implements classic Space Invaders shooting pattern
    /// </summary>
    public class InvaderShootingController
    {
        private readonly Random _random;
        private readonly BulletManager _bulletManager;
        private float _shootTimer;
        private float _shootInterval;

        public InvaderShootingController(BulletManager bulletManager)
        {
            _bulletManager = bulletManager ?? throw new ArgumentNullException(nameof(bulletManager));
            _random = new Random();
            _shootInterval = CalculateShootInterval(GameConstants.TotalInvaders);
            _shootTimer = _shootInterval;
        }

        public void Update(float deltaTime, IEnumerable<Invader> invaders)
        {
            _shootTimer -= deltaTime;

            if (_shootTimer <= 0)
            {
                TryShoot(invaders);

                // Reset timer with some randomness
                var activeCount = invaders.Count();
                _shootInterval = CalculateShootInterval(activeCount);
                _shootTimer = _shootInterval * (0.5f + (float)_random.NextDouble());
            }
        }

        private void TryShoot(IEnumerable<Invader> invaders)
        {
            // Get bottom-most invader in each column
            var shootCandidates = GetBottomInvaders(invaders).ToList();

            if (shootCandidates.Count == 0) return;

            // Check current bullet count
            var currentBullets = _bulletManager.InvaderBullets.Count();
            if (currentBullets >= GameConstants.MAX_INVADER_BULLETS) return;

            // Select random invader to shoot
            var shooter = shootCandidates[_random.Next(shootCandidates.Count)];

            // Fire from bottom center of invader
            var bulletPosition = new Vector2(
                shooter.Position.X + shooter.Size.X / 2f - 1.5f,    // Center 3-pixel bullet
                shooter.Position.Y
                );

            _bulletManager.FireInvaderBullet(bulletPosition);
        }

        private IEnumerable<Invader> GetBottomInvaders(IEnumerable<Invader> invaders)
        {
            return invaders
                .GroupBy(i => Math.Round(i.Position.X / GameConstants.INVADER_HORIZONTAL_SPACING))
                .Select(group => group.OrderByDescending(i => i.Position.Y).First());
        }

        private float CalculateShootInterval(int activeInvaderCount)
        {
            // Shoot more frequently as invaders are destroyed
            // Start at 3 seconds, minimum 0.5 seconds
            var ratio = (float)activeInvaderCount / GameConstants.TotalInvaders;
            return Math.Max(0.5f, 3.0f * ratio);
        }

        public void Reset()
        {
            _shootInterval = CalculateShootInterval(GameConstants.TotalInvaders);
            _shootTimer = _shootInterval;
        }
    }
}
