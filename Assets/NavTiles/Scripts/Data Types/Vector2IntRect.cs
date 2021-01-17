using UnityEngine;

namespace Snowcap.Utilities
{
    /// <summary>
    /// Helper class to create a Rect with two points.
    /// </summary>
    [System.Serializable]
    public class Vector2IntRect
    {
        // Bottom left point of the rect.
        [SerializeField]
        private Vector2Int _bottomLeft;
        public Vector2Int BottomLeft { get { return _bottomLeft; } }

        // Top right point of the rect.
        [SerializeField]
        private Vector2Int _topRight;
        public Vector2Int TopRight { get { return _topRight; } }

        /// <summary>
        /// Constructor
        /// </summary>
        public Vector2IntRect()
        {
            _bottomLeft = new Vector2Int();
            _topRight = new Vector2Int();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="inPoint">Creates a rect the size of this point.</param>
        public Vector2IntRect(Vector2Int inPoint)
        {
            _bottomLeft = inPoint;
            _topRight = inPoint;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="inBottomLeft">Bottom left corner of the rect.</param>
        /// <param name="inTopRight">Top right corner of the rect.</param>
        public Vector2IntRect(Vector2Int inBottomLeft, Vector2Int inTopRight)
        {
            _bottomLeft = inBottomLeft;
            _topRight = inTopRight;
        }

        /// <summary>
        /// Expands the rect to make inPoint fit inside it. all variables get adjusted accordingly.
        /// </summary>
        public void UpdateRect(Vector2Int inPoint)
        {
            if (inPoint.x < _bottomLeft.x)
            {
                _bottomLeft.x = inPoint.x;
            }
            else if (inPoint.x > _topRight.x)
            {
                _topRight.x = inPoint.x;
            }

            if (inPoint.y < _bottomLeft.y)
            {
                _bottomLeft.y = inPoint.y;
            }
            else if (inPoint.y > _topRight.y)
            {
                _topRight.y = inPoint.y;
            }
        }

        /// <summary>
        /// Checks whether the given point is inside this rect.
        /// </summary>
        public bool InsideRect(Vector2Int inPoint)
        {
            return (inPoint.x >= _bottomLeft.x && inPoint.x <= _topRight.x
                && inPoint.y >= _bottomLeft.y && inPoint.y <= _topRight.y);
        }

        /// <summary>
        /// Converts this instance to a Rect.
        /// </summary>
        public RectInt AsRect()
        {
            return new RectInt(_bottomLeft.x, _bottomLeft.y, _topRight.x - _bottomLeft.x, _topRight.y - _bottomLeft.y);
        }

        /// <summary>
        /// Prints the values of this rect.
        /// </summary>
        public void Print()
        {
            Debug.Log("BottomLeft: " + _bottomLeft);
            Debug.Log("TopRight: " + _topRight);
        }
    }
}
