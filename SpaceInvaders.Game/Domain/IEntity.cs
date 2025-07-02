namespace SpaceInvaders.Game.Domain
{
    /// <summary>
    /// Contract for all game entities.
    /// Interface-first design for flexibility.
    /// </summary>
    public interface IEntity
    {
        Vector2 Position { get; }
        Rectangle Bounds { get; }
        bool IsActive { get; }

        void Update(float deltaTime);
    }
}
