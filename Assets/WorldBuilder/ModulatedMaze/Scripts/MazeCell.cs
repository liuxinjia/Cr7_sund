using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_MMaze
{
    public class MazeCell : MonoBehaviour
    {
        // public IntVector2 coordinates;
        // MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];
        int initializeEdgeCount;
        public MazeCellNode node;

        public bool IsFullyInitialized
        {
            get
            {
                return initializeEdgeCount == MazeDirections.Count;
            }
        }

        public MazeDirection RandomUninitializedDirection
        {
            get
            {
                int skips = Random.Range(0, MazeDirections.Count - initializeEdgeCount);

                for (int i = 0; i < MazeDirections.Count; i++)
                {
                    if (node.edges[i] == null)
                    {
                        if (skips == 0)
                        {
                            return (MazeDirection)i;
                        }
                        skips--;
                    }
                }
                //there should be unintialized edges remaining
                throw new System.InvalidOperationException("Maze cell has no uninitialized direction left");
            }
        }

        public void InitMazeCell(IntVector2 coord, Vector3 _worldPos)
        {
            node = new MazeCellNode(coord, _worldPos);
        }

        public MazeCellEdge GetEdge(MazeDirection direction)
        {
            return node.edges[(int)direction];
        }

        public void SetEdge(MazeDirection direction, MazeCellEdge edge)
        {
            node.edges[(int)direction] = edge;
            initializeEdgeCount++;
        }


    }
}