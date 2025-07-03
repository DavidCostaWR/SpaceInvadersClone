namespace SpaceInvaders.Game.Input
{
    /// <summary>
    /// Handles keyboard input for the game.
    /// Maintains current and previous state for edge detection.
    /// </summary>
    public class KeyboardInputHandler : IInputHandler
    {
        // Using HashSets for O(1) lookup performance
        private readonly HashSet<Keys> _currentKeys = new();
        private readonly HashSet<Keys> _previousKeys = new();

        // Support both arrow keys and WASD
        public bool IsLeftPressed => 
            _currentKeys.Contains(Keys.Left) || _currentKeys.Contains(Keys.A);

        public bool IsRightPressed => 
            _currentKeys.Contains(Keys.Right) || _currentKeys.Contains(Keys.D);

        public bool IsFirePressed =>
            _currentKeys.Contains(Keys.Space);

        // Edge detection: pressed this frame but not last frame
        public bool WasFireJustPressed => 
            IsFirePressed && !_previousKeys.Contains(Keys.Space);

        // Called by Form's KeyDown event
        public void KeyDown(Keys key)
        {
            _currentKeys.Add(key);
        }

        // Called by Form's KeyUp event
        public void KeyUp(Keys key)
        {
            _currentKeys.Remove(key);
        }

        public void Update()
        {
            _previousKeys.Clear();
            foreach (var key in _currentKeys)
            {
                _previousKeys.Add(key);
            }
        }
    }
}
