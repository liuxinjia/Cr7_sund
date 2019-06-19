using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyData;

namespace Cr7_Level
{
    public class RoomDataGenerator : MonoBehaviour
    {
        [HideInInspector]
        public HashSet<RoomNode> path;
        public RoomNode[,] rooms;
        List<Vector2> takenPositions;

        int gridSizeX;
        int gridSizeY;
        int PotentailDoors;

        public void FromDimension(int gridHalfextentsX, int gridHalfextentsY, int numberOfRooms, int _PotentailDoors)
        {
            bool walkable = true;

            rooms = new RoomNode[gridHalfextentsX * 2, gridHalfextentsY * 2];
            path = new HashSet<RoomNode>();
            takenPositions = new List<Vector2>();


            gridSizeX = gridHalfextentsX;
            gridSizeY = gridHalfextentsY;
            PotentailDoors = _PotentailDoors;

            takenPositions.Insert(0, Vector2.zero);
            rooms[gridHalfextentsX, gridHalfextentsY] = new RoomNode(walkable, gridHalfextentsX, gridHalfextentsY, new Vector3(gridHalfextentsX, gridHalfextentsY, 0));
            rooms[gridHalfextentsX, gridHalfextentsY].roomType = RoomType.origin;
            path.Add(rooms[gridHalfextentsX, gridHalfextentsY]);


            Vector2 checkPos = Vector2.zero;
            //magic numbers 
            float randomCompare = .2f, randomCompareStart = .2f, randomCompareEnd = .01f;

            for (int i = 0; i < numberOfRooms - 1; i++)
            {
                float randomPerc = ((float)i) / ((float)numberOfRooms - 1);
                randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
                checkPos = NewPosition();
                if (NumberOfNeighbors(checkPos) > 1 && Random.value > randomCompare)
                {
                    int iteration = 0;
                    do
                    {
                        checkPos = SelectiveNewPosition();
                        iteration++;
                    } while (NumberOfNeighbors(checkPos) > 1 && iteration < 30);
                    if (iteration >= 50)
                        Debug.Log("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos));

                }
                int gridX = (int)checkPos.x + gridHalfextentsX, gridY = (int)checkPos.y + gridHalfextentsY;
                rooms[gridX, gridY] = new RoomNode(walkable, gridX, gridY, new Vector3(gridX, gridY, 0));
                takenPositions.Insert(0, checkPos);
                path.Add(rooms[gridX, gridY]);
            }

        }

        public void ModifyRooms()
        {
            var roomTypeDivision = new Vector2(3, 3);
            var roomTypeX = new float[(int)roomTypeDivision.x - 1];
            var roomTypeY = new float[(int)roomTypeDivision.y - 1];
            for (int i = 0; i < roomTypeDivision.x - 1; i++)
            {
                roomTypeX[i] = gridSizeX * ((float)(i + 1) / roomTypeDivision.x);
            }
            for (int i = 0; i < roomTypeDivision.y - 1; i++)
            {
                roomTypeY[i] = gridSizeY * ((float)(i + 1) / roomTypeDivision.y);
            }

            foreach (var wayPoint in path)
            {
                SetRoomDoors(wayPoint);
                if (wayPoint.roomType == RoomType.origin)
                {
                    continue;
                }
                if (wayPoint.gridX == 0 || wayPoint.gridY == 0 || wayPoint.gridX == gridSizeX * 2 - 1 || wayPoint.gridY == gridSizeY * 2 - 1)
                {
                    if (PotentailDoors > 0)
                    {
                        wayPoint.roomType = RoomType.end;
                        PotentailDoors--;
                    }
                    continue;
                }
                float percentX = wayPoint.gridX % gridSizeX;
                float percentY = wayPoint.gridY % gridSizeY;
                var temp = 1;

                for (int i = 0; i < roomTypeDivision.x - 1; i++)
                {
                    if (percentX <= roomTypeX[i])
                    {
                        temp = i + 1;
                        break;
                    }
                }
                for (int i = 0; i < roomTypeDivision.y - 1; i++)
                {
                    if (percentX <= roomTypeY[i])
                    {
                        temp = Mathf.Max(temp, i + 1);
                        break;
                    }
                }

                wayPoint.roomType = (RoomType)temp;
            }

        }

        void SetRoomDoors(RoomNode node)
        {
            int x = node.gridX, y = node.gridY;

            if (y - 1 >= 0)
            {
                node.doorBot = rooms[x, y - 1];
            }
            if (y + 1 < gridSizeY * 2)
            {
                node.doorTop = rooms[x, y + 1];
            }
            if (x - 1 >= 0)
            {
                node.doorLeft = rooms[x - 1, y];
            }
            if (x + 1 < gridSizeX * 2)
            {
                node.doorRight = rooms[x + 1, y];
            }
        }

        Vector2 NewPosition()
        {
            int x = 0, y = 0;
            var checkingPos = Vector2.zero;

            do
            {
                int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                x = (int)takenPositions[index].x;
                y = (int)takenPositions[index].y;
                var a = Random.value < .5f ? 0 : (Random.value < .5f ? -1 : 1);
                var b = a != 0 ? 0 : (Random.value < .5f ? -1 : 1);

                x += a;
                y += b;
                checkingPos = new Vector2(x, y);
            } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);


            return checkingPos;
        }

        Vector2 SelectiveNewPosition()
        {
            int index = 0, inc = 0;
            int x = 0, y = 0;
            Vector2 checkingPos = Vector2.zero;

            do
            {
                inc = 0;
                do
                {
                    index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                    inc++;
                } while (NumberOfNeighbors(takenPositions[index]) > 1 && inc < 30);
                if (inc == 100)
                { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
                    // print("Error: could not find position with only one neighbor");
                    Debug.Log("Error: could not find position with only one neighbor");
                }

                x = (int)takenPositions[index].x;
                y = (int)takenPositions[index].y;
                var a = Random.value < .5f ? 0 : (Random.value < .5f ? -1 : 1);
                var b = a != 0 ? 0 : (Random.value < .5f ? -1 : 1);

                x += a;
                y += b;
                checkingPos = new Vector2(x, y);
            } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

            return checkingPos;
        }

        int NumberOfNeighbors(Vector2 checkingPos)
        {
            int neighbor = 0;

            if (takenPositions.Contains(checkingPos + Vector2.right))
            {
                neighbor++;
            }
            if (takenPositions.Contains(checkingPos + Vector2.left))
            {
                neighbor++;
            }
            if (takenPositions.Contains(checkingPos + Vector2.up))
            {
                neighbor++;
            }
            if (takenPositions.Contains(checkingPos + Vector2.down))
            {
                neighbor++;
            }

            return neighbor;
        }

    }
}
