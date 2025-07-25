
using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.Entities
{
    /// <summary>
    /// Represents a destructible shield/bunker that protects the player
    /// Each shield is made up of individual pixels that can be destroyed
    /// </summary>
    public class Shield : Entity
    {
        private readonly bool[,] _pixels;
        private readonly int _pixelWidth;
        private readonly int _pixelHeight;

        private readonly bool[,] _originalPattern;
        public bool[,] GetPixels() => _pixels;

        public Shield(Vector2 position)
            : base(position, new Vector2(ShieldData.SHIELD_WIDTH, ShieldData.SHIELD_HEIGHT))
        {
            _pixelWidth = ShieldData.SHIELD_WIDTH;
            _pixelHeight = ShieldData.SHIELD_HEIGHT;

            _pixels = new bool[_pixelWidth, _pixelHeight];
            _originalPattern = new bool[_pixelWidth, _pixelHeight];

            InitializePixels();
        }

        private void InitializePixels()
        {
            var pattern = ShieldData.ShieldPattern;

            for (int y = 0; y < _pixelHeight; y++)
            {
                for (int x = 0; x < _pixelWidth; x++)
                {
                    bool hasPixel = pattern[y][x] != ' ';
                    _pixels[x, y] = hasPixel;
                    _originalPattern[x, y] = hasPixel;
                }
            }
        }

        public override void Update(float deltaTime) { }

        /// <summary>
        /// Checks if a bullet hits any pixel of the shield.
        /// Returns true if hit occurreed and damages the shield.
        /// </summary>
        public bool CheckBulletCollision(Bullet bullet)
        {
            if (!bullet.Bounds.Intersects(Bounds)) return false;

            var localX = (int)(bullet.Position.X - Position.X);
            var localY = (int)(bullet.Position.Y - Position.Y);

            for (int by = 0; by < bullet.Size.Y; by++)
            {
                for (int bx = 0; bx < bullet.Size.X; bx++)
                {
                    int pixelX = localX + bx;
                    int pixelY = localY + by;

                    if (pixelX >= 0 && pixelX < _pixelWidth &&
                        pixelY >= 0 && pixelY < _pixelHeight)
                    {
                        if (_pixels[pixelX, pixelY])
                        {
                            // Hit!
                            DamageArea(pixelX, pixelY, bullet.Type);
                            return true; // Hit occurred
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Damages an area of the shield based on impact point.
        /// Different bullet types create different damage patterns.
        /// </summary>
        private void DamageArea(int impactX, int impactY, BulletType bulletType)
        {
            int damageRadius = bulletType == BulletType.Player ? 3 : 4;

            for (int dy = -damageRadius; dy <= damageRadius; dy++)
            {
                for (int dx = -damageRadius; dx <= damageRadius; dx++)
                {
                    int x = impactX + dx;
                    int y = impactY + dy;

                    if (x >= 0 && x < _pixelWidth &&
                        y >= 0 && y < _pixelHeight)
                    {
                        // Circular damage with some randomness
                        float distance = (float)Math.Sqrt(dx * dx + dy * dy);
                        if (distance <= damageRadius)
                        {
                            float destructionChance = 1.0f - (distance / damageRadius) * 0.3f;
                            if (Random.Shared.NextSingle() < destructionChance)
                            {
                                _pixels[x, y] = false;
                            }
                        }
                    }
                }
            }
        }

        public void Reset()
        {
            for (int y = 0; y < _pixelHeight; y++)
            {
                for (int x = 0; x < _pixelWidth; x++)
                {
                    _pixels[x, y] = _originalPattern[x, y];
                }
            }
            IsActive = true;
        }

        public bool IsCompletelyDestroyed()
        {
            for (int y = 0; y < _pixelHeight; y++)
            {
                for (int x = 0; x < _pixelWidth; x++)
                {
                    if (_pixels[x, y]) return false;
                }
            }
            return true;
        }
    }
}
