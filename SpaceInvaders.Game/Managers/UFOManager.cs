using SpaceInvaders.Game.Entities;
using SpaceInvaders.Game.Domain;

namespace SpaceInvaders.Game.Managers
{
    /// <summary>
    /// Manages UFO spawning and lifecycle.
    /// Implements the mysterious scoring algorithm
    /// </summary>
    public class UFOManager
    {
        private readonly Random _random;
        private UFO? _currentUFO;
        private float _spawnerTimer;
        private float _nextSpawnTime;
        private int _shotsFiredSinceLastUFO;

        public UFO? CurrentUFO => _currentUFO?.IsActive == true ? _currentUFO : null;
        public event EventHandler<UFO>? UFODestroyed;

        public UFOManager()
        {
            _random = new Random();
            ResetSpawnTimer();
            _shotsFiredSinceLastUFO = 0;
        }

        public void Update(float deltaTime)
        {
            // Update current UFO
            if (_currentUFO?.IsActive == true)
            {
                _currentUFO.Update(deltaTime);

                if (!_currentUFO.IsActive)
                    _currentUFO = null;
            }

            // Update spawn timer
            if (_currentUFO == null)
            {
                _spawnerTimer += deltaTime;

                if (_spawnerTimer >= _nextSpawnTime)
                    SpawnUFO();
            }
        }

        public void OnPlayerShot() => _shotsFiredSinceLastUFO++;

        private void SpawnUFO()
        {
            // Random direction
            bool movingRight = _random.Next(2) == 0;

            int pointValue = CalculateUFOValue();

            _currentUFO = new UFO(movingRight, pointValue);
            ResetSpawnTimer();
        }

        private int CalculateUFOValue()
        {
            // Classic Space Invaders UFO scoring:
            // Based on number of shots fired by player
            // Every 15th shot = 300 points, otherwise random

            if (_shotsFiredSinceLastUFO > 0 && _shotsFiredSinceLastUFO % 15 == 0)
                return 300;

            // Random value: 50, 100, 150
            int[] possibleValues = { 50, 100, 150 };
            return possibleValues[_random.Next(possibleValues.Length)];
        }

        public bool TryDestroyUFO(Vector2 hitPoint)
        {
            if (_currentUFO?.IsActive == true && _currentUFO.Bounds.Contains(hitPoint))
            {
                var points = _currentUFO.PointValue;
                _currentUFO.Destroy();
                UFODestroyed?.Invoke(this, _currentUFO);
                _currentUFO = null;
                _shotsFiredSinceLastUFO = 0;
                return true;
            }
            return false;
        }

        private void ResetSpawnTimer()
        {
            _spawnerTimer = 0;
            _nextSpawnTime = _random.Next(
                (int)GameConstants.UFO_MIN_SPAWN_TIME,
                (int)GameConstants.UFO_MAX_SPAWN_TIME
             );
        }

        public void Reset()
        {
            _currentUFO = null;
            _shotsFiredSinceLastUFO = 0;
            ResetSpawnTimer();
        }
    }
}
