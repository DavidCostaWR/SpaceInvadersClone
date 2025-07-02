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

        // Formation
        public const int INVADER_ROWS = 5;
        public const int INVADER_COLUMNS = 11;
        public const int INVADER_HORIZONTAL_SPACING = 16;
        public const int INVADER_VERTICAL_SPACING = 16;
        public const float INVADER_DROP_DISTANCE = 8f;
        public const float FORMATION_TOP_MARGIN = 32f;
        public const float FORMATION_BOTTOM_DANGER_ZONE = 32f;

        // Movement
        public const float INVADER_BASE_SPEED = 30f;
        public const float INVADER_SPEED_INCREMENT_PER_KILL = 1.5f;
        public const float INVADER_ANIMATION_INTERVAL = 0.5f;

        // Calculated values (as properties)
        public static int TotalInvaders => INVADER_ROWS * INVADER_COLUMNS;
        public static float FormationWidth => INVADER_COLUMNS * INVADER_HORIZONTAL_SPACING;

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
