namespace SpaceInvaders.Game.Domain
{
    /// <summary>
    /// Base implementation for game entities.
    /// Abstract because we never instantiate a raw Entity.
    /// </summary>
    public abstract class Entity : IEntity
    {
        public Vector2 Position { get; set; }
        public Vector2 Size { get; }
        public bool IsActive { get; protected set; }

        // computed property for bounding box, always synced with Position
        public Rectangle Bounds => new(Position, Size);

        protected Entity(Vector2 position, Vector2 size)
        {
            Position = position;
            Size = size;
            IsActive = true;
        }

        public abstract void Update(float deltaTime);

        public virtual void Destroy() => IsActive = false;
    }
}
