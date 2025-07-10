using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.Entities
{
    /// <summary>
    /// The mystery UFO that occasionally flies across the top of the screen.
    /// Worth variable points based on seemingly random factors.
    /// </summary>
    public class UFO : Entity
    {
        private readonly float _speed;
        private readonly int _direction;
        private readonly int _pointValue;

        public int PointValue => _pointValue;
        public bool IsMovingRight => _direction > 0;

        public UFO(bool movingRight, int pointValue)
            : base(CreateStartPosition(movingRight), GetUFOSize())
        {
            _direction = movingRight ? 1 : -1;
            _speed = GameConstants.UFO_SPEED;
            _pointValue = pointValue;
        }

        public override void Update(float deltaTime)
        {
            // Move horizontally
            Position += Vector2.Right * _direction * _speed * deltaTime;

            // Deactivate when off screen
            if (_direction > 0 && Position.X > GameConstants.GAME_WIDTH)
                Destroy();
            else if (_direction < 0 && Position.X + Size.X < 0)
                Destroy();
        }

        private static Vector2 CreateStartPosition(bool movingRight)
        {
            // Start just off screen
            float x = movingRight ? -GetUFOSize().X : GameConstants.GAME_WIDTH;
            return new Vector2(x, GameConstants.UFO_Y_POSITION);
        }

        private static Vector2 GetUFOSize()
        {
            try
            {
                var sprite = SpriteRepository.Instance.GetSprite(SpriteKey.UFO);
                return new Vector2(sprite.Width, sprite.Height);
            }
            catch
            {
                return new Vector2(16, 7);  // Default UFO size
            }
        }
    }
}
