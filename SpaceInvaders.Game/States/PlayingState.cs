using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Input;

namespace SpaceInvaders.Game.States
{
    /// <summary>
    /// The main game state
    /// </summary>
    public class PlayingState : IGameState
    {
        private readonly GameCore _gameCore;
        private readonly IInputHandler _inputHandler;
        private bool _isPaused;
        private bool _justEntered;

        public PlayingState(GameCore gameCore, IInputHandler inputHandler)
        {
            _gameCore = gameCore;
            _inputHandler = inputHandler;
        }

        public void Enter(StateTransitionContext? context = null)
        {
            _isPaused = false;
            _justEntered = true;

            if (context?.ResetGame == true)
            {
                _gameCore.Reset();
            }
        }

        public void Exit() { }

        public StateTransitionRequest Update(float deltaTime)
        {
            if (_isPaused) return null;

            _gameCore.Update(deltaTime);

            if (_justEntered)
            {
                _justEntered = false;
                return null;
            }

            // Check for game over or victory conditions
            return _gameCore.State switch
            {
                GameState.GameOver => new StateTransitionRequest
                {
                    Transition = StateTransition.ToGameOver
                },
                GameState.Victory => new StateTransitionRequest
                {
                    Transition = StateTransition.ToVictory
                },
                _ => null
            };
        }

        public void Draw(Renderer renderer)
        {
            // The actual game drawing is handled by GameForm
            // This is just for state management
        }

        public StateTransitionRequest HandleInput(Keys key, bool isKeyDown)
        {
            // Handle pause
            if (!isKeyDown && key == Keys.Escape)
            {
                return new StateTransitionRequest
                {
                    Transition = StateTransition.ToPause
                };
            }
            return null;
        }
    }
}
