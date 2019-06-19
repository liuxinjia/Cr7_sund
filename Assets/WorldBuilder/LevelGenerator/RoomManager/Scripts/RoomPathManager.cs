using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Level
{
    [RequireComponent(typeof(RoomConstructor))]
    [RequireComponent(typeof(RoomGrid))]
    [RequireComponent(typeof(PathFinding))]
    public class RoomPathManager : MonoBehaviour
    {
        RoomGrid _roomGrid;
        RoomConstructor _roomConstructor;
        PathFinding _pathFinding;

        int pathFound;
        Vector3[] validRoomPath;

        private void Awake()
        {
            _roomConstructor = GetComponentInChildren<RoomConstructor>();

            _pathFinding = GetComponent<PathFinding>();
            _roomGrid = GetComponent<RoomGrid>();
            _pathFinding = GetComponent<PathFinding>();
        }

        public void InitRoomPath(GridNode[,] _grid, float _gridSizeX, float _gridSizeY, Vector3 _worldGridBootomLeft)
        {
            _roomGrid.InitGrid(_grid, _gridSizeX, _gridSizeY, _worldGridBootomLeft);
        }

        public void StartFindRoomPath(HashSet<RoomNode> path)
        {
            int potentailDoors = _roomConstructor.potentailDoors;
            var target = new Vector3[potentailDoors];
            var origin = new Vector3();

            var doors = 0;
            foreach (var node in path)
            {
                if (node.roomType == RoomType.end)
                {
                    if (doors < potentailDoors)
                    {
                        target[doors] = new Vector3(node.gridX, node.gridY, 0);
                        doors++;
                    }
                }
                if (node.roomType == RoomType.origin)
                {
                    origin = new Vector3(node.gridX, node.gridY, 0);
                }
            }

            foreach (var targetPos in target)
                PathRequestManager.RequestPath(origin, targetPos, _roomGrid, _pathFinding, PathFind);

            StartCoroutine(ProcCreateNewRoom());

        }

        IEnumerator ProcCreateNewRoom()
        {
            yield return new WaitUntil(() => pathFound == _roomConstructor.potentailDoors);

            CreateNewRoom();
        }

        void PathFind(Vector3[] newPath, bool pathFindSuccessful)
        {
            if (pathFindSuccessful)
            {
                // Debug.Log("newpath " + newPath.Length);
                validRoomPath = newPath;

                foreach (var item in newPath)
                {   
                    // var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                    // sphere.transform.position = new Vector3(item.x * _roomConstructor.gridSizeX, item.y * _roomConstructor.gridSizeY, 0);
                }
                pathFound++;
            }
        }

        public void CreateNewRoom()
        {
            // Debug.Log("All path found successfully");
        }

    }
}
