using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cr7_Level;

namespace Cr7_PCMaze
{
    [RequireComponent(typeof(ProceduralMazeConstructor))]
    public class PMRoomInstance : RoomInstance
    {
        ProceduralMazeConstructor mazeConstructor;
        // Cr7_Level.Grid grid;

        protected override void InitRoomInfo()
        {
            base.InitRoomInfo();

            mazeConstructor = GetComponent<ProceduralMazeConstructor>();

            PlayableZone.transform.position = mazeConstructor.transform.position;

        }

        protected override void CreateNewRoom()
        {
            ProcGenerateNewMaze(gridRows, gridCols, true);
        }

        public Mesh ProcGenerateNewMaze(int gridRows, int gridCols, bool createMaze)
        {
            var newMesh = mazeConstructor.ProcGenerateNewMaze(gridRows, gridCols, createMaze);
            // grid = mazeConstructor.grid;
            return newMesh;
        }

    }
}
