using SpaceInvaders.Game.Domain;
using Rectangle = SpaceInvaders.Game.Domain.Rectangle;

namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Handles all drawing operations.
    /// Maintains the game's pixel buffer and scaling.
    /// </summary>
    public class Renderer : IDisposable
    {
        private readonly Bitmap _gameBuffer;
        private readonly System.Drawing.Graphics _gameGraphics;
        private readonly BitmapFont _font;
        private bool _disposed;

        public Renderer()
        {
            // Create buffer at original game resolution
            _gameBuffer = new Bitmap(GameConstants.GAME_WIDTH, GameConstants.GAME_HEIGHT);
            _gameGraphics = System.Drawing.Graphics.FromImage(_gameBuffer);
            _font = new BitmapFont();

            // Pixel-perfect rendering settings
            _gameGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            _gameGraphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;
            _gameGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
        }

        public void Clear(Color color)
        {
            _gameGraphics.Clear(color);
        }

        public void DrawSprite(ISprite sprite, Vector2 position, Color color)
        {
            sprite.Draw(_gameGraphics, (int)position.X, (int)position.Y, color);
        }

        public void DrawText(string text, Vector2 position, Color color, float scale = 1.0f)
        {
            _font.DrawText(this, text, position, color, scale);
        }

        public void DrawTextCentered(string text, float y, Color color, float scale = 1.0f)
        {
            _font.DrawTextCentered(this, text, y, color, scale);
        }

        public void FillRectangle(Rectangle rect, Color color)
        {
            using var brush = new SolidBrush(color);
            _gameGraphics.FillRectangle(brush, rect.ToDrawingRectangle());
        }

        public void DrawRectangle(Rectangle rect, Color color)
        {
            using var pen = new Pen(color, 1);
            _gameGraphics.DrawRectangle(pen, rect.ToDrawingRectangle());
        }

        public void DrawPixel(int x, int y, Color color)
        {
            if (x >= 0 && x < GameConstants.GAME_WIDTH &&
                y >= 0 && y < GameConstants.GAME_HEIGHT)
            {
                using var brush = new SolidBrush(color);
                _gameGraphics.FillRectangle(brush, x, y, 1, 1);
            }
        }

        public void Present(System.Drawing.Graphics targetGraphics, Rectangle targetBounds)
        {
            targetGraphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            targetGraphics.DrawImage(_gameBuffer, targetBounds.ToDrawingRectangle());
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _gameGraphics?.Dispose();
                _gameBuffer?.Dispose();
                _disposed = true;
            }
        }
    }
}
