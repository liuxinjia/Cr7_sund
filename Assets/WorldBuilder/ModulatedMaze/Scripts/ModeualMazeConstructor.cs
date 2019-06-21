using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;

namespace Cr7_MMaze
{
    [RequireComponent(typeof(APathfindingEdge))]

    public class ModeualMazeConstructor : Cr7_Level.MazeConstructor
    {
        #region fields

        //general

        MazeCell[,] cells;

        public float ZoneSize = 1.0f;
        public MazeCell cellPrefab;

        public MazeDoor doorPrefab;
        [Range(0f, 1f)]
        public float doorProbability;

        public MazePassage passagePrefab;
        public MazeWall[] wallPrefabs;
        MazePassage EmptyPassage;
        MazeWall EmptyWall;

        //maze grid


        #endregion

        #region  methods

        public IntVector2 RandomCoordinates
        {
            get
            {
                return new IntVector2(Random.Range(0, rMax), Random.Range(0, cMax));
            }
        }

        public bool InBoxBounds(IntVector2 coordinate)
        {
            return coordinate.x >= 0 && coordinate.x < rMax && coordinate.y >= 0 && coordinate.y < cMax;
        }

        public MazeCell GetCell(IntVector2 coord)
        {
            return cells[coord.x, coord.y];
        }


        public void GenerateNewMaze(int _rMax, int _cMax)
        {
            InitInfo(_rMax, _cMax);

            var activeCells = new List<MazeCell>();
            DoFirstGeneration(activeCells);
            while (activeCells.Count > 0)
            {
                DoNextGeneration(activeCells);
            }


            var MMazeGrid = gameObject.AddComponent<MMazeGrid>();
            MMazeGrid.AssignToGrids(cells, ZoneSize);
            grid = MMazeGrid;
            gridNodes = grid.grids;

            SetDoors();
            CreateDoors(OnStartTrigger, OnGoalTrigger, "MMaze");

        }

        public void InitInfo(int _rMax, int _cMax)
        {
            rMax = _rMax;
            cMax = _cMax;

            roomInstance = GetComponent<MMRoomInstance>();
            pathfinding = gameObject.AddComponent<APathfindingEdge>();


            cells = new MazeCell[rMax, cMax];

            EmptyPassage = new GameObject("EmptyPassage").AddComponent<MazePassage>();
            EmptyWall = new GameObject("EmptyWall").AddComponent<MazeWall>();
            EmptyWall.transform.parent = transform;
            EmptyPassage.transform.parent = transform;
        }

        ///////////////////////////////// Mazes Genearator////////////////////////////////////

        void DoFirstGeneration(List<MazeCell> activeCells)
        {
            activeCells.Add(CreateCell(RandomCoordinates));
        }

        void DoNextGeneration(List<MazeCell> activeCells)
        {
            int currentIndex = activeCells.Count - 1;
            var currentCell = activeCells[currentIndex];
            if (currentCell.IsFullyInitialized)
            {
                activeCells.RemoveAt(currentIndex);
                return;
            }
            var direction = currentCell.RandomUninitializedDirection;
            var coordinate = currentCell.node.coordinates + direction.MDToIntVector2();

            if (InBoxBounds(coordinate))
            {
                var neighbour = GetCell(coordinate);
                if (neighbour == null)
                {
                    neighbour = CreateCell(coordinate);
                    CreatePassage(currentCell, neighbour, direction);
                    activeCells.Add(neighbour);
                }
                else
                {
                    CreateWall(currentCell, neighbour, direction);
                }
            }
            else
            {
                CreateWall(currentCell, null, direction);
            }

        }

        MazeCell CreateCell(IntVector2 coord)
        {
            var newCell = Instantiate(cellPrefab) as MazeCell;
            cells[coord.x, coord.y] = newCell;


            Vector3 worldPos;

            if (LevelManager.Instance.CurrentAxis == LevelManager.AxisType.Z_axis)
                worldPos = new Vector3(coord.x - rMax * .5f + .5f, coord.y - cMax * .5f + .5f, 0f);
            else
                worldPos = new Vector3(coord.x - rMax * .5f + .5f, 0f, coord.y - cMax * .5f + .5f);
            // var worldPos = new Vector3(coord.x + .5f, 0f, coord.z + .5f);
            worldPos *= ZoneSize;
            newCell.InitMazeCell(coord, worldPos);

            newCell.name = "Maze Cell " + coord.x + ", " + coord.y;
            newCell.transform.parent = transform;
            newCell.transform.localPosition = worldPos;
            newCell.transform.localScale = Vector3.one * ZoneSize;

            return newCell;
        }

        void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
        {
            MazePassage prefab = Random.value < doorProbability && direction != MazeDirection.North && direction != MazeDirection.South ? doorPrefab : passagePrefab;

            var passage = Instantiate(prefab) as MazePassage;
            passage.Initialize(cell, otherCell, direction, EdgeType.passage);
            passage = Instantiate(EmptyPassage) as MazePassage;
            passage.Initialize(otherCell, cell, direction.GetOpposite(), EdgeType.passage);
        }

        void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
        {
            var wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
            wall.Initialize(cell, otherCell, direction, EdgeType.wall);
            if (otherCell != null)
            {
                wall = Instantiate(EmptyWall) as MazeWall;
                wall.Initialize(otherCell, cell, direction.GetOpposite(), EdgeType.wall);
            }
        }

        ///////////////////////////////// Maze Doors Genearator////////////////////////////////////

        public override void SetDoors()
        {

            GetToTarget = new MazeCellNode[5];
            if ((roomInstance.roomDoors & MydirectionData.DirectionEnum.Down) != MydirectionData.DirectionEnum.Notready)
            {
                GetToTarget[(int)MydirectionData.DirToSquareVert(MydirectionData.DirectionEnum.Down)] = cells[0, 0].node;
            }
            if ((roomInstance.roomDoors & MydirectionData.DirectionEnum.Left) != MydirectionData.DirectionEnum.Notready)
            {
                GetToTarget[(int)MydirectionData.DirToSquareVert(MydirectionData.DirectionEnum.Left)] = cells[0, cMax - 1].node;

            }
            if ((roomInstance.roomDoors & MydirectionData.DirectionEnum.Right) != MydirectionData.DirectionEnum.Notready)
            {
                GetToTarget[(int)MydirectionData.DirToSquareVert(MydirectionData.DirectionEnum.Right)] = cells[rMax - 1, 0].node;

            }
            if ((roomInstance.roomDoors & MydirectionData.DirectionEnum.Up) != MydirectionData.DirectionEnum.Notready)
            {
                GetToTarget[(int)MydirectionData.DirToSquareVert(MydirectionData.DirectionEnum.Up)] = cells[rMax - 1, cMax - 1].node;

            }

            GetToTarget[(int)MydirectionData.SquareVertEnum.Middle] = cells[(int)rMax / 2, (int)cMax / 2].node;

        }



        #endregion



    }

}
