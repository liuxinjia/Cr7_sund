using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyData
{
    public static class MydirectionData
    {
        [System.Flags]
        public enum DirectionEnum
        {
            Notready = 0,
            Up = 2,
            Down = 4,
            Left = 8,
            Right = 16
        }

        [System.Flags]
        public enum SquareVertEnum
        {
            TopLeft,
            BottomLeft,
            TopRight,
            BottomRight,
            Middle
        }

        public static SquareVertEnum DirToSquareVert(DirectionEnum dir)
        {
            if (dir == DirectionEnum.Down)
            {
                return MydirectionData.SquareVertEnum.BottomLeft;
            }
            if (dir == DirectionEnum.Left)
            {
                return MydirectionData.SquareVertEnum.TopLeft;
            }
            if (dir == DirectionEnum.Up)
            {
                return MydirectionData.SquareVertEnum.TopRight;
            }
            if (dir == DirectionEnum.Right)
            {
                return MydirectionData.SquareVertEnum.BottomRight;
            }

            return MydirectionData.SquareVertEnum.Middle;

        }

        public static DirectionEnum SquareVertToDir(SquareVertEnum vert)
        {
            if (vert == SquareVertEnum.TopLeft)
            {
                return DirectionEnum.Left;
            }
            if (vert == SquareVertEnum.BottomLeft)
            {
                return DirectionEnum.Down;
            }
            if (vert == SquareVertEnum.TopRight)
            {
                return DirectionEnum.Up;
            }
            if (vert == SquareVertEnum.BottomRight)
            {
                return DirectionEnum.Right;
            }

            return DirectionEnum.Notready;
        }

        public static void SetDirection(DirectionEnum dir, ref int x, ref int y)
        {
            if (dir == DirectionEnum.Down)
            {
                y--;
            }
            else if (dir == DirectionEnum.Up)
            {
                y++;
            }
            else if (dir == DirectionEnum.Left)
            {
                x--;
            }
            else if (dir == DirectionEnum.Right)
            {
                x++;
            }

        }
    }


    [System.Serializable]
    public struct IntVector2
    {
        public int x, z;

        public IntVector2(int _x, int _z)
        {
            x = _x;
            z = _z;
        }

        public static IntVector2 operator +(IntVector2 a, IntVector2 b)
        {
            a.x += b.x;
            a.z += b.z;
            return a;
        }
    }

    public enum MazeDirection
    {
        North,
        East,
        South,
        West
    }

    public static class MazeDirections
    {
        public const int Count = 4;
        public static MazeDirection RandomValue
        {
            get
            {
                return (MazeDirection)Random.Range(0, Count);
            }
        }

        private static IntVector2[] vectors = {
               new IntVector2(0, 1),
               new IntVector2(1, 0),
               new IntVector2(0, -1),
               new IntVector2(-1, 0)
        };

        private static MazeDirection[] opposites = {
             MazeDirection.South,
             MazeDirection.West,
             MazeDirection.North,
             MazeDirection.East
        };

        private static Quaternion[] rotations = {
            Quaternion.identity,
            Quaternion.Euler(0f, 90f, 0f),
            Quaternion.Euler(0f, 180f, 0f),
            Quaternion.Euler(0f, 270f, 0f)
        };

        // public static IntVector2 MDToIntVector2(MazeDirection direction)
        // {
        //     return vectors[(int)direction];
        // }

        public static Quaternion ToRotation(this MazeDirection direction)
        {
            return rotations[(int)direction];
        }

        public static IntVector2 MDToIntVector2(this MazeDirection direction)
        {
            // somdDirection.MDToIntVector2()
            return vectors[(int)direction];
        }

        public static MazeDirection GetOpposite(this MazeDirection direction)
        {
            return opposites[(int)direction];
        }

    }

}