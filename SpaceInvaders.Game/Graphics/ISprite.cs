using System.Drawing;

namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Represents a drawable sprite
    /// Abstraction allows different implementations (bitmap, vector, etc.)
    /// </summary>
    public interface ISprite
    {
        int Width { get; }
        int Height { get; }
        void Draw(System.Drawing.Graphics graphics, int x, int y, Color color);
    }
}
