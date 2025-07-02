using SpaceInvaders.Game.Domain;
using SpaceInvaders.Game.Entities;

namespace SpaceInvaders.Game.Managers
{
    /// <summary>
    /// Manages the formation and movement of all invaders.
    /// Implements the classic Space Invaders movement pattern.
    /// </summary>
    public class InvaderFormation
    {
        private readonly Invader[,] _grid;
        private readonly int _rows;
        private readonly int _columns;
        private Vector2 _formationPosition;
        private float _horizontalSpeed;
        private int _direction = 1; // 1 for right, -1 for left
        private float _dropDistance;
        private int _activeCount;

        // Events for game integration
        public event EventHandler<Invader>? InvaderDestroyed;
        public event EventHandler? ReachedBottom;

        public int ActiveCount => _activeCount;
        public IEnumerable<Invader> Invaders => GetActiveInvaders();

        public InvaderFormation(
            int rows = GameConstants.INVADER_ROWS,
            int columns = GameConstants.INVADER_COLUMNS)
        {
            _rows = rows;
            _columns = columns;
            _grid = new Invader[rows, columns];
            _horizontalSpeed = GameConstants.INVADER_BASE_SPEED;
            _dropDistance = GameConstants.INVADER_DROP_DISTANCE;

            // Center the formation
            var formationWidth = columns * GameConstants.INVADER_HORIZONTAL_SPACING;
            _formationPosition = new Vector2(
                (GameConstants.GAME_WIDTH - formationWidth) / 2f,
                GameConstants.FORMATION_TOP_MARGIN
                );

            InitializeInvaders();
        }

        private void InitializeInvaders()
        {
            _activeCount = 0;

            for (int row = 0; row < _rows; row++)
            {
                var invaderType = GetInvaderTypeForRow(row);

                for (int col = 0; col < _columns; col++)
                {
                    var position = GetInvaderPosition(row, col);
                    _grid[row, col] = new Invader(position, invaderType);
                    _activeCount++;
                }
            }
        }

        private InvaderType GetInvaderTypeForRow(int row)
        {
            return row switch
            {
                0 => InvaderType.Small,
                1 => InvaderType.Small,
                2 => InvaderType.Medium,
                3 => InvaderType.Medium,
                4 => InvaderType.Large,
                _ => InvaderType.Large
            };
        }

        private Vector2 GetInvaderPosition(int row, int col) => _formationPosition + new Vector2(
                col * GameConstants.INVADER_HORIZONTAL_SPACING,
                row * GameConstants.INVADER_VERTICAL_SPACING
            );

        public void Update(float deltaTime)
        {
            if (_activeCount == 0) return;

            var movement = new Vector2(_direction * _horizontalSpeed * deltaTime, 0);
            bool needsToDrop = CheckBoundaryCollision(movement);

            if (needsToDrop)
            {
                DropDown();
                ReverseDirection();
                IncreaseSpeed();
            }
            else
            {
                MoveHorizontally(movement);
            }
        }

        private bool CheckBoundaryCollision(Vector2 movement)
        {
            foreach (var invader in GetActiveInvaders())
            {
                var futurePosition = invader.Position + movement;

                if (futurePosition.X < 0 || 
                    futurePosition.X + invader.Size.X > GameConstants.GAME_WIDTH)
                {
                    return true; 
                }
            }
            return false;
        }

        private void MoveHorizontally(Vector2 movement)
        {
            _formationPosition += movement;
            foreach (var invader in GetActiveInvaders())
            {
                invader.Move(movement);
            }
        }

        private void DropDown()
        {
            var dropMovement = Vector2.Down * _dropDistance;
            _formationPosition += dropMovement;

            foreach (var invader in GetActiveInvaders())
            {
                invader.Move(dropMovement);

                var bottomEdge = invader.Position.Y + invader.Size.Y;
                var dangerLine = GameConstants.GAME_HEIGHT - GameConstants.FORMATION_BOTTOM_DANGER_ZONE;

                if (bottomEdge >= dangerLine)
                {
                    ReachedBottom?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void ReverseDirection() => _direction *= -1;

        private void IncreaseSpeed()
        {
            var destroyedCount = GameConstants.TotalInvaders - _activeCount;
            _horizontalSpeed = GameConstants.INVADER_BASE_SPEED
                               + (destroyedCount * GameConstants.INVADER_SPEED_INCREMENT_PER_KILL);
        }

        private IEnumerable<Invader> GetActiveInvaders()
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    var invader = _grid[row, col];
                    if (invader != null)
                        yield return invader;
                }
            }
        }

        public void DestroyInvaderAt(Vector2 position)
        {
            foreach (var invader in GetActiveInvaders())
            {
                if (invader.Bounds.Contains(position))
                {
                    invader.Destroy();
                    _activeCount--;
                    InvaderDestroyed?.Invoke(this, invader);
                    IncreaseSpeed();
                    break;
                }
            }
        }

        public void Reset()
        {
            // Clear existing invaders
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    _grid[row, col] = null;
                }
            }

            // Reset speed and direction
            _horizontalSpeed = GameConstants.INVADER_BASE_SPEED;
            _direction = 1;

            // Reinitialize
            var formationWidth = _columns * GameConstants.INVADER_HORIZONTAL_SPACING;
            _formationPosition = new Vector2(
                (GameConstants.GAME_WIDTH - formationWidth) / 2f,
                GameConstants.FORMATION_TOP_MARGIN
            );

            InitializeInvaders();
        }
    }
}
