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
        private readonly Dictionary<string, ISprite> _sprites;

        public static SpriteRepository Instance => _instance ??= new SpriteRepository();

        private SpriteRepository()
        {
            _sprites = new Dictionary<string, ISprite>();
            LoadSprites();
        }

        private void LoadSprites()
        {
            // Invader sprites
            _sprites["invader.small.1"] = new BitmapSprite(SpriteData.SmallInvader);
            _sprites["invader.small.2"] = new BitmapSprite(SpriteData.SmallInvaderFrame2);

            // Add more sprites as they are implemented
        }

        public ISprite GetSprite(string key)
        {
            if (_sprites.TryGetValue(key, out var sprite))
                return sprite;

            throw new ArgumentException($"Sprite not found: {key}");
        }

        public ISprite GetInvaderSprite(InvaderType type, int animationFrame = 0)
        {
            string key = type switch
            {
                // Todo: implement medium and large
                InvaderType.Small => animationFrame == 0 ? "invader.small.1" : "invader.small.2",
                InvaderType.Medium => animationFrame == 0 ? "invader.small.1" : "invader.small.2",
                InvaderType.Large => animationFrame == 0 ? "invader.small.1" : "invader.small.2",
                _ => throw new ArgumentException($"Unknown invader type: {type}")
            };

            return GetSprite(key);
        }
    }
}
