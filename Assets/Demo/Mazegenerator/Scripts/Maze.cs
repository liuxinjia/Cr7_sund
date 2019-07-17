using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Cr7_Demo
{
    public class Maze : MonoBehaviour
    {
        #region  fields
        //demo display
        public bool displayWholeMaze = true;

        public IntVector2 size;

        //perfabs
        public GameObject[] enemyPrefabs;
        public MazeCell cellPrefab;

        public float generationStepDelay;

        public MazePassage passagePrefab;

        [Range(0f, 1f)]
        public float doorProbability;
        public MazeDoor doorPrefab;

        private MazeCell[,] cells;
        public MazeWall[] wallPrefabs;

        List<MazeRoom> rooms = new List<MazeRoom>();

        public MazeRoomSettings[] roomSettings;

        public IntVector2 RandomCoordinates
        {
            get
            {
                return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
            }
        }

        public bool ContainsCoordinates(IntVector2 coordinate)
        {
            return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
        }

        #endregion

        #region  methods

        public MazeCell GetCell(IntVector2 coordinates)
        {
            return cells[coordinates.x, coordinates.z];
        }

        public IEnumerator GenerateStep()
        {
            WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
            cells = new MazeCell[size.x, size.z];
            List<MazeCell> activeCells = new List<MazeCell>();
            DoFirstGenerationStep(activeCells);
            while (activeCells.Count > 0)
            {
                yield return delay;
                DoNextGenerationStep(activeCells);
            }

            foreach (var item in rooms)
            {
                item.Hide();
            }
        }

        public void GenerateResult()
        {
            WaitForSeconds delay = new WaitForSeconds(generationStepDelay);
            cells = new MazeCell[size.x, size.z];
            List<MazeCell> activeCells = new List<MazeCell>();
            DoFirstGenerationStep(activeCells);
            while (activeCells.Count > 0)
            {
                DoNextGenerationStep(activeCells);
            }

            displayWholeMaze = false;
            if (!displayWholeMaze)
            {
                foreach (var item in rooms)
                {
                    item.Hide();
                }


                foreach (var item in cells)
                {
                    var renderers = item.transform.GetComponentsInChildren<Renderer>();
                    foreach (var renderer in renderers)
                    {
                        Color color = renderer.material.color;
                        Material material = new Material(Shader.Find("Custom/SplatShader"));
                        material.color = color;
                        renderer.material = material;
                        if (renderer.gameObject.GetComponent<SplatPainting>() == null)
                            renderer.gameObject.AddComponent<SplatPainting>();
                    }
                }
            }

        }

        private void DoFirstGenerationStep(List<MazeCell> activeCells)
        {
            MazeCell newCell = CreateCell(RandomCoordinates);
            newCell.Initialize(CreateRoom(-1));
            activeCells.Add(newCell);
        }

        private void DoNextGenerationStep(List<MazeCell> activeCells)
        {
            int currentIndex = activeCells.Count - 1;
            MazeCell currentCell = activeCells[currentIndex];
            if (currentCell.IsFullyInitialized)
            {
                activeCells.RemoveAt(currentIndex);
                return;
            }
            MazeDirection direction = currentCell.RandomUninitializedDirection;
            IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
            if (ContainsCoordinates(coordinates))
            {
                MazeCell neighbor = GetCell(coordinates);
                if (neighbor == null)
                {
                    neighbor = CreateCell(coordinates);
                    CreatePassage(currentCell, neighbor, direction);
                    activeCells.Add(neighbor);
                }
                else if (currentCell.room.settingsIndex == neighbor.room.settingsIndex)
                {
                    CreatePassageInSameRoom(currentCell, neighbor, direction);
                }
                else
                {
                    CreateWall(currentCell, neighbor, direction);
                }
            }
            else
            {
                CreateWall(currentCell, null, direction);
            }
        }

        private MazeCell CreateCell(IntVector2 coordinates)
        {
            MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
            cells[coordinates.x, coordinates.z] = newCell;
            newCell.coordinates = coordinates;
            newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
            newCell.transform.parent = transform;
            newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
            newCell.CreateEnemy(enemyPrefabs[Mathf.CeilToInt(Random.Range(0, enemyPrefabs.Length))]);
            return newCell;
        }

        private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
        {
            MazePassage prefab = Random.value < doorProbability ? doorPrefab : passagePrefab;
            MazePassage passage = Instantiate(prefab) as MazePassage;
            passage.Initialize(cell, otherCell, direction);
            passage = Instantiate(prefab) as MazePassage;
            if (passage is MazeDoor)
            {
                otherCell.Initialize(CreateRoom(cell.room.settingsIndex));
            }
            else
            {
                otherCell.Initialize(cell.room);
            }
            passage.Initialize(otherCell, cell, direction.GetOpposite());
        }

        private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
        {
            MazeWall wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
            wall.Initialize(cell, otherCell, direction);
            if (otherCell != null)
            {
                wall = Instantiate(wallPrefabs[Random.Range(0, wallPrefabs.Length)]) as MazeWall;
                wall.Initialize(otherCell, cell, direction.GetOpposite());
            }
        }

        MazeRoom CreateRoom(int indexToExclude)
        {
            var newRoom = ScriptableObject.CreateInstance<MazeRoom>();
            newRoom.settingsIndex = Random.Range(0, roomSettings.Length);
            //check whether picing the same index
            //if so, add one to the index and wrap around
            if (newRoom.settingsIndex == indexToExclude)
            {
                newRoom.settingsIndex = (newRoom.settingsIndex + 1) % roomSettings.Length;
            }

            newRoom.settings = roomSettings[newRoom.settingsIndex];
            rooms.Add(newRoom);
            return newRoom;
        }

        private void CreatePassageInSameRoom(MazeCell cell, MazeCell otherCell, MazeDirection direction)
        {
            MazePassage passage = Instantiate(passagePrefab) as MazePassage;
            passage.Initialize(cell, otherCell, direction);
            passage = Instantiate(passagePrefab) as MazePassage;
            passage.Initialize(otherCell, cell, direction.GetOpposite());

            if (cell.room != otherCell.room)
            {
                var roomToAssimilate = otherCell.room;
                cell.room.Assimilate(roomToAssimilate);
                rooms.Remove(roomToAssimilate);
                Destroy(roomToAssimilate);
            }
        }


        #endregion
    }

    #region enumType

    [System.Serializable]
    public struct IntVector2
    {

        public int x, z;

        public IntVector2(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public static IntVector2 operator +(IntVector2 a, IntVector2 b)
        {
            a.x += b.x;
            a.z += b.z;
            return a;
        }
    }

    #endregion
}