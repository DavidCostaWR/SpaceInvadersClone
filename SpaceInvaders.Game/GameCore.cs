using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Managers;
using SpaceInvaders.Game.Input;

namespace SpaceInvaders.Game
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public class GameCore
    {
        private readonly InvaderFormation _invaderFormation;
        private readonly AnimationController _invaderAnimator;
        private readonly Player _player;
        private GameState _state = GameState.Playing;
        private int _score;
        private int _lives;

        public GameState State => _state;
        public int Score => _score;
        public int Lives => _lives;
        public IEnumerable<Invader> Invaders => _invaderFormation.Invaders;
        public int ActiveInvaders => _invaderFormation.ActiveCount;
        public int CurrentAnimationFrame => _invaderAnimator.CurrentFrame;
        public Player Player => _player;

        public GameCore(IInputHandler inputHandler)
        {
            if (inputHandler == null)
                throw new ArgumentNullException(nameof(inputHandler));

            // Initialize game objects
            _invaderFormation = new InvaderFormation();
            _invaderAnimator = new AnimationController(GameConstants.INVADER_ANIMATION_INTERVAL);
            _player = new Player(GameConstants.PlayerStartPosition, inputHandler);
            _lives = GameConstants.PLAYER_LIVES;
            _score = 0;

            // Subscribe to game events
            _invaderFormation.InvaderDestroyed += OnInvaderDestroyed;
            _invaderFormation.ReachedBottom += OnInvadersReachedBottom;
            _player.FireRequested += OnPlayerFireRequested;
        }

        public void Update(float deltaTime)
        {
            if (_state != GameState.Playing) return;

            _invaderFormation.Update(deltaTime);
            _invaderAnimator.Update(deltaTime);
            _player.Update(deltaTime);

            // Check victory condition
            if (_invaderFormation.ActiveCount == 0)
            {
                _state = GameState.Victory;
            }
        }

        private void OnInvaderDestroyed(object? sender, Invader invader)
        {
            _score += invader.PointValue;
        }

        private void OnInvadersReachedBottom(object? sender, EventArgs e)
        {
            _state = GameState.GameOver;
        }

        private void OnPlayerFireRequested(object? sender, EventArgs e)
        {
            // TODO: Create bullet when projectile system is implemented
            // For now, just log it
            Console.WriteLine($"Fire requested at position: {_player.Position.X + _player.Size.X / 2}");
        }

        public void Reset()
        {
            _invaderFormation.Reset();
            _invaderAnimator.Reset();
            _player.Position = GameConstants.PlayerStartPosition;

            _score = 0;
            _lives = GameConstants.PLAYER_LIVES;
            _state = GameState.Playing;
        }
    }
}
