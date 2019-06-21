using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

namespace Cr7_Level
{
    public class PathFinding : MonoBehaviour
    {
        protected PathRequestManager pathRequestManager;
        protected Grid grid;

        public void StartFindPath(Vector3 startPos, Vector3 targetPos, Grid _grid)
        {
            grid = _grid;
            StartCoroutine(FindPath(startPos, targetPos));
        }

        public virtual IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
        {
            var LevelManagers = GameObject.FindGameObjectsWithTag(Tags.LevelManagerTag);
            if (LevelManagers.Length > 1) print("Tag Error: Can't contain too much LevelManager");
            pathRequestManager = GameObject.FindGameObjectWithTag(Tags.LevelManagerTag).GetComponent<PathRequestManager>();

            var sw = new Stopwatch();
            sw.Start();

            Vector3[] wayPoints = new Vector3[0];
            bool pathSuccess = false;

            var startNode = grid.NodeFromWorldPoint(startPos);
            var targetNode = grid.NodeFromWorldPoint(targetPos);

            if (startNode == null)
            {
                print(startPos);
            }

            if (targetNode == null)
            {
                print(targetPos);
            }

            if (startNode.walkable && targetNode.walkable)
            {
                var openSet = new Heap<GridNode>(grid.MaxGridSize);
                var closeSet = new HashSet<GridNode>();
                openSet.Add(startNode);
                while (openSet.Count > 0)
                {

                    var currentNode = openSet.removeFirst();
                    if (currentNode == targetNode)
                    {
                        sw.Stop();
                        print("Path found: " + sw.ElapsedMilliseconds + " ms");
                        pathSuccess = true;
                        break;
                    }
                    closeSet.Add(currentNode);

                    var neighbours = grid.GetNeightBours(currentNode);
                    foreach (var neighbour in neighbours)
                    {
                        if (!neighbour.walkable || closeSet.Contains(neighbour))
                            continue;
                        int newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighbour) + neighbour.movePenality;
                        if (newMovementCostToNeighbor < neighbour.gCost || !openSet.Contains(neighbour))
                        {
                            neighbour.gCost = newMovementCostToNeighbor;
                            neighbour.hCost = GetDistance(neighbour, targetNode);
                            neighbour.parent = currentNode;

                            if (!openSet.Contains(neighbour))
                            {
                                openSet.Add(neighbour);
                            }
                            else
                            {
                                openSet.UpdateItem(neighbour);
                            }
                        }
                    }
                }
            }

            yield return null;

            if (pathSuccess)
            {
                wayPoints = RetractPath(startNode, targetNode);
            }

            pathRequestManager.FinishProcessingPath(wayPoints, pathSuccess);
        }

        protected virtual Vector3[] RetractPath(GridNode startnode, GridNode endnode)
        {
            var path = new List<GridNode>();
            var currentNode = endnode;

            while (startnode != currentNode)
            {
                path.Add(currentNode);
                currentNode = currentNode.parent;
            }

            var wayPoints = new Vector3[path.Count];
            for (int i = 0; i < path.Count; i++)
                wayPoints[i] = path[i].worldPosition;
            Array.Reverse(wayPoints);
            return wayPoints;
        }

        protected int GetDistance(GridNode ndoeA, GridNode nodeB)
        {
            int distX = Mathf.Abs(ndoeA.gridX - nodeB.gridX);
            int distY = Mathf.Abs(ndoeA.gridY - nodeB.gridY);

            if (distX > distY)
                return 14 * distY + 10 * (distX - distY);
            else
                return 14 * distX + 10 * (distY - distX);
        }

    }

}