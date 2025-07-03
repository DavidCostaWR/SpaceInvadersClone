using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Input;
using Timer = System.Windows.Forms.Timer;

namespace SpaceInvaders.Game
{
    public partial class GameForm : Form
    {
        private readonly Renderer _renderer;
        private readonly Timer _gameTimer;
        private readonly KeyboardInputHandler _inputHandler;
        private readonly GameCore _game;
        private DateTime _lastUpdate = DateTime.Now;

        public GameForm()
        {
            InitializeComponent();
            SetupWindow();

            _renderer = new Renderer();
            _inputHandler = new KeyboardInputHandler();

            _game = new GameCore(_inputHandler);

            // Enable key events
            KeyPreview = true;  // Form receives key events before controls

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

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            _inputHandler.KeyDown(e.KeyCode);

            // Prevent beep sound on spacebar
            if (e.KeyCode == Keys.Space)
            {
                e.Handled = true;
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            _inputHandler.KeyUp(e.KeyCode);
        }

        private void Update()
        {
            var now = DateTime.Now;
            var deltaTime = (float)(now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;

            _inputHandler.Update();

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

            // Draw player
            var playerSprite = SpriteRepository.Instance.GetSprite(SpriteKey.Player);
            _renderer.DrawSprite(playerSprite, _game.Player.Position, Color.White);

            // Draw HUD (temporary text rendering)
            DrawHUD(e.Graphics);

            // Present to screen
            var targetRect = new Domain.Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            _renderer.Present(e.Graphics, targetRect);
        }

        private void DrawHUD(System.Drawing.Graphics graphics)
        {
            var font = new Font("Consolas", 12);
            var brush = new SolidBrush(Color.White);

            // Score at top left
            graphics.DrawString($"SCORE: {_game.Score:D5}", font, brush, 10, 10);

            // Lives at top right
            var livesText = $"LIVES: {_game.Lives}";
            var textSize = graphics.MeasureString(livesText, font);
            graphics.DrawString(livesText, font, brush, ClientSize.Width - textSize.Width - 10, 10);
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _gameTimer.Stop();
            _renderer.Dispose();
        }
    }
}
