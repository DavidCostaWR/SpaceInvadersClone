using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Input;
using SpaceInvaders.Game.States;
using Timer = System.Windows.Forms.Timer;

namespace SpaceInvaders.Game
{
    public partial class GameForm : Form
    {
        private readonly Renderer _renderer;
        private readonly Timer _gameTimer;
        private readonly KeyboardInputHandler _inputHandler;
        private readonly GameCore _game;
        private readonly GameStateManager _stateManager;

        private DateTime _lastUpdate = DateTime.Now;

        public GameForm()
        {
            InitializeComponent();
            SetupWindow();

            _renderer = new Renderer();
            _inputHandler = new KeyboardInputHandler();
            _game = new GameCore(_inputHandler);
            _stateManager = new GameStateManager(_renderer, _game);

            SetupStates();

            // Enable key events
            KeyPreview = true;  // Form receives key events before controls

            // Setup game timer
            _gameTimer = new Timer();
            _gameTimer.Interval = GameConstants.FRAME_TIME_MS;
            _gameTimer.Tick += GameTimer_Tick;
            _gameTimer.Start();
        }

        private void SetupStates()
        {
            var startMenuState = new StartMenuState(() => { /* Game start logic */ });
            var playingState = new PlayingState(_game, _inputHandler);
            var pauseMenuState = new PauseMenuState();
            var gameOverState = new GameOverState(() => _game.Score);
            var victoryState = new VictoryState(() => _game.Score);

            _stateManager.RegisterState(startMenuState);
            _stateManager.RegisterState(playingState);
            _stateManager.RegisterState(pauseMenuState);
            _stateManager.RegisterState(gameOverState);
            _stateManager.RegisterState(victoryState);

            _stateManager.StartWithState<StartMenuState>();
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
            _stateManager.HandleInput(e.KeyCode, true);

            // Prevent beep sound on spacebar
            if (e.KeyCode == Keys.Space)
                e.Handled = true;
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            _inputHandler.KeyUp(e.KeyCode);
            _stateManager.HandleInput(e.KeyCode, false);
        }

        private void Update()
        {
            var now = DateTime.Now;
            var deltaTime = (float)(now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;

            _stateManager.Update(deltaTime);
            _inputHandler.Update();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _renderer.Clear(Color.Black);

            _stateManager.Draw();

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
