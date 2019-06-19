using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNode : IHeapItem<GridNode>
{
    public bool walkable;
    public Vector3 worldPosition;
    public int gridX;
    public int gridY;
    public int gCost;
    public int hCost;
    public int movePenality;
    public GridNode parent;

    public string name;

    int heapIndex;
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }

    public int fCost
    {
        get { return gCost + hCost; }
    }

    public int CompareTo(GridNode nodeToCompare)
    {
        int compare = fCost.CompareTo(nodeToCompare.fCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }

    public GridNode(bool _walkable, Vector3? _worldPos, int _gridX, int _gridY, int _penality)
    {
        walkable = _walkable;
        worldPosition = _worldPos.GetValueOrDefault();
        gridX = _gridX;
        gridY = _gridY;
        movePenality = _penality;
    }

    public GridNode(){

    }
}
