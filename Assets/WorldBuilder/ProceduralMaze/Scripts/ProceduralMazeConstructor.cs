using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;

namespace Cr7_PCMaze
{
    [RequireComponent(typeof(Cr7_Level.ApathfindingWithSimplifiedPath))]
    public class ProceduralMazeConstructor : Cr7_Level.MazeConstructor
    {
        #region fields

        ///////////////////////////////// General////////////////////////////////////

        //General


        public int[,] data { get; private set; }
        // PMRoomInstance roomInstance;

        //maze mesh Generator;
        [HideInInspector]
        public GameObject ProcMaze;
        MazeMeshGenerator meshGenerator;

        //maze data Generator
        public float PlacementThresold = .1f;

        //grid
        [HideInInspector]

        public Cr7_Level.TerrainType[] walkableRegions;

        public float SizeWidth = 8.0f;
        public float SizeHeight = 8.0f;

        //Maze Potential Doors
        int successfulPath;

        int targetPathCount;




        #endregion

        #region  Methods

        public Mesh ProcGenerateNewMaze(int gridRows, int gridCols, bool createMaze)
        {
            rMax = gridRows;
            cMax = gridCols;
            roomInstance = GetComponent<PMRoomInstance>();
            pathfinding = GetComponent<Cr7_Level.ApathfindingWithSimplifiedPath>();
            //there is something wrong using monobehaviour with new methods
            // grid = new MazeGrid();
            grid = gameObject.AddComponent<MazeGrid>();

            if (rMax % 2 == 0 && cMax % 2 == 0)
            {
                Debug.LogError("Odd numbers work better for dungeon size");
            }

            var newMesh = GenerateNewMaze(createMaze);

            var gridBootomLeft = transform.position;
            grid.FromDimension(data, SizeWidth, SizeHeight, gridBootomLeft);
            gridNodes = grid.grids;

            SetDoors();
            CreateDoors(OnStartTrigger, OnGoalTrigger);

            return newMesh;
        }

        public Mesh GenerateNewMaze(bool createMaze)
        {
            var dataGenerator = new MazeDataGenerator();
            meshGenerator = new MazeMeshGenerator();

            // data = new int[,] { };

            dataGenerator.InitInfo(PlacementThresold);
            data = dataGenerator.FromDimensions(rMax, cMax);


            var newMesh = AssignToMazeMesh();

            if (createMaze)
                CreateNewMaze(newMesh);

            return newMesh;
        }

        Mesh AssignToMazeMesh()
        {
            meshGenerator.InitInfo(SizeWidth, SizeHeight);
            return meshGenerator.FromData(data);
        }

        void CreateNewMaze(Mesh newMesh)
        {
            ProcMaze = new GameObject();
            ProcMaze.transform.parent = this.transform;
            ProcMaze.transform.position = Vector3.zero;
            ProcMaze.name = "Procedural maze";
            ProcMaze.tag = "Generated";
            ProcMaze.layer = LayerMask.NameToLayer("Obstacle");

            var mf = ProcMaze.AddComponent<MeshFilter>();
            mf.mesh = newMesh;

            var mc = ProcMaze.AddComponent<MeshCollider>();
            mc.sharedMesh = mf.mesh;

            var mr = ProcMaze.AddComponent<MeshRenderer>();
            mr.materials = new Material[2] { mazeGroundMat, mazeWallMat };

            //scripts
            var mazeChanger = ProcMaze.AddComponent<MazeChanger>();
        }


        ///////////////////////////////// Maze Doors Genearator////////////////////////////////////

        public override void SetDoors()
        {
            GetToTarget = new GridNode[5];

            if ((roomInstance.roomDoors & (MydirectionData.DirectionEnum.Left | MydirectionData.DirectionEnum.Down)) != MydirectionData.DirectionEnum.Notready)
            {
                for (int i = 0; i < rMax; i++)
                {
                    if ((roomInstance.roomDoors & MydirectionData.DirectionEnum.Down) != MydirectionData.DirectionEnum.Notready)
                    {
                        for (int j = 0; j < cMax; j++)
                        {
                            if (data[i, j] == 0)
                            {
                                GetToTarget[(int)MydirectionData.DirToSquareVert(MydirectionData.DirectionEnum.Down)] = gridNodes[i, j];
                                break;
                            }
                        }
                    }
                    if ((roomInstance.roomDoors & MydirectionData.DirectionEnum.Left) != MydirectionData.DirectionEnum.Notready)
                    {
                        for (int j = cMax; j > 0; j--)
                        {
                            if (data[i, j - 1] == 0)
                            {
                                GetToTarget[(int)MydirectionData.DirToSquareVert(MydirectionData.DirectionEnum.Left)] = gridNodes[i, j - 1];
                                break;
                            }
                        }
                    }
                }
            }
            if ((roomInstance.roomDoors & (MydirectionData.DirectionEnum.Right | MydirectionData.DirectionEnum.Up)) != MydirectionData.DirectionEnum.Notready)
            {
                for (int i = rMax; i > 0; i--)
                {
                    if ((roomInstance.roomDoors & MydirectionData.DirectionEnum.Right) != MydirectionData.DirectionEnum.Notready)
                    {
                        for (int j = 0; j < cMax; j++)
                        {
                            if (data[i - 1, j] == 0)
                            {
                                GetToTarget[(int)MydirectionData.DirToSquareVert(MydirectionData.DirectionEnum.Right)] = gridNodes[i - 1, j];
                                break;
                            }
                        }
                    }
                    if ((roomInstance.roomDoors & MydirectionData.DirectionEnum.Up) != MydirectionData.DirectionEnum.Notready)
                    {
                        for (int j = cMax; j > 0; j--)
                        {
                            if (data[i - 1, j - 1] == 0)
                            {
                                GetToTarget[(int)MydirectionData.DirToSquareVert(MydirectionData.DirectionEnum.Up)] = gridNodes[i - 1, j - 1];
                                break;
                            }
                        }
                    }
                }
            }

            for (int i = rMax / 2; i < rMax; i++)
            {
                for (int j = cMax / 2; j < cMax; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        int x = MagicNumbers.FourDirArray[k] + i;
                        int y = MagicNumbers.FourDirArray[k + 1] + j;

                        if (data[x, y] == 0)
                        {
                            GetToTarget[(int)MydirectionData.SquareVertEnum.Middle] = gridNodes[x, y];
                            return;
                        }
                    }
                }
            }

        }

        void OnDrawGizmos()
        {
            if (displayMazeDebug && gridNodes.Length > 0)
            {
                foreach (var node in gridNodes)
                {
                    if (node == null) continue;
                    int x = node.gridX, y = node.gridY;

                    int x_After = grid.NodeFromWorldPoint(node.worldPosition).gridX;
                    int y_After = grid.NodeFromWorldPoint(node.worldPosition).gridY;
                    if (x != x_After || y != y_After)
                    {
                        Debug.Log("error: " + x + ", " + y);
                        Debug.Log("afror: " + x_After + ", " + y_After);
                    }
                    // float randHeight = Random.Range(0.8f, 1.8f);
                    Gizmos.color = (node).walkable ? Color.red : Color.gray;

                    Gizmos.DrawCube(node.worldPosition, new Vector3(SizeWidth, SizeHeight, SizeWidth));
                }
            }
        }


        #endregion

    }

}