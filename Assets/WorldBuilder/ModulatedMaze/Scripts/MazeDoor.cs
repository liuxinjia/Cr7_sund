using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_MMaze
{
    public class MazeDoor : MazePassage
    {
        private MazeDoor OtherSideOfDoor
        {
            get
            {
                return otherCell.GetEdge(direction.GetOpposite()) as MazeDoor;
            }
        }

        // public override void Initialize(MazeCell _cell, MazeCell _otherCell, MazeDirection _direction, EdgeType _type)
        // {
        //     base.Initialize(_cell, _otherCell, direction, _type);

        //     GameObject newHinge = Instantiate(transform.GetChild(2).gameObject);
        //     newHinge.transform.parent = transform;

        //     var hinge = newHinge.transform;
        //     if (OtherSideOfDoor != null)
        //     {
        //         hinge.localScale = new Vector3(-1f, 1f, 1f);
        //         Vector3 pos = transform.GetChild(2).gameObject.transform.localPosition;
        //         pos.y = -pos.y;

        //         hinge.localPosition = pos;
        //     }
        // }
    }
}