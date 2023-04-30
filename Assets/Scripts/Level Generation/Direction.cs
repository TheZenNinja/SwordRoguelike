using System.Collections;
using UnityEngine;

namespace LevelGeneration
{
    public enum Direction
    {
        north, east, south, west
    }
    public static class DirectionFunctions
    {
        public static Direction RotateLeft(this Direction dir)
        {
            var i = (int)dir;
            i--;
            return (Direction)Clamp(i);
        }
        public static Direction RotateRight(this Direction dir)
        {
            var i = (int)dir;
            i++;
            return (Direction)Clamp(i);
        }
        public static Direction Reverse(this Direction dir)
        {
            var i = (int)dir;
            i -= 2;
            return (Direction)Clamp(i);
        }
        private static float Clamp(int i)
        {
            if (i > 3)
                i -= 4;
            else if (i < 0)
                i += 4;

            return i;
        }

        public static Vector2Int Add(Vector2Int v, Direction d)
        {
            return v + d.ToPosition();
        }

        public static Vector2Int ToPosition(this Direction dir)
        {
            switch (dir)
            {
                case Direction.north:
                    return Vector2Int.up;
                case Direction.east:
                    return Vector2Int.right;
                case Direction.south:
                    return Vector2Int.down;
                case Direction.west:
                    return Vector2Int.left;
                default:
                    return Vector2Int.zero;
            }
        }
    }
}