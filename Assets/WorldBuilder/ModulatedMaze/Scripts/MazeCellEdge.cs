using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;

namespace Cr7_MMaze
{
    public abstract class MazeCellEdge : MonoBehaviour
    {
        public MazeCell cell, otherCell;
        public MazeDirection direction;
        public EdgeType edgeType;

        public void Initialize(MazeCell _cell, MazeCell _otherCell, MazeDirection _direction, EdgeType _type)
        {
            cell = _cell;
            otherCell = _otherCell;
            direction = _direction;
            edgeType = _type;
            cell.SetEdge(direction, this);

            transform.name += direction.ToString();
            transform.parent = _cell.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = direction.ToRotation();
        }

    }

    public enum EdgeType
    {
        wall,
        passage,
    }

}