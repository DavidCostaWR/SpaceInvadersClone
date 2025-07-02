using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Managers;

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
        private GameState _state = GameState.Playing;
        private int _score;
        private int _lives;

        public GameState State => _state;
        public int Score => _score;
        public int Lives => _lives;
        public IEnumerable<Invader> Invaders => _invaderFormation.Invaders;
        public int ActiveInvaders => _invaderFormation.ActiveCount;
        public int CurrentAnimationFrame => _invaderAnimator.CurrentFrame;

        public GameCore()
        {
            _invaderFormation = new InvaderFormation();
            _invaderAnimator = new AnimationController(GameConstants.INVADER_ANIMATION_INTERVAL);

            // Subscribe to game events
            _invaderFormation.InvaderDestroyed += OnInvaderDestroyed;
            _invaderFormation.ReachedBottom += OnInvadersReachedBottom;
        }

        public void Update(float deltaTime)
        {
            if (_state != GameState.Playing) return;

            _invaderFormation.Update(deltaTime);
            _invaderAnimator.Update(deltaTime);

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

        public void Reset()
        {
            _invaderFormation.Reset();
            _invaderAnimator.Reset();
            _score = 0;
            _lives = GameConstants.PLAYER_LIVES;
            _state = GameState.Playing;
        }
    }
}
