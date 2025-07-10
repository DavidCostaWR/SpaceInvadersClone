using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Graphics;
using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Managers;
using SpaceInvaders.Game.Input;

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
        private readonly AnimationController _invaderAnimator;
        private readonly InvaderShootingController _invaderShootingController;
        private readonly InvaderFormation _invaderFormation;
        private readonly Player _player;
        private GameState _state = GameState.Playing;
        private int _score;
        private int _lives;

        public GameState State => _state;
        public int Score => _score;
        public int Lives => _lives;
        public IEnumerable<Invader> Invaders => _invaderFormation.Invaders;
        public int ActiveInvaders => _invaderFormation.ActiveCount;
        public int CurrentAnimationFrame => _invaderAnimator.CurrentFrame;
        public Player Player => _player;
        public IEnumerable<Bullet> Bullets => _bulletManager.Bullets;

        public GameCore(IInputHandler inputHandler)
        {
            if (inputHandler == null)
                throw new ArgumentNullException(nameof(inputHandler));

            // Initialize game objects
            _bulletManager = new BulletManager();
            _collisionManager = new CollisionManager();
            _invaderAnimator = new AnimationController(GameConstants.INVADER_ANIMATION_INTERVAL);
            _invaderShootingController = new InvaderShootingController(_bulletManager);

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
        }

        public void Update(float deltaTime)
        {
            if (_state != GameState.Playing) return;

            // Update all game objects
            _invaderFormation.Update(deltaTime);
            _invaderAnimator.Update(deltaTime);
            _bulletManager.Update(deltaTime);
            _player.Update(deltaTime);

            // Update invader shooting
            _invaderShootingController.Update(deltaTime, _invaderFormation.Invaders);

            // Check collisions
            _collisionManager.CheckCollisions(
                _bulletManager.Bullets,
                _invaderFormation.Invaders,
                _player
                );

            // Check victory condition
            if (_invaderFormation.ActiveCount == 0)
                _state = GameState.Victory;
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
                _player.Respawn(GameConstants.PlayerStartPosition);
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
            // Valvulate bullet spawn position (center-top of player)
            var bulletX = _player.Position.X + (_player.Size.X / 2) - 0.5f;
            var bulletY = _player.Position.Y - 1;

            var bulletPosition = new Vector2(bulletX, bulletY);

            if (_bulletManager.TryFirePlayerBulet(bulletPosition))
                Console.WriteLine("Bullet fired!"); // could play sound effect here
        }

        public void Reset()
        {
            _invaderFormation.Reset();
            _invaderAnimator.Reset();
            _bulletManager.Clear();
            _invaderShootingController.Reset();

            _player.Position = GameConstants.PlayerStartPosition;

            _score = 0;
            _lives = GameConstants.PLAYER_LIVES;
            _state = GameState.Playing;
        }
    }
}
