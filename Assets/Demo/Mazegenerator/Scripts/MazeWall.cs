using UnityEngine;

namespace Cr7_Demo
{
    public class MazeWall : MazeCellEdge
    {
        // public Transform wall;

        public override void Initialize(MazeCell cell, MazeCell otherCell, MazeDirection direction)
        {
            base.Initialize(cell, otherCell, direction);
            var wall = transform.GetChild(0);
            wall.GetComponent<Renderer>().material = cell.room.settings.wallMaterial;
        }
    }
}