using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;


namespace Cr7_Level
{
    // [RequireComponent(typeof(MazeDataGenerator))]
    // [RequireComponent(typeof(MazeMeshGenerator))]
    [RequireComponent(typeof(ApathfindingWithSimplifiedPath))]
    public abstract class MazeConstructor : MonoBehaviour
    {

        #region fields

        //general
        protected int rMax, cMax;
        public bool displayMazeDebug = false;
        protected RoomInstance roomInstance;

        //maze generator
        [SerializeField] protected Material mazeGroundMat;
        [SerializeField] protected Material mazeWallMat;
        [SerializeField] protected Material reachedTriggerMat;


        //grid
        [HideInInspector]
        public Grid grid;
        protected GridNode[,] gridNodes;


        //Maze Potential Doors
        protected PathFinding pathfinding;
        int successfulPath;
        protected GridNode[] GetToTarget;
        int targetPathCount;

        //Maze Zone
        ZoneType[,] validMazeZone;

        #endregion

        #region  methods

        ///////////////////////////////// Maze Doors Genearator////////////////////////////////////

        public abstract void SetDoors();

        public void CreateDoors(TriggerEventHandler startCallback, TriggerEventHandler reachCallback, string name = null)
        {
            bool hadStart = false;
            var targetList = new List<Vector3>();
            var start = new Vector3();

            for (int i = 0; i < 4; i++)
            {
                if (GetToTarget[i] != null)
                {
                    if (roomInstance.lastNode != null && roomInstance.lastNode == GetToTarget[i])
                    {
                        start = GetToTarget[i].worldPosition;
                        PlaceStartTrigger(start, startCallback);
                        hadStart = true;
                    }
                    else
                    {
                        var pos = GetToTarget[i].worldPosition;

                        // var pos = GetToTarget[i].worldPosition + new Vector3(SizeWidth * gridOffset, 0, SizeWidth * gridOffset);
                        PlaceReachTrigger(pos, reachCallback);
                        targetList.Add(pos);
                    }
                }

            }

            if (!hadStart)
            {
                start = GetToTarget[4].worldPosition;
                PlaceStartTrigger(start, startCallback);
            }

            targetPathCount = targetList.Count;
            PathFinder(targetList, start);

        }

        void PlaceStartTrigger(Vector3 position, TriggerEventHandler callback)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);

            go.transform.position = position;
            go.name = "Start trigger";

            go.GetComponent<BoxCollider>().isTrigger = true;
            go.GetComponent<MeshRenderer>().sharedMaterial = reachedTriggerMat;

