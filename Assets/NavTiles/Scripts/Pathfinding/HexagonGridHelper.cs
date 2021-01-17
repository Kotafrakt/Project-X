using UnityEngine;

/// <summary>
/// Class that helps with Hexagonal grid calculations.
/// </summary>
public static class HexagonGridHelper
{
    public enum Direction
    {
        TopRight,
        Right,
        BottomRight,
        BottomLeft,
        Left,
        TopLeft
    }

    private const int NUMBER_OF_DIRECTIONS = 6;

    /// <summary>
    /// These are the hexagonal offsets in a certain direction. The order matches the Direction enum.
    /// </summary>
    private static readonly Vector2Int[,] directionsOffsets = { { new Vector2Int(0, 1), new Vector2Int(1, 0), new Vector2Int(0, -1),
                                                                  new Vector2Int(-1, -1), new Vector2Int(-1, 0), new Vector2Int(-1, 1) },
                                                                { new Vector2Int(1, 1), new Vector2Int(1, 0), new Vector2Int(1, -1),
                                                                  new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(0, 1) } };

    /// <summary>
    /// Gets a relative direction from the given one.
    /// </summary>
    /// <param name="inCurrentDirection">Direction to get the relative one for.</param>
    /// <param name="inNumberOfRotations">Amount of rotations clockwise. Negative will go counter-clockwise.</param>
    /// <returns>The new relative direction</returns>
    public static Direction GetRelativeDirection(Direction inCurrentDirection, int inNumberOfRotations)
    {
        return (Direction)Modulo(((int)inCurrentDirection + inNumberOfRotations), NUMBER_OF_DIRECTIONS);
    }

    /// <summary>
    /// Gets a neighbour coordinate based on the given direction.
    /// </summary>
    /// <param name="inCurrentCoordinate">Coordinate to get the neighbour for.</param>
    /// <param name="inDirection">Direction in which to get the neighbour for.</param>
    /// <returns>The coordinate of the neighbour.</returns>
    public static Vector2Int GetNeighbourInDirection(Vector2Int inCurrentCoordinate, Direction inDirection)
    {
        // Checks if the y is odd and uses it to access the direction array.
        int oddParity = inCurrentCoordinate.y & 1;
        return inCurrentCoordinate + directionsOffsets[oddParity, (int)inDirection];
    }

    /// <summary>
    /// Calculates the direction from one node to another. Nodes have to be on one line.
    /// </summary>
    public static Direction GetDirection(Vector2Int inFromCoordinate, Vector2Int inToCoordinate)
    {
        Vector3Int fromConverted = ToCubeCoordinate(inFromCoordinate);
        Vector3Int toConverted = ToCubeCoordinate(inToCoordinate);
        Vector3Int difference = toConverted - fromConverted;

        if (difference.x == 0)
            return difference.z > 0 ? Direction.TopRight : Direction.BottomLeft;

        if (difference.y == 0)
            return difference.x > 0 ? Direction.BottomRight : Direction.TopLeft;

        if (difference.z == 0)
            return difference.x > 0 ? Direction.Right : Direction.Left;

        Debug.LogError("Hexagonal direction requested for nodes that are not on 1 line.");
        return Direction.TopRight;
    }

    /// <summary>
    /// Gets the distance in amount of steps from one node to another.
    /// </summary>
    public static int GetDistance(Vector2Int inFirstCoord, Vector2Int inSecondCoord)
    {
        Vector3Int firstConverted = ToCubeCoordinate(inFirstCoord);
        Vector3Int secondConverted = ToCubeCoordinate(inSecondCoord);

        return (Mathf.Abs(firstConverted.x - secondConverted.x) +
                Mathf.Abs(firstConverted.y - secondConverted.y) +
                Mathf.Abs(firstConverted.z - secondConverted.z) / 2);
    }

    /// <summary>
    /// A slight adaption of the modulo regarding negative numbers.
    /// This makes sure the modulo pattern continues when going below 0 instead of flipping.
    /// </summary>
    private static int Modulo(int inX, int inModulo)
    {
        int result = inX % inModulo;
        return result < 0 ? result + inModulo : result;
    }

    /// <summary>
    /// Converts a 2D hexagonal coordinate to a 3D cubed coordinate.
    /// These coordinates are easier to do calculations with.
    /// </summary>
    /// <param name="inHexCoordinate">Coordinate to convert.</param>
    private static Vector3Int ToCubeCoordinate(Vector2Int inHexCoordinate)
    {
        int x = inHexCoordinate.x - (inHexCoordinate.y - (inHexCoordinate.y & 1)) / 2;
        int z = inHexCoordinate.y;
        int y = -x - z;

        return new Vector3Int(x, y, z);
    }

    /// <summary>
    /// Converts a 3D cubed coordinate to a 2D hexagonal coordinate.
    /// Used to convert back to the unity grid coordinates.
    /// </summary>
    /// <param name="inCubeCoordinate">Coordinate to convert.</param>
    private static Vector2Int ToHexCoordinate(Vector3Int inCubeCoordinate)
    {
        return new Vector2Int(inCubeCoordinate.x + (inCubeCoordinate.z - (inCubeCoordinate.z & 1)) / 2, 
                              inCubeCoordinate.z);
    }
}
