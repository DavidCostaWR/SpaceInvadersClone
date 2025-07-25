
using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.Managers
{
    /// <summary>
    /// Manages all shields in the game.
    /// Handles positioning and collision detection.
    /// </summary>
    public class ShieldManager
    {
        private readonly List<Shield> _shields;
        private const int SHIELD_COUNT = 4;
        private const float SHIELD_Y_POSITION = 180f;

        public IEnumerable<Shield> Shields => _shields.Where(s => s.IsActive);

        public ShieldManager()
        {
            _shields = new List<Shield>();
            InitializeShields();
        }

        private void InitializeShields()
        {
            float totalShieldWidth = ShieldData.SHIELD_WIDTH * SHIELD_COUNT;
            float totalSpacing = GameConstants.GAME_WIDTH - totalShieldWidth;
            float spacingBetween = totalSpacing / (SHIELD_COUNT + 1);

           
            for (int i = 0; i < SHIELD_COUNT; i++)
            {
                float x = spacingBetween + i * (ShieldData.SHIELD_WIDTH + spacingBetween);
                var position = new Vector2(x, SHIELD_Y_POSITION);
                _shields.Add(new Shield(position));
            }
        }

        public bool CheckBulletCollision(Bullet bullet)
        {
            foreach (var shield in _shields)
            {
                if (shield.CheckBulletCollision(bullet))
                {
                    if (shield.IsCompletelyDestroyed())
                        shield.Destroy();
                    return true;
                }
            }
            return false;
        }

        public void CheckInvaderCollisions(IEnumerable<Invader> invaders)
        {
            foreach (var shield in _shields)
            {
                foreach (var invader in invaders.Where(i => i.IsActive))
                {
                    if (shield.Bounds.Intersects(invader.Bounds))
                    {
                        shield.Destroy();
                        break;
                    }
                }
            }
        }

        public void Reset()
        {
            foreach (var shield in _shields)
            {
                shield.Reset();
            }
        }
    }
}
