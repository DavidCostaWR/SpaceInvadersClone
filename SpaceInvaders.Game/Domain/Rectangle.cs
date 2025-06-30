namespace SpaceInvaders.Game.Domain
{
    /// <summary>
    /// Axis-aligned bounding box for collision detection
    /// </summary>
    public readonly struct Rectangle : IEquatable<Rectangle>
    {
        public float X { get; }
        public float Y { get; }
        public float Width { get; }
        public float Height { get; }

        public float Left => X;
        public float Top => Y;
        public float Right => X + Width;
        public float Bottom => Y + Height;

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Rectangle(Vector2 position, Vector2 size)
            : this(position.X, position.Y, size.X, size.Y) { }

        public bool Intersects(Rectangle other) 
            => Left < other.Right &&
               Right > other.Left &&
               Top < other.Bottom &&
               Bottom > other.Top;

        public bool Contains(Vector2 point) 
            => point.X >= Left &&
               point.X <= Right &&
               point.Y >= Top &&
               point.Y <= Bottom;

        public Rectangle Offset(Vector2 offset) 
            => new(X + offset.X, Y + offset.Y, Width, Height);

        public bool Equals(Rectangle other) 
            => X.Equals(other.X) &&
               Y.Equals(other.Y) &&
               Width.Equals(other.Width) &&
               Height.Equals(other.Height);

        public override bool Equals(object? obj) 
            => obj is Rectangle other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(X, Y, Width, Height);

        public override string ToString()
            => $"[{X:F2}, {Y:F2}, {Width:F2}, {Height:F2}]";
    }
}
