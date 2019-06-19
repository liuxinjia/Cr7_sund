using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_MMaze
{
    [System.Serializable]
    public struct IntVector2
    {
        public int x, y;

        public IntVector2(int _x, int _z)
        {
            x = _x;
            y = _z;
        }

        public static IntVector2 operator +(IntVector2 a, IntVector2 b)
        {
            a.x += b.x;
            a.y += b.y;
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

        private static Quaternion[] rotations_Y = {
            Quaternion.Euler(-90, 90.0f, 0f),
            Quaternion.Euler(0f ,90.0f, 0f),
            Quaternion.Euler(90.0f, 90.0f, 0f),
            Quaternion.Euler(180.0f, 90.0f, 0f)
        };

        private static Quaternion[] rotations_Z = {
            Quaternion.identity,
            Quaternion.Euler(0f, 90.0f, 0f),
            Quaternion.Euler(0f, 180f, 0f),
            Quaternion.Euler(0f, 270f, 0f)
        };


        public static string[] names = {
            "North",
            "East",
            "South",
            "West"
        };

        // public static IntVector2 MDToIntVector2(MazeDirection direction)
        // {
        //     return vectors[(int)direction];
        // }

        public static Quaternion ToRotation(this MazeDirection direction)
        {
            return rotations_Y[(int)direction];
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