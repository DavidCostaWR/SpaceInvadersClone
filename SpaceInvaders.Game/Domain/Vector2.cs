
namespace SpaceInvaders.Game.Domain
{
    /// <summary>
    /// Represents a 2D position or direction in the game world.
    /// Immutable to prevent accidental modifications.
    /// </summary>
    public readonly struct Vector2 : IEquatable<Vector2>
    {
        public float X { get; }
        public float Y { get; }

        public Vector2(float x, float y)
        {
            X = x;
            Y = y;
        }

        // Factory methods for common vectors
        public static Vector2 Zero => new Vector2(0, 0);
        public static Vector2 One => new Vector2(1, 1);
        public static Vector2 Up => new Vector2(0, -1);
        public static Vector2 Down => new Vector2(0, 1);
        public static Vector2 Left => new Vector2(-1, 0);
        public static Vector2 Right => new Vector2(1, 0);

        // Operator overloads for intuitive vector arithmetic
        public static Vector2 operator +(Vector2 a, Vector2 b) 
            => new Vector2(a.X + b.X, a.Y + b.Y);

        public static Vector2 operator -(Vector2 a, Vector2 b) 
            => new Vector2(a.X - b.X, a.Y - b.Y);
        
        public static Vector2 operator *(Vector2 v, float scalar) 
            => new Vector2(v.X * scalar, v.Y * scalar);
        
        public static Vector2 operator *(float scalar, Vector2 v)
            => v * scalar;
        
        public static Vector2 operator /(Vector2 v, float scalar) 
            => new Vector2(v.X / scalar, v.Y / scalar);
        
        // Implement IEquatable for performance
        public bool Equals(Vector2 other) 
            => X == other.X && Y == other.Y;
        
        public override bool Equals(object? obj) 
            => obj is Vector2 other && Equals(other);
        
        public override int GetHashCode() 
            => HashCode.Combine(X, Y);
        
        public override string ToString() 
            => $"({X}, {Y})";
    }
}
