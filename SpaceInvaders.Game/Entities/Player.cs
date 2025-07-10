using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Input;

namespace SpaceInvaders.Game.Entities
{
    public enum PlayerState { Alive, Dying, Respawning, Dead}

    /// <summary>
    /// Represents the player's cannon.
    /// Responsibilities:
    /// - Handle player movement based on input
    /// - Enforce movement boundaries
    /// - Signal when player wants to fire
    /// - Track firing cooldown
    /// - Death animation
    /// - Respawn mechanics
    /// </summary>
    public class Player : Entity
    {
        private readonly IInputHandler _input;
        private readonly float _speed;
        private float _fireCooldownTimer;
        private PlayerState _state;
        private float _stateTimer;

        // Animation
        private int _deathAnimationFrame;
        private float _animationTimer;

        // Constantes for state durations
        private const float DEATH_ANIMATION_DURATION = 1.5f;
        private const float RESPAWN_INVINCIBILITY_DURATION = 2.0f;
        private const float DEATH_FRAME_DURATION = 0.25f;
        private const float BLINK_INTERVAL = 0.1f;

        // Events
        public event EventHandler? FireRequested;
        public event EventHandler? DeathAnimationComplete;

        // Properties
        public bool CanFire => _fireCooldownTimer <= 0 && _state == PlayerState.Alive;
        public float FireCooldownRemaining => Math.Max(0, _fireCooldownTimer);
        public PlayerState State => _state;
        public bool IsVulnerable => _state == PlayerState.Alive;
        public bool ShouldRender
        {
            get
            {
                if (_state == PlayerState.Dying) return true;
                if (_state == PlayerState.Dead) return false;
                if (_state == PlayerState.Respawning)
                {
                    // Blink during invincibility
                    return ((int)(_stateTimer / BLINK_INTERVAL)) % 2 == 0;
                }
                return true;
            }
        }
        public int DeathAnimationFrame => _deathAnimationFrame;

        public Player(Vector2 position, IInputHandler input)
            : base(position, GetPlayerSize())
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _speed = GameConstants.PLAYER_SPEED;
            _fireCooldownTimer = 0f;
            _state = PlayerState.Alive;
            _stateTimer = 0f;
            _deathAnimationFrame = 0;
            _animationTimer = 0f;
        }

        public override void Update(float deltaTime)
        {
            UpdateState(deltaTime);

            if (_state == PlayerState.Alive)
            {
                UpdateCooldown(deltaTime);
                HandleMovement(deltaTime);
                HandleFiring();
            }
        }

        private void UpdateState(float deltaTime)
        {
            _stateTimer -= deltaTime;

            switch (_state)
            {
                case PlayerState.Dying:
                    UpdateDeathAnimation(deltaTime);
                    if (_stateTimer <= 0)
                    {
                        _state = PlayerState.Dead;
                        DeathAnimationComplete?.Invoke(this, EventArgs.Empty);
                    }
                    break;

                case PlayerState.Respawning:
                    if (_stateTimer <= 0)
                        _state = PlayerState.Alive;
                    break;
            }
        }

        private void UpdateDeathAnimation(float deltaTime)
        {
            _animationTimer += deltaTime;
            if (_animationTimer >= DEATH_FRAME_DURATION)
            {
                _animationTimer -= DEATH_FRAME_DURATION;
                _deathAnimationFrame = (_deathAnimationFrame + 1) % 2;
            }
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

        public void Hit()
        {
            if (_state != PlayerState.Alive) return;

            _state = PlayerState.Dying;
            _stateTimer = DEATH_ANIMATION_DURATION;
            _deathAnimationFrame = 0;
            _animationTimer = 0;
        }

        public void Respawn(Vector2 position)
        {
            Position = position;
            _state = PlayerState.Respawning;
            _stateTimer = RESPAWN_INVINCIBILITY_DURATION;
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
    }
}
