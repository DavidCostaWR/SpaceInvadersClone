using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Input;

namespace SpaceInvaders.Game.Entities
{
    /// <summary>
    /// Represents the player's cannon.
    /// Responsibilities:
    /// - Handle player movement based on input
    /// - Enforce movement boundaries
    /// - Signal when player wants to fire
    /// - Track firing cooldown
    /// </summary>
    public class Player : Entity
    {
        private readonly IInputHandler _input;
        private readonly float _speed;
        private float _fireCooldownTimer;

        public event EventHandler? FireRequested;

        public bool CanFire => _fireCooldownTimer <= 0;
        public float FireCooldownRemaining => Math.Max(0, _fireCooldownTimer);

        public Player(Vector2 position, IInputHandler input)
            : base(position, GetPlayerSize())
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _speed = GameConstants.PLAYER_SPEED;
            _fireCooldownTimer = 0f;
        }

        private static Vector2 GetPlayerSize()
        {
            try
            {
                var sprite = SpriteRepository.Instance.GetSprite(SpriteKey.Player);
                return new Vector2(sprite.Width, sprite.Height);
            }
            catch
            {
                return new Vector2(13, 8); // Default size if sprite not found
            }
        }

        public override void Update(float deltaTime)
        {
            UpdateCooldown(deltaTime);
            HandleMovement(deltaTime);
            HandleFiring();
        }

        private void UpdateCooldown(float deltaTime)
        {
            if (_fireCooldownTimer > 0)
                _fireCooldownTimer -= deltaTime;
        }

        private void HandleMovement(float deltaTime)
        {
            float horizontalInput = 0f;

            if (_input.IsLeftPressed)
                horizontalInput -= 1f;

            if (_input.IsRightPressed)
                horizontalInput += 1f;

            if (horizontalInput == 0)
                return;

            var movement = Vector2.Right * horizontalInput * _speed * deltaTime;
            var newX = Position.X + movement.X;

            newX = Math.Clamp(newX, 0, GameConstants.GAME_WIDTH - Size.X);

            Position = new Vector2(newX, Position.Y);
        }

        private void HandleFiring()
        {
            if (_input.WasFireJustPressed && CanFire)
            {
                FireRequested?.Invoke(this, EventArgs.Empty);
                _fireCooldownTimer = GameConstants.PLAYER_FIRE_COOLDOWN;
            }
        }
    }
}
