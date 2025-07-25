using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Managers;
using SpaceInvaders.Game.Input;
using Rectangle = SpaceInvaders.Game.Domain.Rectangle;

namespace SpaceInvaders.Game
{
    public enum GameState
    {
        Playing,
        Paused,
        GameOver,
        Victory
    }

    public class GameCore
    {
        private readonly BulletManager _bulletManager;
        private readonly CollisionManager _collisionManager;
        private readonly UFOManager _ufoManager;
        private readonly ShieldManager _shieldManager;

        private readonly AnimationController _invaderAnimator;
        private readonly InvaderShootingController _invaderShootingController;

        private readonly InvaderFormation _invaderFormation;
        private readonly Player _player;

        private GameState _state = GameState.Playing;
        private int _score;
        private int _lives;

        private int _currentWave;
        private bool _transitioning;
        private float _transitionTimer;
        private const float WAVE_TRANSITION_TIME = 2.0f;

        public GameState State => _state;
        public int Score => _score;
        public int Lives => _lives;
        public IEnumerable<Invader> Invaders => _invaderFormation.Invaders;
        public int ActiveInvaders => _invaderFormation.ActiveCount;
        public int CurrentAnimationFrame => _invaderAnimator.CurrentFrame;
        public Player Player => _player;
        public UFO? CurrentUFO => _ufoManager.CurrentUFO;
        public ShieldManager ShieldManager => _shieldManager;
        public IEnumerable<Bullet> Bullets => _bulletManager.Bullets;
        public int CurrentWave => _currentWave;
        public bool IsTransitioning => _transitioning;

        public GameCore(IInputHandler inputHandler)
        {
            if (inputHandler == null)
                throw new ArgumentNullException(nameof(inputHandler));

            // Initialize game objects
            _bulletManager = new BulletManager();
            _ufoManager = new UFOManager();
            _invaderAnimator = new AnimationController(GameConstants.INVADER_ANIMATION_INTERVAL);
            _invaderShootingController = new InvaderShootingController(_bulletManager);
            _shieldManager = new ShieldManager();
            _collisionManager = new CollisionManager(_shieldManager);

            _player = new Player(GameConstants.PlayerStartPosition, inputHandler);
            _invaderFormation = new InvaderFormation();

            _lives = GameConstants.PLAYER_LIVES;
            _score = 0;

            // Subscribe to game events
            _invaderFormation.InvaderDestroyed += OnInvaderDestroyed;
            _invaderFormation.ReachedBottom += OnInvadersReachedBottom;
            _player.FireRequested += OnPlayerFireRequested;
            _collisionManager.CollisionDetected += OnCollisionDetected;
            _player.DeathAnimationComplete += OnPlayerDeathAnimationComplete;
            _ufoManager.UFODestroyed += OnUFODestroyed;

            _currentWave = 1;
            _transitioning = false;
            _transitionTimer = 0f;
        }

        public void Update(float deltaTime)
        {
            if (_state != GameState.Playing) return;

            if (_transitioning)
            {
                _transitionTimer -= deltaTime;
                if (_transitionTimer <= 0f)
                {
                    StartNewWave();
                    _transitioning = false;
                }
                return;
            }

            // Update all game objects
            _invaderFormation.Update(deltaTime);
            _invaderAnimator.Update(deltaTime);
            _bulletManager.Update(deltaTime);
            _player.Update(deltaTime);
            _ufoManager.Update(deltaTime);

            // Update invader shooting
            _invaderShootingController.Update(deltaTime, _invaderFormation.Invaders);

            // Check collisions
            _collisionManager.CheckCollisions(
                _bulletManager.Bullets,
                _invaderFormation.Invaders,
                _player
                );

            CheckUFOCollisions();

            // Check victory condition
            if (_invaderFormation.ActiveCount == 0)
            {
                _transitioning = true;
                _transitionTimer = WAVE_TRANSITION_TIME;
            }
        }

