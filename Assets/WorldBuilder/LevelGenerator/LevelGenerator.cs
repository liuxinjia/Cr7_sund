using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyData;

namespace Cr7_Level
{
    public class LevelGenerator : MonoBehaviour
    {
        public GameObject roomObject;

        public RoomConstructor roomConstructor;

        GameObject curRoom;

        void Start()
        {
            roomConstructor.ProcCreateLevels();
            roomConstructor.StartWithNewRoom();
        }


        void Update()
        {
            if (Input.GetKeyDown("r"))
            {//reload scene, for testing purposes
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        public RoomInstance CreateNewRoomInstance(RoomNode node)
        {
            if (curRoom == null)
            {
                curRoom = Instantiate(roomObject, Vector3.zero, Quaternion.identity);
                curRoom.name = "room01";
                curRoom.transform.parent = transform;
            }
            else
            {
                Destroy(curRoom);
                curRoom = Instantiate(roomObject, Vector3.zero, Quaternion.identity);
                curRoom.name = "room02";
                curRoom.transform.parent = transform;
            }

            var roomInstance = curRoom.GetComponent<RoomInstance>();
            SetRoomInstanceInfo(roomInstance, node);

            return roomInstance;
        }

        public void SetRoomInstanceInfo(RoomInstance roomInstance, RoomNode room)
        {
            if (roomConstructor.lastRoom != null)
                roomInstance.lastNode = roomConstructor.lastRoom;

            roomInstance.roomDoors |= room.doorTop != null ? MydirectionData.DirectionEnum.Up : MydirectionData.DirectionEnum.Notready;
            roomInstance.roomDoors |= room.doorBot != null ? MydirectionData.DirectionEnum.Down : MydirectionData.DirectionEnum.Notready;
            roomInstance.roomDoors |= room.doorRight != null ? MydirectionData.DirectionEnum.Right : MydirectionData.DirectionEnum.Notready;
            roomInstance.roomDoors |= room.doorLeft != null ? MydirectionData.DirectionEnum.Left : MydirectionData.DirectionEnum.Notready;
        }

    }
}