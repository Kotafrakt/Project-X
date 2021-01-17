using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Snowcap.NavTiles
{
    /// <summary>
    /// Line of Sight Smoothing class.
    /// </summary>
    public class LOSSmoothing : INavTilePathModifier
    {
        /// <summary>
        /// Modifies the input path by removing any nodes which are not neccesarry for the path to still be valid.
        /// </summary>
        public NavTilePath ModifyPath(NavTilePath inPath)
        {
            // If the path is too short, return the path as is.
            if (inPath.Count <= 2)
            {
                return inPath;
            }

            NavTilePath modifiedPath = inPath;

            // Iterate over all nodes between start and finish.
            // (i < inPath.Count - 1) is deliberate, since the last iteration needs to be the node before the last one.
            for (int i = 1; i < modifiedPath.Count - 1;)
            {
                Vector2Int before = modifiedPath[i - 1].TilePosition;
                Vector2Int after = modifiedPath[i + 1].TilePosition;

                if (IsWalkable(before, after, inPath.AreaMask))
                {
                    // If the area between the 2 points is walkable, remove the point in between.
                    // Don't increment 'i' as the next node will be on the current index.
                    modifiedPath.RemoveAt(i);
                }
                else
                {
                    // If the tile can't be removed, move to the next one
                    i++;
                }
            }

            return modifiedPath;
        }

        /// <summary>
        /// Checks whether the area from pointA to pointB is walkable based on the given area mask.
        /// </summary>
        public bool IsWalkable(Vector2Int inPointA, Vector2Int inPointB, int inAreaMask)
        {
            foreach(var node in GetPointsOnLine(inPointA, inPointB, true))
            {
                if (!NavTileManager.Instance.SurfaceManager.Data.IsTileWalkable(node, inAreaMask))
                {
                    return false;
                }
            }

            return true; 
        }

        /// <summary>
        /// Added option to fill gaps for continuous lines.
        /// </summary>
        public IEnumerable<Vector2Int> GetPointsOnLine(Vector2Int inStartPos, Vector2Int inEndPos, bool inFillGaps)
        {
            var points = GetPointsOnLine(inStartPos, inEndPos);
            if (inFillGaps)
            {
                var rise = inEndPos.y - inStartPos.y;
                var run = inEndPos.x - inStartPos.x;

                if (rise != 0 || run != 0)
                {
                    var extraStart1 = inStartPos;
                    var extraEnd1 = inEndPos;

                    var extraStart2 = inStartPos;
                    var extraEnd2 = inEndPos;


                    if (Mathf.Abs(rise) >= Mathf.Abs(run))
                    {
                        extraStart1.x += 1;
                        extraEnd1.x += 1;

                        extraStart2.x -= 1;
                        extraEnd2.x -= 1;
                    }
                    else // Mathf.Abs(rise) < Mathf.Abs(run)
                    {

                        extraStart1.y += 1;
                        extraEnd1.y += 1;

                        extraStart2.y -= 1;
                        extraEnd2.y -= 1;
                    }

                    var extraPoints1 = GetPointsOnLine(extraStart1, extraEnd1);
                    var extraPoints2 = GetPointsOnLine(extraStart2, extraEnd2);
                    points = points.Union(extraPoints1).Union(extraPoints2);
                }
            }

            return points;
        }

        /// <summary>
        /// Gets an enumerable for all the cells directly between two points.
        /// http://ericw.ca/notes/bresenhams-line-algorithm-in-csharp.html
        /// </summary>
        /// <param name="inP1">A starting point of a line.</param>
        /// <param name="inP2">An ending point of a line.</param>
        /// <returns>Gets an enumerable for all the cells directly between two points.</returns>
        public IEnumerable<Vector2Int> GetPointsOnLine(Vector2Int inP1, Vector2Int inP2)
        {
            int x0 = inP1.x;
            int y0 = inP1.y;
            int x1 = inP2.x;
            int y1 = inP2.y;

            bool steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // Swap x0 and y0.
                x0 = y0;
                y0 = t;
                t = x1; // Swap x1 and y1.
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // Swap x0 and x1.
                x0 = x1;
                x1 = t;
                t = y0; // Swap y0 and y1.
                y0 = y1;
                y1 = t;
            }
            int dx = x1 - x0;
            int dy = Math.Abs(y1 - y0);
            int error = dx / 2;
            int ystep = (y0 < y1) ? 1 : -1;
            int y = y0;
            for (int x = x0; x <= x1; x++)
            {
                yield return new Vector2Int((steep ? y : x), (steep ? x : y));
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
            yield break;
        }

        /// <summary>
        /// Returns the name to be showed in the NavTile Settings Window.
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            return "Line of Sight Smoothing";
        }
    }
}
