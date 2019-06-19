using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cr7_Level
{


    public class ApathfindingWithSimplifiedPath : PathFinding
    {
        protected override Vector3[] RetractPath(GridNode startnode, GridNode endnode)
        {
            var path = new List<GridNode>();
            var currentNode = endnode;

            while (startnode != currentNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            var wayPoints = SimplifyPath(path);
            Array.Reverse(wayPoints);
            return wayPoints;
        }

        protected Vector3[] SimplifyPath(List<GridNode> path)
        {
            var wayPoints = new List<Vector3>();
            var oldDirection = Vector2.zero;

            for (int i = 1; i < path.Count; i++)
            {
                var newDirection = new Vector2(path[i - 1].gridX - path[i].gridX, path[i - 1].gridY - path[i].gridY);
                if (newDirection != oldDirection)
                {
                    wayPoints.Add(path[i - 1].worldPosition);
                }
                oldDirection = newDirection;
            }
            return wayPoints.ToArray();
        }
    }
}