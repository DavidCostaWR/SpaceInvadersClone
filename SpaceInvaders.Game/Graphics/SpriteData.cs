
namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Original Space Invaders sprite patterns.
    /// Based on 1978 arcade ROM data.
    /// </summary>
    public static class SpriteData
    {
        // Small invader - 8x8 pixels
        public static readonly string[] SmallInvader = new[]
        {
            "  X  X  ",
            "   XX   ",
            "  XXXX  ",
            " XX  XX ",
            "XXXXXXXX",
            "X XXXX X",
            "X      X",
            " X    X "
        };

        // Animation frame 2
        public static readonly string[] SmallInvaderFrame2 = new[]
        {
            "  X  X  ",
            "X  XX  X",
            "X XXXX X",
            "XXX  XXX",
            "XXXXXXXX",
            " XXXXXX ",
            "  X  X  ",
            "X      X"
        };

        // TODO: Add other sprites as needed
    }
}
