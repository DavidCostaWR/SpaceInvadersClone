using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Entities;
using Timer = System.Windows.Forms.Timer;
using SpaceInvaders.Game.Managers;

namespace SpaceInvaders.Game
{
    public partial class GameForm : Form
    {
        private readonly Renderer _renderer;
        private readonly Timer _gameTimer;
        private readonly InvaderFormation _invaderFormation;
        private DateTime _lastUpdate = DateTime.Now;
        private int _animationFrame = 0;
        private float _animationTimer = 0f;

        public GameForm()
        {
            InitializeComponent();
            SetupWindow();

            _renderer = new Renderer();
            _invaderFormation = new InvaderFormation();

            // Subscribe to game events
            _invaderFormation.ReachedBottom += OnInvadersReachedBottom;
            _invaderFormation.InvaderDestroyed += OnInvaderDestroyed;

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
            DoubleBuffered = true; // Prevents flickering
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

            // Update game objects
            _invaderFormation.Update(deltaTime);

            // Update animation
            _animationTimer += deltaTime;
            if (_animationTimer >= GameConstants.INVADER_ANIMATION_INTERVAL)
            {
                _animationTimer -= GameConstants.INVADER_ANIMATION_INTERVAL;
                _animationFrame = (_animationFrame + 1) % 2;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Clear to black
            _renderer.Clear(Color.Black);

            // Draw all invaders
            foreach (var invader in _invaderFormation.Invaders)
            {
                var sprite = SpriteRepository.Instance.GetInvaderSprite(
                    invader.Type,
                    _animationFrame
                );
                _renderer.DrawSprite(sprite, invader.Position, Color.White);
            }

            // Present to screen
            var targetRect = new Domain.Rectangle(0, 0, ClientSize.Width, ClientSize.Height);
            _renderer.Present(e.Graphics, targetRect);
        }

        private void OnInvadersReachedBottom(object? sender, EventArgs e)
        {
            // Game over logic will go here
            _gameTimer.Stop();
            MessageBox.Show("Game Over! Invaders reached Earth!", "Game Over");
        }

        private void OnInvaderDestroyed(object? sender, Invader invader)
        {
            // Score logic will go here
            // For now, just track it
            Console.WriteLine($"Invader destroyed! Points: {invader.PointValue}");
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            base.OnFormClosed(e);
            _gameTimer.Stop();
            _renderer.Dispose();
        }
    }
}
