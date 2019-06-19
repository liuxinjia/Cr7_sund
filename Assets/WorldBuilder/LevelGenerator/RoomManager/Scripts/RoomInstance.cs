using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;

namespace Cr7_Level
{
    public abstract class RoomInstance : MonoBehaviour
    {
        [HideInInspector]
        public MydirectionData.DirectionEnum roomDoors;
        [HideInInspector]
        public MydirectionData.DirectionEnum getToDestination;

        public GridNode lastNode;
        public bool isCurrent;


        public int gridCols = 15;
        public int gridRows = 15;
        // public Transform pathFinders;

        //playable zone
        protected Transform PlayableZone;
        protected Dictionary<PlayableZoneType, Transform> Zones;

        private void Start()
        {
            getToDestination = MydirectionData.DirectionEnum.Notready;
            isCurrent = true;

            InitRoomInfo();

            CreateNewRoom();
        }

        protected virtual void InitRoomInfo()
        {
            PlayableZone = GameObject.FindGameObjectWithTag(Tags.PlayableZone).transform;

            Zones = new Dictionary<PlayableZoneType, Transform>(PlayableZone.transform.childCount);
            for (int i = 0; i < PlayableZone.transform.childCount; i++)
            {
                Zones.Add((PlayableZoneType)i, PlayableZone.GetChild(i));
            }
        }
        protected abstract void CreateNewRoom();

        ///////////////////////////////// Maze PlayableZone////////////////////////////////////

        public void CreatePathFinder(Vector3 origin, Vector3 dest, Grid grid, PathFinding _pathFinding)
        {
            var pathFinder = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            pathFinder.name = "seeker1";
            // pathFinder.transform.parent = pathFinders;
            pathFinder.transform.position = origin;

            var target = GameObject.CreatePrimitive(PrimitiveType.Capsule).transform;
            target.name = "target1";
            target.transform.position = dest;
            // target.transform.parent = pathFinders;
            var pathfindUnit = pathFinder.AddComponent<PathFindUnit>();
            pathfindUnit.target = target;
            pathfindUnit.grid = grid;
            pathfindUnit.pathFinding = _pathFinding;
        }

        public void CratePlayableZone(PlayableZoneType zoneType, Vector3 zonePos)
        {
            var zone = GameObject.Instantiate(Zones[zoneType], zonePos, Quaternion.identity);
            zone.transform.parent = PlayableZone;
        }

    }
    public enum PlayableZoneType
    {
        PlayableHZone,
        PlayableVZone,
        PlayableTZone,
        PlayableXZone
    }
}