using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

namespace Cr7_MMaze
{
    public class APathfindingEdge : Cr7_Level.ApathfindingWithSimplifiedPath
    {

        public override IEnumerator FindPath(Vector3 startPos, Vector3 targetPos)
        {
            var LevelManagers = GameObject.FindGameObjectsWithTag(Tags.LevelManagerTag);
            if (LevelManagers.Length > 1) print("Tag Error: Can't contain too much LevelManager");
            pathRequestManager = GameObject.FindGameObjectWithTag(Tags.LevelManagerTag).GetComponent<Cr7_Level.PathRequestManager>();

            var sw = new Stopwatch();
            sw.Start();

            Vector3[] wayPoints = new Vector3[0];
            bool pathSuccess = false;

            var startNode = grid.NodeFromWorldPoint(startPos);
            var targetNode = grid.NodeFromWorldPoint(targetPos);

            if (startNode != null && targetNode != null)
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

                    // var obj = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                    // obj.transform.position = currentNode.worldPosition;
                    // obj.name = "OBJCapsule";
                    // obj.transform.parent = transform;

                    var neighbours = grid.GetNeightBours(currentNode);
                    foreach (var neighbour in neighbours)
                    {
                        if (closeSet.Contains(neighbour))
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

                                // obj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                                // obj.transform.position = neighbour.worldPosition;
                                // obj.name = "OBJSphere";
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

    }
}
