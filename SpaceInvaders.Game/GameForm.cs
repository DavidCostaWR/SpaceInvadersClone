using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using System.ComponentModel.DataAnnotations;
using Timer = System.Windows.Forms.Timer;

namespace SpaceInvaders.Game
{
    public partial class GameForm : Form
    {
        private readonly Renderer _renderer;
        private readonly Timer _gameTimer;
        private readonly GameCore _game;
        private DateTime _lastUpdate = DateTime.Now;

        public GameForm()
        {
            InitializeComponent();
            SetupWindow();

            _renderer = new Renderer();
            _game = new GameCore();

            // Setup game timer
            _gameTimer = new Timer();
            _gameTimer.Interval = GameConstants.FRAME_TIME_MS;
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();
        }

        private void SetupWindow()
        {
            Text = "Space Invaders";
            BackColor = Color.Black;
            ClientSize = new Size(GameConstants.DISPLAY_WIDTH, GameConstants.DISPLAY_HEIGHT);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;
        }

        private void GameTimer_Tick(object? sender, EventArgs e)
        {
            Update();
            Invalidate(); // Triggers a repaint
        }

        private void Update()
        {
            var now = DateTime.Now;
            var deltaTime = (float)(now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;

            _game.Update(deltaTime);

            switch (_game.State)
            {
                case GameState.Playing:
                    break;
                case GameState.GameOver:
                    _gameTimer.Stop();
                    MessageBox.Show("Game Over! Invaders reached Earth!", "Game Over");
                    break;
                case GameState.Victory:
                    _gameTimer.Stop();
                    MessageBox.Show($"Victory! Score: {_game.Score}", "Victory");
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Clear to black
            _renderer.Clear(Color.Black);

            // Draw all invaders
            foreach (var invader in _game.Invaders)
            {
                var sprite = SpriteRepository.Instance.GetInvaderSprite(
                    invader.Type,
                    _game.CurrentAnimationFrame
                );
                _renderer.DrawSprite(sprite, invader.Position, Color.White);
            }

            // Draw score and lives (teporary text rendering)
            using var font = new Font("Arial", 12);
            using var brush = new SolidBrush(Color.White);
            e.Graphics.DrawString($"Score: {_game.Score}", font, brush, 10, 10);
            e.Graphics.DrawString($"Lives: {_game.Lives}", font, brush, 10, 30);

            // Present to screen
            var targetRect = new Domain.Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            _renderer.Present(e.Graphics, targetRect);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _gameTimer.Stop();
            _renderer.Dispose();
        }
    }
}
