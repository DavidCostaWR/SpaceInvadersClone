using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.Domain
{
    /// <summary>
    /// Central configuration for the game.
    /// Based on the original 1978 Space Invaders specifications.
    /// </summary>
    public static class GameConstants
    {
        // Display
        public const int GAME_WIDTH = 224;
        public const int GAME_HEIGHT = 256;
        public const int DISPLAY_SCALE = 3;
        public const int DISPLAY_WIDTH = GAME_WIDTH * DISPLAY_SCALE;
        public const int DISPLAY_HEIGHT = GAME_HEIGHT * DISPLAY_SCALE;

        // Player
        public const float PLAYER_SPEED = 100f; // Pixels per second
        public const float PLAYER_START_Y = 230f; // Near the bottom of the screen
        public const float PLAYER_FIRE_COOLDOWN = 0.5f; // Seconds between shots
        public static Vector2 PlayerStartPosition
        {
            get
            {
                float playerWidth = 13f; // Default player width
                try
                {
                    var sprite = SpriteRepository.Instance.GetSprite(SpriteKey.Player);
                    playerWidth = sprite.Width;
                }
                catch { }

                return new Vector2((GAME_WIDTH / 2f - playerWidth) / 2f, PLAYER_START_Y);
            }
        }

        // Formation
        public const int INVADER_ROWS = 5;
        public const int INVADER_COLUMNS = 11;
        public const int INVADER_HORIZONTAL_SPACING = 16;
        public const int INVADER_VERTICAL_SPACING = 16;
        public const float INVADER_DROP_DISTANCE = 8f;
        public const float FORMATION_TOP_MARGIN = 16f;
        public const float FORMATION_BOTTOM_DANGER_ZONE = 32f;

        // Movement
        public const float INVADER_BASE_SPEED = 10f;
        public const float INVADER_SPEED_INCREMENT_PER_KILL = 1.01f;
        public const float INVADER_ANIMATION_INTERVAL = 0.5f;

        // Calculated values (as properties)
        public static int TotalInvaders => INVADER_ROWS * INVADER_COLUMNS;
        public static float FormationWidth => INVADER_COLUMNS * INVADER_HORIZONTAL_SPACING;

        // Projectiles
        public const float BULLET_SPEED = 300f;             // Player bullet speed (pixels/second)
        public const float INVADER_BULLET_SPEED = 150f;     // Invader bullets are slower
        public const int MAX_PLAYER_BULLETS = 1;            // Original game allowed only 1
        public const int MAX_INVADER_BULLETS = 3;           // Maximum simultaneous invader shots

        // Game rules
        public const int PLAYER_LIVES = 3;

        // Scoring
        public static class Points
        {
            public const int SmallInvader = 30;
            public const int MediumInvader = 20;
            public const int LargeInvader = 10;
            public const int UFO_MIN = 50;
            public const int UFO_MAX = 300;
        }

        // Timing
        public const int TARGET_FPS = 60;
        public const int FRAME_TIME_MS = 1000 / TARGET_FPS;
    }
}
