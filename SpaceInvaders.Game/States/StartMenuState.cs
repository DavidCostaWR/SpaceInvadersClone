using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Graphics;

namespace SpaceInvaders.Game.States
{
    /// <summary>
    /// The classic Space Invaders start screen showing point values
    /// </summary>
    public class StartMenuState : IGameState
    {
        private readonly Action _onStartGame;
        private float _blinkTimer;
        private bool _showStartText;

        // Animation for demo invaders
        private readonly AnimationController _demoAnimator;

        public StartMenuState(Action onStartGame)
        {
            _onStartGame = onStartGame;
            _demoAnimator = new AnimationController(0.5f);
        }

        public void Enter(StateTransitionContext? context = null)
        {
            _blinkTimer = 0;
            _showStartText = true;
            _demoAnimator.Reset();
        }

        public void Exit() { }

        public StateTransitionRequest? Update(float deltaTime)
        {
            // Blink "PRESS SPACE TO START"
            _blinkTimer += deltaTime;
            if (_blinkTimer >= 0.5f)
            {
                _blinkTimer = 0;
                _showStartText = !_showStartText;
            }

            _demoAnimator.Update(deltaTime);

            return null;
        }

        public void Draw(Renderer renderer)
        {
            renderer.Clear(Color.Black);

            // Title
            renderer.DrawTextCentered("SPACE", 30, Color.White, 3);
            renderer.DrawTextCentered("INVADERS", 50, Color.White, 3);

            // Score advance table
            renderer.DrawTextCentered("SCORE ADVANCE TABLE", 90, Color.White, 1);

            // Draw point values with invader sprites
            DrawScoreTable(renderer);

            // Start prompt
            if (_showStartText)
            {
                renderer.DrawTextCentered("PRESS SPACE TO START", 200, Color.Cyan, 1);
            }

            // Credits
            renderer.DrawTextCentered("INSERT COIN", 230, Color.Red, 1);
        }

        private void DrawScoreTable(Renderer renderer)
        {
            var tableWidth = 100f; // Approximate width of the table
            var tableX = (GameConstants.GAME_WIDTH - tableWidth) / 2;

            DrawInvaderPointValue(renderer, InvaderType.Small, new Vector2(tableX, 110), "30");
            DrawInvaderPointValue(renderer, InvaderType.Medium, new Vector2(tableX, 130), "20");
            DrawInvaderPointValue(renderer, InvaderType.Large, new Vector2(tableX, 150), "10");
            DrawUFOPointValue(renderer, new Vector2(tableX, 170), "???");
        }

        private void DrawInvaderPointValue(Renderer renderer, InvaderType type, Vector2 position, string points)
        {
            var sprite = SpriteRepository.Instance.GetInvaderSprite(type, _demoAnimator.CurrentFrame);
            renderer.DrawSprite(sprite, position, Color.White);

            var textX = position.X + 20;
            DrawText(renderer, $"= {points} POINTS", new Vector2(textX, position.Y), Color.White, 1);
        }

        private void DrawUFOPointValue(Renderer renderer, Vector2 position, string points)
        {
            var sprite = SpriteRepository.Instance.GetSprite(SpriteKey.UFO);
            renderer.DrawSprite(sprite, position, Color.White);

            var textX = position.X + 20;
            DrawText(renderer, $"= {points} POINTS", new Vector2(textX, position.Y), Color.White, 1);
        }

        private void DrawText(Renderer renderer, string text, Vector2 position, Color color, int scale)
        {
            renderer.DrawText(text, position, color, scale);
        }

        private void DrawCharacter(Renderer renderer, char c, Vector2 position, Color color, float scale)
        {
            // Placeholder for character rendering
            // To implement bitmap font data
            var rect = new Domain.Rectangle(position.X, position.Y, 5 * scale, 7 * scale);
            // renderer.DrawRectangle(rect, color); // To implement this
        }

        public StateTransitionRequest? HandleInput(Keys key, bool isKeyDown)
        {
            if (!isKeyDown && key == Keys.Space)
            {
                _onStartGame();
                return new StateTransitionRequest
                {
                    Transition = StateTransition.ToPlaying,
                    ResetGame = true
                };
            }
            return null;
        }
    }
}
