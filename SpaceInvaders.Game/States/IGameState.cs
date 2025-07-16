using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.States
{
    /// <summary>
    /// Interface for game states
    /// </summary>
    public interface IGameState
    {
        void Enter(StateTransitionContext? context = null);
        void Exit();
        StateTransitionRequest? Update(float deltaTime);
        void Draw(Renderer renderer);
        StateTransitionRequest? HandleInput(Keys key, bool isKeyDown);
    }

    public class StateTransitionRequest
    {
        public StateTransition Transition { get; set; }
        public bool ResetGame { get; set; }
    }
    public class StateTransitionContext
    {
        public bool ResetGame { get; set; }
    }

    public enum StateTransition
    {
        None,
        ToMenu,
        ToPlaying,
        ToPause,
        ToGameOver,
        ToVictory
    }
}
