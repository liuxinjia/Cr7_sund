using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Level
{
    public abstract class Grid : MonoBehaviour
    {
        public bool displayGridGizmos = true;
        protected TerrainType[] walkableRegions;
        protected float gridSizeX, gridSizeY;
        protected float gridHalfY, gridHalfX;
        protected int rMax, cMax;
        protected Vector3 worldGridBootomLeft;


        protected LayerMask walkableLayer;
        public GridNode[,] grids;
        protected Dictionary<int, int> walkableDictionary;
        public int MaxGridSize
        {
            get { return rMax * cMax; }
        }

        void Awake()
        {
            // mazeConstructor = GetComponent<MazeConstructor>();

            walkableDictionary = new Dictionary<int, int>();

        }

        public void FromDimension(int[,] mazeData, float _gridSizeX, float _gridSizeY, Vector3 _worldGridBootomLeft, bool showGizmos = false, TerrainType[] _walkableRegions = null)
        {
            InitGrid(mazeData, _gridSizeX, _gridSizeY, _worldGridBootomLeft);
            // worldGridBootomLeft = transform.position - new Vector3(gridSizeX * gridTolerance, 0, gridSizeX * gridTolerance);

            for (int x = 0; x < rMax; x++)
            {
                for (int y = 0; y < cMax; y++)
                {
                    // += new Vector3(SizeWidth * gridOffset, 0, SizeWidth * gridOffset)
                    var worldPoint = worldGridBootomLeft + (x * gridSizeX + gridHalfX) * Vector3.right + (y * gridSizeY + gridHalfY) * Vector3.up;
                    int movementPenality = 0;
                    bool walkable = mazeData[x, y] == 1 ? false : true;
                    if (mazeData[x, y] != 1)
                    {
                        Ray ray = new Ray(worldPoint + Vector3.down * 50, Vector3.up);
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit, 100, walkableLayer))
                        {
                            walkableDictionary.TryGetValue(hit.collider.gameObject.layer, out movementPenality);
                        }
                    }
                    grids[x, y] = new GridNode(walkable, worldPoint, x, y, movementPenality);
                }
            }

        }

        void InitGrid(int[,] mazeData, float _gridSizeX, float _gridSizeY, Vector3 _worldGridBootomLeft, bool showGizmos = false, TerrainType[] _walkableRegions = null)
        {
            gridSizeX = _gridSizeX;
            gridSizeY = _gridSizeY;
            gridHalfY = gridSizeY / 2;
            gridHalfX = gridSizeX * .5f;

            rMax = mazeData.GetUpperBound(0) + 1;
            cMax = mazeData.GetUpperBound(1) + 1;
            if (rMax == 1)
            {
                Debug.LogError("Please generate the mesh first");
                return;
            }
            grids = new GridNode[rMax, cMax];

            worldGridBootomLeft = _worldGridBootomLeft;
            displayGridGizmos = showGizmos;

            if (_walkableRegions != null)
            {
                walkableRegions = _walkableRegions;


                foreach (var terrian in walkableRegions)
                {
                    walkableLayer |= terrian.terrianLayerMask;
                    walkableDictionary.Add((int)Mathf.Log(terrian.terrianLayerMask.value, 2), terrian.terrianPenality);
                }
            }
        }

        public void InitGrid(GridNode[,] _grid, float _gridSizeX, float _gridSizeY, Vector3 _worldGridBootomLeft)
        {
            grids = _grid;
            rMax = grids.GetLength(0);
            cMax = grids.GetLength(1);
            gridSizeX = _gridSizeX;
            gridSizeY = _gridSizeY;
            gridHalfY = gridSizeY / 2;
            gridHalfX = gridSizeX * .5f;
            worldGridBootomLeft = _worldGridBootomLeft;
        }

        public abstract GridNode NodeFromWorldPoint(Vector3 worldPosition);

        public virtual List<GridNode> GetNeightBours(GridNode node)
        {
            var neightbours = new List<GridNode>();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 0) continue;

                    int checkX = node.gridX + x;
                    int checkY = node.gridY + y;

                    if (checkX >= 0 && checkX < rMax && checkY >= 0 && checkY < cMax && grids[checkX, checkY] != null)
                    {
                        neightbours.Add(grids[checkX, checkY]);
                    }
                }
            }

            return neightbours;
        }

        public virtual void DebugInfo(GridNode node)
        {

        }
    }

    [System.Serializable]
    public class TerrainType
    {
        public LayerMask terrianLayerMask;
        public int terrianPenality;
    }

}