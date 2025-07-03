namespace SpaceInvaders.Game.Input
{
    /// <summary>
    /// Interface for handling player input
    /// </summary>
    public interface IInputHandler
    {
        bool IsLeftPressed { get; }
        bool IsRightPressed { get; }
        bool IsFirePressed { get; }

        // Separate property for edge detection
        bool WasFireJustPressed { get; }

        // Must be called each frame to update input state
        void Update();
    }
}
