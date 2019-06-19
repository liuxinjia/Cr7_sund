using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Level
{
    public class RoomNode : GridNode
    {
        public RoomNode doorTop, doorBot, doorLeft, doorRight;
        public RoomType roomType;
        public RoomNode(bool _walkable, int _gridX, int _gridY, Vector3 _worldPos, int _penality = 0) :
        base(_walkable, _worldPos, _gridX, _gridY, _penality)
        {
            roomType = RoomType.start;
        }

    }

    public enum RoomType
    {
        origin, start, turn, end
    }
}