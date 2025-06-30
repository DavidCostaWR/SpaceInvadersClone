namespace SpaceInvaders.Game.Domain
{
    /// <summary>
    /// Central configuration for the game.
    /// Based on the original 1978 Space Invaders specifications.
    /// </summary>
    public static class GameConstraints
    {
        // Original arcade resolution
        public const int GAME_WIDTH = 224;
        public const int GAME_HEIGHT = 256;

        // Scale for modern displays
        public const int DISPLAY_SCALE = 3;

        // Calculated display size
        public const int DISPLAY_WIDTH = GAME_WIDTH * DISPLAY_SCALE;
        public const int DISPLAY_HEIGHT = GAME_HEIGHT * DISPLAY_SCALE;

        // Game rules
        public const int INVADER_ROWS = 5;
        public const int INVADER_COLUMNS = 11;
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
