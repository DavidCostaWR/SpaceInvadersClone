using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.Entities
{
    /// <summary>
    /// Represents a single space invader.
    /// Moves as part of the formation
    /// </summary>
    public class Invader : Entity
    {
        private readonly int _pointValue;

        public int PointValue => _pointValue;
        public InvaderType Type { get; }

        public Invader(Vector2 position, InvaderType type)
            : base(position, GetSizeForType(type))
        {
            Type = type;
            _pointValue = GetPointValueForType(type);
        }

        public override void Update(float deltaTime)
        {
            // Invaders don't update themselves directly,
            // they are moved by the formation
        }

        public void Move(Vector2 offset)
        {
            Position += offset;
        }

        public static Vector2 GetSizeForType(InvaderType type)
        {
            try
            {
                var sprite = SpriteRepository.Instance.GetInvaderSprite(type, 0);
                return new Vector2(sprite.Width, sprite.Height);
            }
            catch
            {
                return type switch
                {
                    InvaderType.Small => new Vector2(8, 8),
                    InvaderType.Medium => new Vector2(11, 8),
                    InvaderType.Large => new Vector2(12, 8),
                    _ => throw new ArgumentException($"Unknown invader type: {type}")
                };
            }
        }

        private static int GetPointValueForType(InvaderType type)
        {
            return type switch
            {
                InvaderType.Small => GameConstants.Points.SmallInvader,
                InvaderType.Medium => GameConstants.Points.MediumInvader,
                InvaderType.Large => GameConstants.Points.LargeInvader,
                _ => throw new ArgumentException($"Unknown invader type: {type}")
            };
        }
    }
    public enum InvaderType
    {
        Small,
        Medium,
        Large
    }
}
