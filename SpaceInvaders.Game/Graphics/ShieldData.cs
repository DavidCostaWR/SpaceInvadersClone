
namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Defines the visual pattern for shields.
    /// Based on the original 1978 Space Invaders shield design.
    /// </summary>
    public static class ShieldData
    {
        public const int SHIELD_WIDTH = 22;
        public const int SHIELD_HEIGHT = 16;

        public static readonly string[] ShieldPattern = new[]
        {
            "    XXXXXXXXXXXXXX    ",
            "   XXXXXXXXXXXXXXXX   ",
            "  XXXXXXXXXXXXXXXXXX  ",
            " XXXXXXXXXXXXXXXXXXXX ",
            "XXXXXXXXXXXXXXXXXXXXXX",
            "XXXXXXXXXXXXXXXXXXXXXX",
            "XXXXXXXXXXXXXXXXXXXXXX",
            "XXXXXXXXXXXXXXXXXXXXXX",
            "XXXXXXXXXXXXXXXXXXXXXX",
            "XXXXXXXXXXXXXXXXXXXXXX",
            "XXXXXXXXXXXXXXXXXXXXXX",
            "XXXXXXX      XXXXXXXXX",
            "XXXXXX        XXXXXXXX",
            "XXXXX          XXXXXXX",
            "XXXXX          XXXXXXX",
            "XXXXX          XXXXXXX"
        };
    }
}
