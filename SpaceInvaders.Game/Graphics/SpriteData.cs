
namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Original Space Invaders sprite patterns.
    /// Based on 1978 arcade ROM data.
    /// </summary>
    public static class SpriteData
    {
        // Small invader - 8x8 pixels
        public static readonly string[] SmallInvaderFrame1 = 
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
        public static readonly string[] SmallInvaderFrame2 = 
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

        // Medium invader - 11x8 pixels
        public static readonly string[] MediumInvaderFrame1 =
        [
            "   X   X   ",
            "  XXX XXX  ",
            " XXXXXXXXX ",
            "XXX XXX XXX",
            "XXXXXXXXXXX",
            "X XXXXXXX X",
            "X X     X X",
            "   XX XX   "
        ];
        public static readonly string[] MediumInvaderFrame2 =
        [
            "   X   X   ",
            "X XXX XXX X",
            "XXXXXXXXXXX",
            "XXX XXX XXX",
            " XXXXXXXXX ",
            "  XXXXXXX  ",
            " X       X ",
            "X         X"
        ];

        // Large invader - 12x8 pixels
        public static readonly string[] LargeInvaderFrame1 =
        {
            "    XXXX    ",
            " XXXXXXXXXX ",
            "XXXXXXXXXXXX",
            "XXX  XX  XXX",
            "XXXXXXXXXXXX",
            "  XX    XX  ",
            " XX  XX  XX ",
            "XX        XX"
        };
        public static readonly string[] LargeInvaderFrame2 =
        {
            "    XXXX    ",
            " XXXXXXXXXX ",
            "XXXXXXXXXXXX",
            "XXX  XX  XXX",
            "XXXXXXXXXXXX",
            "   XX  XX   ",
            "  XX XX XX  ",
            "XX   XX   XX"
        };

        // Player cannon - 13x8 pixels
        public static readonly string[] Player =
        {
            "      X      ",
            "     XXX     ",
            "     XXX     ",
            " XXXXXXXXXXX ",
            "XXXXXXXXXXXXX",
            "XXXXXXXXXXXXX",
            "XXXXXXXXXXXXX",
            "XXXXXXXXXXXXX"
        };

        // Player explosion animation - 15x8 pixels
        public static readonly string[] PlayerExplosionFrame1 =
        {
            "    X     X    ",
            " X   X   X   X ",
            "  X   XXX   X  ",
            "   XX  X  XX   ",
            " XXXXXXXXXXXXX ",
            "   XX  X  XX   ",
            "  X   XXX   X  ",
            " X   X   X   X "
        };
        public static readonly string[] PlayerExplosionFrame2 =
        {
            " X   X   X   X ",
            "  X X X X X X  ",
            "X  X  XXX  X  X",
            " X  XX   XX  X ",
            "  XX       XX  ",
            " X  XX   XX  X ",
            "X  X  XXX  X  X",
            "  X X X X X X  "
        };
    }
}
