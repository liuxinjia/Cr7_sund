using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_MMaze
{
    public class MMazeGrid : Cr7_Level.Grid
    {
        MazeCellNode[,] cellGrids;

        public override List<GridNode> GetNeightBours(GridNode node)
        {
            var neightbours = new List<GridNode>();

            for (int k = 0; k < 4; k++)
            {
                MazeDirection dir = (MazeDirection)k;
                int x = dir.MDToIntVector2().x;
                int y = dir.MDToIntVector2().y;
                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                // int checkX = node.gridX + MagicNumbers.FourDirArray[k];
                // int checkY = node.gridY + MagicNumbers.FourDirArray[k + 1];

                if (checkX >= 0 && checkX < rMax && checkY >= 0 && checkY < cMax && grids[checkX, checkY] != null)
                {
                    if (cellGrids[node.gridX, node.gridY].edges[k].edgeType != EdgeType.wall)
                    {
                        grids[checkX, checkY].name = cellGrids[node.gridX, node.gridY].edges[k].name;
                        neightbours.Add(grids[checkX, checkY]);
                    }
                }

            }
            return neightbours;
        }

        public override void DebugInfo(GridNode node)
        {
            var cell = cellGrids[node.gridX, node.gridY];
            foreach (var item in cell.edges)
            {
                if (item.edgeType == EdgeType.wall)
                    print(item.edgeType + item.name);
            }
        }

        public override GridNode NodeFromWorldPoint(Vector3 _worldPosition)
        {
            // float percentX = Mathf.Clamp01((_worldPosition.x + (rMax - 1) * .5f * gridSizeX) / gridSizeX);
            // float percentY = Mathf.Clamp01((_worldPosition.y + (cMax - 1) * .5f * gridSizeX) / gridSizeX);

            // int x = Mathf.RoundToInt(percentX * (rMax ));
            // int y = Mathf.RoundToInt(percentY * (cMax));

            int x = Mathf.RoundToInt((_worldPosition.x + (rMax - 1) * .5f * gridSizeX) / gridSizeX);
            int y = Mathf.RoundToInt((_worldPosition.y + (cMax - 1) * .5f * gridSizeX) / gridSizeX);

            if (x < 0 || y < 0 || x >= rMax || y >= cMax)
            {
                Debug.Log("MMazeGrid Out of range " + _worldPosition.x + ", " + _worldPosition.y + ", ");
            }

            return grids[x, y];
        }

        public void AssignToGrids(MazeCell[,] cells, float _size, Vector3? _worldGridBootomLeft = null, Cr7_Level.TerrainType[] _walkableRegions = null)
        {
            int xLenth = cells.GetLength(0);
            int yLength = cells.GetLength(1);
            cellGrids = new MazeCellNode[xLenth, yLength];

            ReConstructGrid(cells, xLenth, yLength, _walkableRegions);
            InitGrid(cellGrids, _size, _size, _worldGridBootomLeft.GetValueOrDefault());

            //examine whether the nodeFromWorldPoint method validation
            // --------------------------------------------------
            // foreach (var item in grids)
            // {
            //     var node = NodeFromWorldPoint(item.worldPosition);
            //     if (node != item)
            //     {
            //         Debug.Log(node.gridX + ", " + node.gridY);
            //     }
            // }
        }

        void ReConstructGrid(MazeCell[,] cells, int xLenth, int yLength, Cr7_Level.TerrainType[] _walkableRegions)
        {
            if (_walkableRegions != null)
            {
                walkableRegions = _walkableRegions;


                foreach (var terrian in walkableRegions)
                {
                    walkableLayer |= terrian.terrianLayerMask;
                    walkableDictionary.Add((int)Mathf.Log(terrian.terrianLayerMask.value, 2), terrian.terrianPenality);
                }
            }


            for (int i = 0; i < xLenth; i++)
            {
                for (int j = 0; j < yLength; j++)
                {
                    Ray ray = new Ray(cells[i, j].node.worldPosition + Vector3.down * 50, Vector3.up);
                    RaycastHit hit;
                    int penality = 0;

                    if (Physics.Raycast(ray, out hit, 100, walkableLayer))
                    {
                        walkableDictionary.TryGetValue(hit.collider.gameObject.layer, out penality);
                    }

                    cells[i, j].node.movePenality = penality;
                    cells[i, j].node.walkable = true;
                    cells[i, j].node.gridX = i;
                    cells[i, j].node.gridY = j;

                    cellGrids[i, j] = cells[i, j].node;
                }
            }
        }

    }
}
