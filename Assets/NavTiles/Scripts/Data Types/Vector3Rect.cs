using UnityEngine;

namespace Snowcap.Utilities
{
    /// <summary>
    /// Helper class to create a Rect with two points.
    /// </summary>
    [System.Serializable]
    public class Vector3Rect
    {
        // Bottom left point of the rect.
        private Vector3 _bottomLeft;
        public Vector3 BottomLeft { get { return _bottomLeft; } }
        
        // Top right point of the rect.
        private Vector3 _topRight;
        public Vector3 TopRight { get { return _topRight; } }

        /// <summary>
        /// Constructor.
        /// </summary>
        public Vector3Rect()
        {
            _bottomLeft = new Vector3();
            _topRight = new Vector3();
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="inPoint">Creates a rect the size of this point.</param>
        public Vector3Rect(Vector3 inPoint)
        {
            _bottomLeft = inPoint;
            _topRight = inPoint;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="inBottomLeft">Bottom left corner of the rect.</param>
        /// <param name="inTopRight">Top right corner of the rect.</param>
        public Vector3Rect(Vector3 inBottomLeft, Vector3 inTopRight)
        {
            _bottomLeft = inBottomLeft;
            _topRight = inTopRight;
        }

        /// <summary>
        /// Expands the rect to make inPoint fit inside it. all variables get adjusted accordingly.
        /// </summary>
        public void UpdateRect(Vector3 inPoint)
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
        public bool InsideRect(Vector3 inPoint)
        {
            return (inPoint.x >= _bottomLeft.x && inPoint.x <= _topRight.x
                && inPoint.y >= _bottomLeft.y && inPoint.y <= _topRight.y);
        }

        /// <summary>
        /// Converts this instance to a Rect.
        /// </summary>
        public Rect AsRect()
        {
            return new Rect(_bottomLeft.x, _bottomLeft.y, _topRight.x - _bottomLeft.x, _topRight.y - _bottomLeft.y);
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
