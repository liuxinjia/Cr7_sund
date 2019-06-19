using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_PCMaze
{
    public class MazeGrid : Cr7_Level.Grid
    {
        public override List<GridNode> GetNeightBours(GridNode node)
        {
            var neightbours = new List<GridNode>();

            for (int k = 0; k < 4; k++)
            {
                int checkX = node.gridX + MagicNumbers.FourDirArray[k];
                int checkY = node.gridY + MagicNumbers.FourDirArray[k + 1];

                if (checkX >= 0 && checkX < rMax && checkY >= 0 && checkY < cMax && grids[checkX, checkY] != null)
                {
                    neightbours.Add(grids[checkX, checkY]);
                }
            }
            return neightbours;
        }

        public override GridNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            worldPosition = worldPosition - worldGridBootomLeft;
            //1st: formula
            int x = Mathf.RoundToInt((worldPosition.x - gridHalfX) / gridSizeX);
            int y = Mathf.RoundToInt((worldPosition.y - gridHalfY) / gridSizeY);
            return grids[x, y];
        }
    }

}