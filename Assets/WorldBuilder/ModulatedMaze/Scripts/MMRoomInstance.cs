using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cr7_Level;

namespace Cr7_MMaze
{
    [RequireComponent(typeof(ModeualMazeConstructor))]
    public class MMRoomInstance : Cr7_Level.RoomInstance
    {
        ModeualMazeConstructor mazeConstructor;

        protected override void InitRoomInfo()
        {
            base.InitRoomInfo();

            mazeConstructor = GetComponent<ModeualMazeConstructor>();

            PlayableZone.transform.position = mazeConstructor.transform.position;

        }

        protected override void CreateNewRoom()
        {
            mazeConstructor.GenerateNewMaze(gridRows, gridCols);
            // StartCoroutine(mazeConstructor.GenerateNewMaze());
        }
    }

}