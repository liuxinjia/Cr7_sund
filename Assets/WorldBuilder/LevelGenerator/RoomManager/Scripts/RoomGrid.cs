using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Level
{
    public class RoomGrid : Grid
    {
        public override GridNode NodeFromWorldPoint(Vector3 worldPosition)
        {
            return grids[(int)worldPosition.x, (int)worldPosition.y];
        }

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


    }
}