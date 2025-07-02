namespace SpaceInvaders.Game.Graphics
{
    /// <summary>
    /// Manages sprite animation timing and frame selection
    /// </summary>
    public class AnimationController
    {
        private float _timer;
        private int _currentFrame;
        private readonly float _frameInterval;
        private readonly int _frameCount;

        public int CurrentFrame => _currentFrame;
        public float FrameInterval => _frameInterval;

        public AnimationController(float frameInterval, int frameCount = 2)
        {
            _frameInterval = frameInterval;
            _frameCount = frameCount;
            _timer = 0f;
            _currentFrame = 0;
        }

        public void Update(float deltaTime)
        {
            _timer += deltaTime;

            while (_timer >= _frameInterval)
            {
                _timer -= _frameInterval;
                _currentFrame = (_currentFrame + 1) % _frameCount;
            }
        }

        public void Reset()
        {
            _timer = 0f;
            _currentFrame = 0;
        }
    }
}
