using System.Drawing;

namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Sprite defined by a 2D array of pixels.
    /// Used for authentic pixel art.
    /// </summary>
    public class BitmapSprite : ISprite
    {
        private readonly bool[,] _pixels;

        public int Width { get; }
        public int Height { get; }

        public BitmapSprite(string[] pattern)
        {
            if (pattern == null || pattern.Length == 0)
                throw new ArgumentException("Pattern cannot be null or empty");

            Height = pattern.Length;
            Width = pattern[0].Length;
            _pixels = new bool[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                if (pattern[y].Length != Width)
                    throw new ArgumentException($"Inconsistent row width at row {y}");

                for (int x = 0; x < Width; x++)
                {
                    _pixels[x, y] = pattern[y][x] != ' ';
                }
            }
        }

        public void Draw(System.Drawing.Graphics graphics, int x, int y, System.Drawing.Color color)
        {
            using var brush = new SolidBrush(color);

            for (int py = 0; py < Height; py++)
            {
                for (int px = 0; px < Width; px++)
                {
                    if (_pixels[px, py])
                    {
                        graphics.FillRectangle(brush, x + px, y + py, 1, 1);
                    }
                }
            }
        }
    }
}
