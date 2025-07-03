using SpaceInvaders.Game.Entities;

namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Central repository for all game sprites.
    /// Singleton pattern ensures sprites are loaded once.
    /// </summary>
    public class SpriteRepository
    {
        private static SpriteRepository? _instance;
        private readonly Dictionary<SpriteKey, ISprite> _sprites;

        public static SpriteRepository Instance => _instance ??= new SpriteRepository();

        private SpriteRepository()
        {
            _sprites = new Dictionary<SpriteKey, ISprite>();
            LoadSprites();
        }

        private void LoadSprites()
        {
            // Invader sprites
            _sprites[SpriteKey.InvaderSmallFrame1] = new BitmapSprite(SpriteData.SmallInvaderFrame1);
            _sprites[SpriteKey.InvaderSmallFrame2] = new BitmapSprite(SpriteData.SmallInvaderFrame2);
            _sprites[SpriteKey.InvaderMediumFrame1] = new BitmapSprite(SpriteData.MediumInvaderFrame1);
            _sprites[SpriteKey.InvaderMediumFrame2] = new BitmapSprite(SpriteData.MediumInvaderFrame2);
            _sprites[SpriteKey.InvaderLargeFrame1] = new BitmapSprite(SpriteData.LargeInvaderFrame1);
            _sprites[SpriteKey.InvaderLargeFrame2] = new BitmapSprite(SpriteData.LargeInvaderFrame2);
            _sprites[SpriteKey.Player] = new BitmapSprite(SpriteData.Player);
            _sprites[SpriteKey.PlayerExplosion1] = new BitmapSprite(SpriteData.PlayerExplosionFrame1);
            _sprites[SpriteKey.PlayerExplosion1] = new BitmapSprite(SpriteData.PlayerExplosionFrame1);
            _sprites[SpriteKey.PlayerBullet] = new BitmapSprite(SpriteData.PlayerBullet);
            _sprites[SpriteKey.InvaderBulletFrame1] = new BitmapSprite(SpriteData.InvaderBulletFrame1);
            _sprites[SpriteKey.InvaderBulletFrame2] = new BitmapSprite(SpriteData.InvaderBulletFrame2);
        }

        public ISprite GetSprite(SpriteKey key)
        {
            if (_sprites.TryGetValue(key, out var sprite))
                return sprite;

            throw new ArgumentException($"Sprite not found: {key}");
        }

        public ISprite GetInvaderSprite(InvaderType type, int animationFrame = 0)
        {
            var key = (type, animationFrame) switch
            {
                (InvaderType.Small, 0) => SpriteKey.InvaderSmallFrame1,
                (InvaderType.Small, _) => SpriteKey.InvaderSmallFrame2,
                (InvaderType.Medium, 0) => SpriteKey.InvaderMediumFrame1,
                (InvaderType.Medium, _) => SpriteKey.InvaderMediumFrame2,
                (InvaderType.Large, 0) => SpriteKey.InvaderLargeFrame1,
                (InvaderType.Large, _) => SpriteKey.InvaderLargeFrame2,
                _ => throw new ArgumentException($"Invalid invader type or frame: {type}, {animationFrame}")
            };

            return GetSprite(key);
        }
    }
}