            var tc = go.AddComponent<TriggerEventRouter>();
            tc.callback = callback;
        }

        void PlaceReachTrigger(Vector3 position, TriggerEventHandler callback)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Cube);

            go.transform.position = position;
            go.name = "Reached trigger";

            go.GetComponent<BoxCollider>().isTrigger = true;
            go.GetComponent<MeshRenderer>().sharedMaterial = reachedTriggerMat;

            var tc = go.AddComponent<TriggerEventRouter>();
            tc.callback = callback;
        }

        protected virtual void OnStartTrigger(GameObject trigger, GameObject other)
        {
            Debug.Log("Do you want to Go Back");
        }

        protected virtual void OnGoalTrigger(GameObject trigger, GameObject other)
        {
            Debug.Log("Goal");

            var gridNode = grid.NodeFromWorldPoint(trigger.transform.position);

            for (int i = 0; i < 4; i++)
            {
                if (gridNode == GetToTarget[i])
                {
                    roomInstance.getToDestination = MydirectionData.SquareVertToDir((MydirectionData.SquareVertEnum)i);
                    break;
                }
            }
        }



        ///////////////////////////////// Maze Zones Genearator////////////////////////////////////

        void PathFinder(List<Vector3> targetList, Vector3 start)
        {
            validMazeZone = new ZoneType[rMax, cMax];
            for (int i = 0; i < rMax; i++)
            {
                for (int j = 0; j < cMax; j++)
                { validMazeZone[i, j] = ZoneType.empty; }
            }

            foreach (var target in targetList)
            {
                roomInstance.CreatePathFinder(start, target, grid, pathfinding);

                Cr7_Level.PathRequestManager.RequestPath(start, target, grid, pathfinding, PathFind);
            }

        }

        void PathFind(Vector3[] newPath, bool pathFindSuccessful)
        {
            if (pathFindSuccessful)
            {
                // CreateValidRoom(newPath);

                successfulPath++;

                var node = grid.NodeFromWorldPoint(newPath[0]);
                validMazeZone[node.gridX, node.gridY] |= ZoneType.turn;
                for (int i = 1; i < newPath.Length; i++)
                {
                    CreatePZone(newPath[i - 1], newPath[i]);
                }

                if (successfulPath >= targetPathCount)
                {

                    CreateXone();


                    for (int i = 0; i < rMax; i++)
                    {
                        for (int j = 0; j < cMax; j++)
                        {
                            if (validMazeZone[i, j] == ZoneType.empty)
                                continue;
                            if (validMazeZone[i, j] == ZoneType.random)
                            {
                                roomInstance.CratePlayableZone(PlayableZoneType.PlayableXZone, gridNodes[i, j].worldPosition);
                            }

                            if ((validMazeZone[i, j] & ZoneType.h_Tunel) != ZoneType.empty)
                            {
                                if (validMazeZone[i, j] == ZoneType.h_Tunel)
                                {
                                    roomInstance.CratePlayableZone(PlayableZoneType.PlayableHZone, gridNodes[i, j].worldPosition);
                                }
                                else if ((validMazeZone[i, j] & ZoneType.v_Tunel) != ZoneType.empty)
                                {
                                    roomInstance.CratePlayableZone(PlayableZoneType.PlayableTZone, gridNodes[i, j].worldPosition);

                                }
                                else
                                {
                                    roomInstance.CratePlayableZone(PlayableZoneType.PlayableVZone, gridNodes[i, j].worldPosition);
                                }
                            }

                        }
                    }
                }
            }
        }

        void CreatePZone(Vector3 A, Vector3 B)
        {
            var nodeA = grid.NodeFromWorldPoint(A);
            var nodeB = grid.NodeFromWorldPoint(B);

            validMazeZone[nodeB.gridX, nodeB.gridY] |= ZoneType.turn;

            int distance_X = Mathf.Abs(nodeA.gridX - nodeB.gridX);
            int distance_Y = Mathf.Abs(nodeA.gridY - nodeB.gridY);
            int min_Y = Mathf.Min(nodeA.gridY, nodeB.gridY);
            int min_X = Mathf.Min(nodeA.gridX, nodeB.gridX);

            for (int i = 1; i < distance_X; i += Random.Range(2, 3))
            {
                // var pos = gridNodes[min_X + i, nodeA.gridY].worldPosition;
                validMazeZone[min_X + i, nodeA.gridY] |= ZoneType.h_Tunel;
            }

            for (int i = 1; i < distance_Y; i += Random.Range(1, 3))
            {
                // var pos = gridNodes[nodeA.gridX, min_Y + i].worldPosition;
                validMazeZone[nodeA.gridX, min_Y + i] |= ZoneType.v_Tunel;
            }

        }

        void CreateXone()
        {
            for (int i = 0; i < rMax; i++)
            {
                for (int j = 0; j < cMax; j++)
                {
                    var node = gridNodes[i, j];
                    if (node.walkable && validMazeZone[i, j] == ZoneType.empty)
                    {
                        if (Random.value < .1f)
                        {
                            // validMazeZone[node.gridX, node.gridY] |= (x == 0 ? ZoneType.v_Tunel : ZoneType.h_Tunel);
                            validMazeZone[i, j] = ZoneType.random;
                        }
                    }

                }
            }
        }


        #endregion


        #region  enumType

        protected enum ZoneType
        {
            empty = 0,
            h_Tunel = 1,
            v_Tunel = 2,
            turn = 3,
            random = 4
        }

        #endregion
    }


}
