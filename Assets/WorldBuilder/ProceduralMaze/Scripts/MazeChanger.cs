using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_PCMaze
{
    public class MazeChanger : MeshChanger
    {
        public int gridRows = 15;
        public int gridCols = 17;
        PMRoomInstance room;

        public void ChangeMaze()
        {
            ChangeMesh();
            // room.CreatePathFinder();
        }

        public void ChangeMesh()
        {
            room = transform.parent.GetComponent<PMRoomInstance>();
            var newMesh = room.ProcGenerateNewMaze(gridRows, gridCols, false);

            UpdateMesh(newMesh);
        }
    }
}