using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_PCMaze
{
    public class MazeDataGenerator : MonoBehaviour
    {
        // Start is called before the first frame update
        float placementThreshold;

        public void InitInfo(float _threshold)
        {
            placementThreshold = _threshold;
        }
        
        public int[,] FromDimensions(int sizeRows, int sizeCols)
        {
            var maze = new int[sizeRows, sizeCols];
            var rMax = maze.GetUpperBound(0);
            var cMax = maze.GetUpperBound(1);

            for (int i = 0; i <= rMax; i++)
            {
                for (int j = 0; j <= cMax; j++)
                {
                    if (i == 0 || j == 0 || i == rMax || j == cMax)
                    {
                        maze[i, j] = 1;
                    }
                    else if (i % 2 == 0 && j % 2 == 0)
                    {
                        if (Random.value > placementThreshold)
                        {
                            maze[i, j] = 1;

                            var a = Random.value < .5 ? 0 : (Random.value < .5 ? -1 : 1);
                            var b = a != 0 ? 0 : (Random.value < .5 ? -1 : 1);
                            maze[i + a, j + b] = 1;
                        }
                    }
                }
            }
            return maze;
        }
    }

}
