using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.States
{
    /// <summary>
    /// Game over screen
    /// </summary>
    public class GameOverState : IGameState
    {
        private readonly Func<int> _getScore;
        private int _finalScore;
        private float _timer;

        public GameOverState(Func<int> getScore)
        {
            _getScore = getScore;
        }

        public void Enter(StateTransitionContext? context = null)
        {
            _timer = 0;
            _finalScore = _getScore();
        }

        public void Exit() { }

        public StateTransitionRequest? Update(float deltaTime)
        {
            _timer += deltaTime;
            return null;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Clear(Color.Black);

            // Game Over text with animation
            var gameOverY = 50f + (float)Math.Sin(_timer * 2) * 5;
            renderer.DrawTextCentered("GAME OVER", gameOverY, Color.Red, 2);

            // Final score
            renderer.DrawTextCentered($"FINAL SCORE: {_finalScore}", 100, Color.White, 1);

            // Earth invaded message
            renderer.DrawTextCentered("THE INVADERS HAVE", 130, Color.Green, 1);
            renderer.DrawTextCentered("REACHED EARTH", 145, Color.Green, 1);

            // Continue prompt
            if ((int)(_timer * 2) % 2 == 0)
            {
                renderer.DrawTextCentered("PRESS SPACE TO CONTINUE", 200, Color.Cyan, 1);
            }
        }

        private void DrawText(Renderer renderer, string text, Vector2 position, Color color, int scale)
        {
            renderer.DrawText(text, position, color, scale);
        }

        public StateTransitionRequest? HandleInput(Keys key, bool isKeyDown)
        {
            if (!isKeyDown && key == Keys.Space && _timer > 2.0f)   // Prevent accidental skip
            {
                return new StateTransitionRequest
                {
                    Transition = StateTransition.ToMenu
                };
            }
            return null;
        }
    }
}