        private void StartNewWave()
        {
            _currentWave++;

            _invaderFormation.ResetForWave(_currentWave);

            _bulletManager.Clear();
            _invaderShootingController.Reset();
            _shieldManager.Reset();
            _invaderAnimator.Reset();
            _ufoManager.Reset();
        }

        private void CheckUFOCollisions()
        {
            if (_ufoManager.CurrentUFO == null) return;

            foreach (var bullet in _bulletManager.PlayerBullets.ToList())
            {
                if (bullet.Bounds.Intersects(_ufoManager.CurrentUFO.Bounds))
                {
                    _bulletManager.DestroyBullet(bullet);
                    _ufoManager.TryDestroyUFO(bullet.Position + bullet.Size / 2);
                    break;
                }
            }
        }

        private void HandlePlayerBulletHitInvader(Bullet? bullet, Invader? invader)
        {
            if (bullet == null || invader == null) return;

            _bulletManager.DestroyBullet(bullet);
            _invaderFormation.DestroyInvaderAt(invader.Position + invader.Size / 2);
        }

        private void HandleInvaderBulletHitPlayer(Bullet? bullet)
        {
            if (bullet == null) return;

            _bulletManager.DestroyBullet(bullet);

            if (_player.IsVulnerable)
            {
                _player.Hit();
                _lives--;
            }

            if (_lives <= 0)
                _state = GameState.GameOver;
        }

        private void OnCollisionDetected(object? sender, CollisionEventArgs e)
        {
            switch (e.Type)
            {
                case CollisionType.PlayerBulletHitInvader:
                    HandlePlayerBulletHitInvader(e.Entity1 as Bullet, e.Entity2 as Invader);
                    break;

                case CollisionType.InvaderBulletHitPlayer:
                    HandleInvaderBulletHitPlayer(e.Entity1 as Bullet);
                    break;
            }
        }

        private void OnPlayerDeathAnimationComplete(object? sender, EventArgs e)
        {
            if (_lives > 0)
            {
                ClearBulletsNearPosition(GameConstants.PlayerStartPosition);
                _player.Respawn(GameConstants.PlayerStartPosition);
            }
        }

        private void ClearBulletsNearPosition(Vector2 position)
        {
            var safeZone = new Rectangle(
                position.X - 10,
                position.Y - 10,
                _player.Size.X + 20,
                _player.Size.Y + 20
                );

            foreach (var bullet in _bulletManager.Bullets.ToList())
            {
                if (bullet.Type == BulletType.Invader && safeZone.Intersects(bullet.Bounds))
                {
                    bullet.Destroy();
                }
            }
        }

        private void OnInvaderDestroyed(object? sender, Invader invader)
        {
            _score += invader.PointValue;
        }

        private void OnInvadersReachedBottom(object? sender, EventArgs e)
        {
            _state = GameState.GameOver;
        }

        private void OnPlayerFireRequested(object? sender, EventArgs e)
        {
            // Calculate bullet spawn position (center-top of player)
            var bulletX = _player.Position.X + (_player.Size.X / 2) - 0.5f;
            var bulletY = _player.Position.Y - 1;

            var bulletPosition = new Vector2(bulletX, bulletY);

            if (_bulletManager.TryFirePlayerBullet(bulletPosition))
            {
                // Track shot for UFO scoring
                _ufoManager.OnPlayerShot();
            }
        }

        private void OnUFODestroyed(object? sender, UFO ufo)
        {
            _score += ufo.PointValue;
            // Could trigger special sound effect here
            Console.WriteLine($"UFO destroyed! Points: {ufo.PointValue}");
        }

        public void Reset()
        {
            _currentWave = 1;
            _transitioning = false;
            _transitionTimer = 0f;

            _invaderFormation.Reset();
            _invaderAnimator.Reset();
            _bulletManager.Clear();
            _invaderShootingController.Reset();
            _ufoManager.Reset();
            _shieldManager.Reset();

            _player.Position = GameConstants.PlayerStartPosition;

            _score = 0;
            _lives = GameConstants.PLAYER_LIVES;
            _state = GameState.Playing;
        }
    }
}
