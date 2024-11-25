using UnityEngine;

namespace Polaris
{
    public class Utility
    {
        /// <summary>
        /// Takes a value and turns it into a float between 0 and 1.
        /// </summary>
        /// <param name="value">The value to turn to 0 or 1.</param>
        /// <param name="min1">The lowest possible number value can be.</param>
        /// <param name="max1">The highest possible number value can be.</param>
        /// <param name="min2">The desired lowest possible number the output will be.</param>
        /// <param name="max2">The desired highest possible number the output will be.</param>
        /// <param name="clamp">Should the value be clamped between the input.</param>
        /// <returns>The current value as a float.</returns>
        public static float Map(float value, float min1, float max1, float min2, float max2, bool clamp = false)
        {
            var v = min2 + (max2 - min2) * ((value - min1) / (max1 - min1));

            return clamp ? Mathf.Clamp(v, Mathf.Min(min2, max2), Mathf.Max(min2, max2)) : v;
        }
    }
}