using SpaceInvaders.Game.Graphics;
using Xunit;

namespace SpaceInvaders.Tests.Graphics
{
    public class AnimationControllerTests
    {
        [Fact]
        public void Update_ShouldAdvanceFrameAfterInterval()
        {
            // Arrange
            var controller = new AnimationController(0.5f, 2);

            // Act
            controller.Update(0.3f); // Should not advance frame

            // Assert
            Assert.Equal(0, controller.CurrentFrame);

            // Act
            controller.Update(0.3f); // Should advance frame

            // Assert
            Assert.Equal(1, controller.CurrentFrame);
        }

        [Fact]
        public void Update_ShouldWrapAroundToFirstFrame()
        {
            // Arrange
            var controller = new AnimationController(0.5f, 2);

            // Act
            controller.Update(0.6f); // Advance to frame 1
            controller.Update(0.6f); // Should wrap to frame 0

            // Assert
            Assert.Equal(0, controller.CurrentFrame);
        }

        [Fact]
        public void Reset_ShouldReturnToFirstFrame()
        {
            // Arrange
            var controller = new AnimationController(0.5f, 2);

            // Act
            controller.Update(0.6f); // Advance to frame 1
            controller.Reset(); // Return to frame 0

            // Assert
            Assert.Equal(0, controller.CurrentFrame);
        }
    }
}
