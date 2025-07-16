using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.States
{
    /// <summary>
    /// Pause menu overlay
    /// </summary>
    public class PauseMenuState : IGameState
    {
        private int _selectedOption;
        private readonly string[] _options = { "RESUME", "RESTART", "QUIT TO MENU" };

        public PauseMenuState()
        {
        }

        public void Enter(StateTransitionContext? context = null)
        {
            _selectedOption = 0;
        }

        public void Exit() { }

        public StateTransitionRequest? Update(float deltaTime) { return null; }

        public void Draw(Renderer renderer)
        {
            // Draw the game in backgoround (frozen)
            // This would be handled by preserving the last frame

            // Draw semi-transparent overlay
            DrawOverlay(renderer);

            // Draw menu
            renderer.DrawTextCentered("PAUSED", 50, Color.Yellow, 2);

            var startY = 100f;
            for (int i = 0; i< _options.Length; i++)
            {
                var color = i == _selectedOption ? Color.Cyan : Color.White;
                var prefix = i == _selectedOption ? "> " : "  ";    
                var y = startY + (i * 25);

                renderer.DrawTextCentered(prefix + _options[i], y, color, 1);
            }

            // Instructions at bottom
            renderer.DrawTextCentered("USE ARROW KEYS TO SELECT", 200, Color.Yellow, 1);
            renderer.DrawTextCentered("SPACE TO CONFIRM", 215, Color.Yellow, 1);
        }

        private void DrawOverlay(Renderer renderer)
        {
            // Draw semi-transparent black overlay
            // To implement a FillRectangle method in Renderer
            var rect = new Domain.Rectangle(0, 0, GameConstants.GAME_WIDTH, GameConstants.GAME_HEIGHT);
        }

        private void DrawText(Renderer renderer, string text, Vector2 position, Color color, int scale)
        {
            renderer.DrawText(text, position, color, scale);
        }

        public StateTransitionRequest? HandleInput(Keys key, bool isKeyDown)
        {
            if (isKeyDown) return null;

            switch (key)
            {
                case Keys.Up:
                    _selectedOption = (_selectedOption - 1 + _options.Length) % _options.Length;
                    break;
                case Keys.Down:
                    _selectedOption = (_selectedOption + 1) % _options.Length;
                    break;
                case Keys.Space:
                case Keys.Enter:
                    return ExecuteOption();
                case Keys.Escape:
                    return new StateTransitionRequest
                    {
                        Transition = StateTransition.ToPlaying
                    };
            }
            return null;
        }

        private StateTransitionRequest? ExecuteOption()
        {
            return _selectedOption switch
            {
                0 => new StateTransitionRequest
                {
                    Transition = StateTransition.ToPlaying
                },
                1 => new StateTransitionRequest
                {
                    Transition = StateTransition.ToPlaying,
                    ResetGame = true
                },
                2 => new StateTransitionRequest
                {
                    Transition = StateTransition.ToMenu
                },
                _ => null
            };
        }
    }
}
