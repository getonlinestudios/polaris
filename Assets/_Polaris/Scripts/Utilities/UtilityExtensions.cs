using Polaris.Characters.Components;

namespace Polaris.Utilities
{
    public static class UtilityExtensions
    {
        public static int AsValue(this FacingDirection facingDirection)
        {
            return (int)facingDirection;
        }
    }
}