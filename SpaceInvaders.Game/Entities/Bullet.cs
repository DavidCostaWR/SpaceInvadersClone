using SpaceInvaders.Game.Domain;

namespace SpaceInvaders.Game.Entities
{
    /// <summary>
    /// Represents a projectile fired by either player or invaders
    /// Moves in a straight line until hitting something or leaving the screen
    /// </summary>
    public class Bullet : Entity
    {
        private readonly float _velocity;
        private readonly BulletType _type;

        public BulletType Type => _type;

        public Bullet(Vector2 position, float velocity, BulletType type) : 
            base(position, GetSizeForType(type))
        {
            _velocity = velocity;
            _type = type;
        }

        public override void Update(float deltaTime)
        {
            // Simple linear movement
            Position += Vector2.Up * _velocity * deltaTime;

            // Deactivate if outside screen bounds
            if (Position.Y <= -Size.Y || Position.Y >= GameConstants.GAME_HEIGHT)
            {
                Destroy();
            }
        }

        private static Vector2 GetSizeForType(BulletType type)
        {
            return type switch
            {
                BulletType.Player => new Vector2(1, 4),
                BulletType.Invader => new Vector2(3, 7),
                _ => throw new ArgumentException($"Unknown bullet type: {type}")
            };
        }
    }

    public enum BulletType
    {
        Player,
        Invader
    }
}
