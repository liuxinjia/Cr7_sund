using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_MMaze
{
    public class MazeCellNode : GridNode
    {
        public IntVector2 coordinates;
        public MazeCellEdge[] edges;

        public MazeCellNode(IntVector2 _coord, Vector3 _worldPos, bool _walkable = true, int _penality = 0)
        {
            edges = new MazeCellEdge[MazeDirections.Count];
            coordinates = _coord;
            worldPosition = _worldPos;
            walkable = _walkable;
            movePenality = _penality;
        }
    }
}
