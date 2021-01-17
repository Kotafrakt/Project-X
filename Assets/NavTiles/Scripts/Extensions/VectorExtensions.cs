using UnityEngine;

namespace Snowcap.Extensions
{
    /// <summary>
    /// Extensions on vectors.
    /// </summary>
    public static class VectorExtensions
    {
        /// <summary>
        /// Gets a Vector3Int where the z component is set to 0.
        /// </summary>
        /// <param name="vector">Vector to get the 3D version of.</param>
        /// <returns>A Vector3Int with the same x and y values but a z of 0.</returns>
        public static Vector3Int GetVector3Int(this Vector2Int vector)
        {
            return new Vector3Int(vector.x, vector.y, 0);
        }

        /// <summary>
        /// Gets a Vector2Int ignoring the z component of the given vector.
        /// </summary>
        /// <param name="vector">Vector to get the 2D version of.</param>
        /// <returns>A Vector2Int with the same x and y values but no z value.</returns>
        public static Vector2Int GetVector2Int(this Vector3Int vector)
        {
            return new Vector2Int(vector.x, vector.y);
        }
    }
}
