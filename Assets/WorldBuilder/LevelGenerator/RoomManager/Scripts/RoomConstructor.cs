using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;

namespace Cr7_Level
{
    [RequireComponent(typeof(RoomDataGenerator))]
    [RequireComponent(typeof(RoomPathManager))]
    public class RoomConstructor : MonoBehaviour
    {
        #region  fields

        //Construct the rooms
        //-------------------
        //generate room data
        RoomDataGenerator _roomDataGenerator;
        public int potentailDoors = 2;
        public int numberOfRooms = 20;
        public int rMax, cMax;

        //generate room map sprite
        public Transform mapRoot;
        public GameObject roomMapSprit;

        public float gridSizeX, gridSizeY;

        //Room path finder and manager
        RoomPathManager _roomPathManger;

        //-------------------

        //interact with the level generator
        //-------------------

        //mangaer room instance and go to next one
        LevelGenerator _levelGenerator;
        HashSet<RoomNode> visitedSet;
        RoomNode currentRoom;
        public RoomNode lastRoom;
        //-------------------

        #endregion


        #region  Methods


        void Awake()
        {
            _levelGenerator = transform.GetComponentInParent<LevelGenerator>();

            visitedSet = new HashSet<RoomNode>();

            // _roomDataGenerator = new RoomDataGenerator();
            // _roomPathManger = new RoomPathManager();
            _roomDataGenerator = GetComponent<RoomDataGenerator>();
            _roomPathManger = GetComponent<RoomPathManager>();

            //note: these are half-extents
            rMax = 4;
            cMax = 4;
            gridSizeX = 16;
            gridSizeY = 8;
            numberOfRooms = 20;
            potentailDoors = 2;
            if (numberOfRooms >= (rMax * 2) * (cMax * 2))
            {
                numberOfRooms = (rMax * 2) * (cMax * 2);
            }
        }

        public void ProcCreateLevels()
        {
            if (numberOfRooms == 0)
                Debug.LogError("Number of room can't be zero");
            if (rMax == 0 || cMax == 0)
                Debug.LogError("RoomSize can't be zero");

            CreateRooms();
            DrawRooms();

            //Really nothing happened because commenting
            _roomPathManger.InitRoomPath(_roomDataGenerator.rooms, 1, 1, Vector3.zero);
            _roomPathManger.StartFindRoomPath(_roomDataGenerator.path);
        }

        void CreateRooms()
        {
            _roomDataGenerator.FromDimension(rMax, cMax, numberOfRooms, potentailDoors);
            _roomDataGenerator.ModifyRooms();
        }

        void DrawRooms()
        {
            var path = _roomDataGenerator.path;

            foreach (var room in path)
            {
                DrawMapper(room);
            }
        }

        void DrawMapper(RoomNode room)
        {
            Vector2 drawPos = new Vector2(room.gridX, room.gridY);
            drawPos.x *= gridSizeX;//aspect ratio of map sprite
            drawPos.y *= gridSizeY;
            //create map obj and assign its variables
            MapSpriteSelector mapper = Object.Instantiate(roomMapSprit, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
            mapper.type = room.roomType;
            mapper.up = room.doorTop != null;
            mapper.down = room.doorBot != null;
            mapper.right = room.doorRight != null;
            mapper.left = room.doorLeft != null;
            mapper.gameObject.transform.parent = mapRoot;
        }


        //room instantiating

        public void StartWithNewRoom()
        {
            var node = _roomDataGenerator.rooms[rMax, cMax];
            NextRoom(node);
        }

        public void NextRoom(RoomNode node)
        {
            var roomInstance = _levelGenerator.CreateNewRoomInstance(node);
            currentRoom = node;
            visitedSet.Add(node);

            StartCoroutine(FinishedRoom(node, roomInstance));
        }

        IEnumerator FinishedRoom(RoomNode curNode, RoomInstance curRoomInstance)
        {
            yield return new WaitUntil(() => curRoomInstance.getToDestination != MydirectionData.DirectionEnum.Notready);

            var curDir = curRoomInstance.getToDestination;
            int x = curNode.gridX, y = curNode.gridY;
            MydirectionData.SetDirection(curDir, ref x, ref y);

            if (x < rMax * 2 && x >= 0 && y >= 0 && y < cMax * 2 && _roomDataGenerator.rooms[x, y] != null)
            {
                lastRoom = curNode;
                var nextNode = _roomDataGenerator.rooms[x, y];

                if (true)
                {
                    curRoomInstance.isCurrent = false;
                    NextRoom(nextNode);
                }
            }
            else
            {
                StartCoroutine(FinishedRoom(currentRoom, curRoomInstance));
            }

        }

        #endregion
    }

}
