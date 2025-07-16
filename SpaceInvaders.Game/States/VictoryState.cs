using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using Rectangle = SpaceInvaders.Game.Domain.Rectangle;

namespace SpaceInvaders.Game.States
{
    /// <summary>
    /// Victory screen - shown when all invaders are destroyed
    /// </summary>
    public class VictoryState : IGameState
    {
        private readonly Func<int> _getScore;
        private int _finalScore;
        private float _timer;
        private readonly Random _random = new();

        public VictoryState(Func<int> getScore)
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

            // Victory text with color cycling
            var hue = (int)(_timer * 60) % 360;
            var victoryColor = ColorFromHSV(hue, 1, 1);
            renderer.DrawTextCentered("VICTORY!", 50, victoryColor, 2);

            // Score
            renderer.DrawTextCentered($"FINAL SCORE: {_finalScore}", 100, Color.White, 1);

            // Earth saved message
            renderer.DrawTextCentered("THE EARTH IS SAFE", 130, Color.Cyan, 1);
            renderer.DrawTextCentered("YOU ARE A HERO", 145, Color.Cyan, 1);

            // Draw fireworks/start
            DrawCelebration(renderer);

            // Continue prompt
            if ((int)(_timer * 2) % 2 == 0)
            {
                renderer.DrawTextCentered("PRESS SPACE TO CONTINUE", 200, Color.White, 1);
            }
        }

        private void DrawCelebration(Renderer renderer)
        {
            // Simple star field effect
            var starCount = 20;
            for (int i = 0; i < starCount; i++)
            {
                var x = (float)(Math.Sin(i * 1.5f + _timer) * 50 + GameConstants.GAME_WIDTH / 2);
                var y = (float)(Math.Cos(i * 1.5f + _timer * 0.8f) * 30 + 100);
                var size = (float)(Math.Sin(_timer * 3 + i) * .5 + 1.5);

                // Draw star as pixels
                var starColor = Color.FromArgb(
                    255,
                    255,
                    255,
                    (int)(128 + 127 * Math.Sin(_timer * 5 + i))
                );

                // Draw a simple star/sparkle
                renderer.FillRectangle(new Rectangle(x - size/2, y, size, 1), starColor);
                renderer.FillRectangle(new Rectangle(x, y - size/2, 1, size), starColor);
            }
        }

        private Color ColorFromHSV(double hue, double saturation, double value)
        {
            int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
            double f = hue / 60 - Math.Floor(hue / 60);

            value = value * 255;
            int v = Convert.ToInt32(value);
            int p = Convert.ToInt32(value * (1 - saturation));
            int q = Convert.ToInt32(value * (1 - f * saturation));
            int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

            return hi switch
            {
                0 => Color.FromArgb(255, v, t, p),
                1 => Color.FromArgb(255, q, v, p),
                2 => Color.FromArgb(255, p, v, t),
                3 => Color.FromArgb(255, p, q, v),
                4 => Color.FromArgb(255, t, p, v),
                _ => Color.FromArgb(255, v, p, q),
            };
        }

        public StateTransitionRequest? HandleInput(Keys key, bool isKeyDown)
        {
            if (!isKeyDown && key == Keys.Space && _timer > 2.0f)
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
