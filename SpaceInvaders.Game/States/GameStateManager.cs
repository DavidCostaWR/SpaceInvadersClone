using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Managers;
using Rectangle = SpaceInvaders.Game.Domain.Rectangle;

namespace SpaceInvaders.Game.States
{
    /// <summary>
    /// Manages game state transitions and updates.
    /// </summary>
    public class GameStateManager
    {
        private readonly Dictionary<Type, IGameState> _states;
        private readonly Renderer _renderer;
        private readonly GameCore _gameCore;
        private IGameState? _currentState;

        private readonly Dictionary<StateTransition, Type> _transitionMap = new()
        {
            { StateTransition.ToMenu, typeof(StartMenuState) },
            { StateTransition.ToPlaying, typeof(PlayingState) },
            { StateTransition.ToPause, typeof(PauseMenuState) },
            { StateTransition.ToGameOver, typeof(GameOverState) },
            { StateTransition.ToVictory, typeof(VictoryState) }
        };

        public IGameState? CurrentState => _currentState;

        public GameStateManager(Renderer renderer, GameCore gameCore)
        {
            _states = new Dictionary<Type, IGameState>();
            _renderer = renderer;
            _gameCore = gameCore;
        }

        public void RegisterState<T>(T state) where T : IGameState
        {
            _states[typeof(T)] = state;
        }

        public void Update(float deltaTime)
        {
            var request = _currentState?.Update(deltaTime);
            HandleTransitionRequest(request);
        }

        public void Draw()
        {
            _currentState?.Draw(_renderer);

            // Handle game drawing for playing states
            if (_currentState is PlayingState or PauseMenuState)
            {
                DrawGameElements();

                if (_currentState is PlayingState)
                {
                    DrawHUD();
                }

                if (_currentState is PauseMenuState)
                {
                    DrawPauseOverlay();
                }
            }
        }

        private void DrawGameElements()
        {
            if (IsInWaveTransition())
            {
                DrawWaveTransition();
                return;
            }

            foreach (var invader in _gameCore.Invaders)
            {
                var sprite = SpriteRepository.Instance.GetInvaderSprite(
                    invader.Type,
                    _gameCore.CurrentAnimationFrame
                    );
                _renderer.DrawSprite(sprite, invader.Position, Color.White);
            }

            DrawUFO();
            DrawPlayer();
            DrawBullets();
            DrawShields();
        }

        private void DrawWaveTransition()
        {
            _renderer.Clear(Color.Black);

            var wave = _gameCore.CurrentWave;
            _renderer.DrawTextCentered($"WAVE {wave}", 100, Color.Cyan, 2);
            _renderer.DrawTextCentered("GET READY!", 130, Color.Yellow, 1);
            _renderer.DrawTextCentered($"SCORE: {_gameCore.Score}", 160, Color.Yellow, 1);
        }

        private bool IsInWaveTransition() => _gameCore.IsTransitioning;

        private void DrawPauseOverlay()
        {
            // Draw semi-transparent overlay
            var overlayRect = new Rectangle(0, 0, GameConstants.GAME_WIDTH, GameConstants.GAME_HEIGHT);
            _renderer.FillRectangle(overlayRect, Color.FromArgb(128, 0, 0, 0));
        }

        private void DrawUFO()
        {
            var ufo = _gameCore.CurrentUFO;
            if (ufo == null) return;

            var sprite = SpriteRepository.Instance.GetSprite(SpriteKey.UFO);
            _renderer.DrawSprite(sprite, ufo.Position, Color.Red);
        }

        private void DrawPlayer()
        {
            var player = _gameCore.Player;

            if (!player.ShouldRender) return;

            ISprite sprite;
            Color color = Color.White;

            if (player.State == PlayerState.Dying)
            {
                // Use explosion sprite based on animation frame
                var spriteKey = player.DeathAnimationFrame == 0
                    ? SpriteKey.PlayerExplosion1
                    : SpriteKey.PlayerExplosion2;
                sprite = SpriteRepository.Instance.GetSprite(spriteKey);
            }
            else
            {
                sprite = SpriteRepository.Instance.GetSprite(SpriteKey.Player);

                if (player.State == PlayerState.Respawning)
                    color = Color.Cyan;
            }
            _renderer.DrawSprite(sprite, player.Position, color);
        }

        private void DrawBullets()
        {
            foreach (var bullet in _gameCore.Bullets)
            {
                ISprite bulletSprite;
                Color bulletColor = Color.White;

                if (bullet.Type == BulletType.Player)
                {
                    bulletSprite = SpriteRepository.Instance.GetSprite(SpriteKey.PlayerBullet);
                }
                else
                {
                    var spriteKey = _gameCore.CurrentAnimationFrame == 0
                        ? SpriteKey.InvaderBulletFrame1
                        : SpriteKey.InvaderBulletFrame2;
                    bulletSprite = SpriteRepository.Instance.GetSprite(spriteKey);
                }

                _renderer.DrawSprite(bulletSprite, bullet.Position, bulletColor);
            }
        }

        private void DrawShields()
        {
            var shieldManager = GetShieldManager();

            foreach (var shield in shieldManager.Shields)
            {
                var pixels = shield.GetPixels();
                var position = shield.Position;

                for (int y = 0; y < ShieldData.SHIELD_HEIGHT; y++)
                {
                    for (int x = 0; x < ShieldData.SHIELD_WIDTH; x++)
                    {
                        if (pixels[x, y])
                        {
                            _renderer.DrawPixel(
                                (int)(position.X + x),
                                (int)(position.Y + y),
                                Color.Lime
                                );
                        }
                    }
                }
            }
        }

        private ShieldManager GetShieldManager()
        {
            return _gameCore.ShieldManager ?? throw new InvalidOperationException("ShieldManager is not initialized.");
        }

        private void DrawHUD()
        {
            _renderer.DrawText($"Score: {_gameCore.Score}", new Vector2(10, 10), Color.White);
            _renderer.DrawText($"Wave: {_gameCore.CurrentWave}", new Vector2(90, 10), Color.White);
            _renderer.DrawText($"Lives: {_gameCore.Lives}", new Vector2(170, 10), Color.White);
        }


        public void HandleInput(Keys key, bool isKeyDown)
        {
            var request = _currentState?.HandleInput(key, isKeyDown);
            HandleTransitionRequest(request);
        }

        private void HandleTransitionRequest(StateTransitionRequest? request)
        {
            if (request == null || request.Transition == StateTransition.None) 
                return;

            if (_transitionMap.TryGetValue(request.Transition, out var stateType))
            {
                if (_states.TryGetValue(stateType, out var newState))
                {
                    _currentState?.Exit();

                    var context = new StateTransitionContext
                    {
                        ResetGame = request.ResetGame
                    };

                    _currentState = newState;
                    _currentState.Enter(context);
                }
            }
        }

        public void StartWithState<T>() where T : IGameState
        {
            if (_states.TryGetValue(typeof(T), out var state))
            {
                _currentState = state;
                _currentState.Enter();
            }
        }
    }
}
