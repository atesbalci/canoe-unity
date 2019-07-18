using UnityEngine;

namespace Framework.Scripts.Utils
{
    public class SquareGridDirection
    {
        public static readonly Vector2Int Top = Vector2Int.up;
        public static readonly Vector2Int Right = Vector2Int.right;
        public static readonly Vector2Int Bottom = Vector2Int.down;
        public static readonly Vector2Int Left = Vector2Int.left;

        public static readonly Vector2Int[] DirectionsByIndices =
        {
            Top, Right, Bottom, Left
        };

        public static Vector2Int GetByIndex(int index)
        {
            return DirectionsByIndices[index];
        }
    }

    public class HexagonalGridDirection
    {
        public static readonly Vector3Int TopRight = new Vector3Int(1, -1, 0);
        public static readonly Vector3Int Right = new Vector3Int(1, 0, -1);
        public static readonly Vector3Int BottomRight = new Vector3Int(0, 1, -1);
        public static readonly Vector3Int BottomLeft = new Vector3Int(-1, 1, 0);
        public static readonly Vector3Int Left = new Vector3Int(-1, 0, 1);
        public static readonly Vector3Int TopLeft = new Vector3Int(0, -1, 1);

        public static readonly Vector3Int[] DirectionsByIndices =
        {
            TopRight, Right, BottomRight, BottomLeft, Left, TopLeft
        };

        public static Vector3Int GetByIndex(int index)
        {
            return DirectionsByIndices[index];
        }
    }

    public static class Vector2IntExtension
    {
        public static Vector2Int GetAdjacent(this Vector2Int vector, Vector2Int direction)
        {
            return vector + direction;
        }
    }
}