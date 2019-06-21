using UnityEngine;

namespace Cr7_Demo
{
    public class MazeCell : MonoBehaviour
    {
        public IntVector2 coordinates;
        public MazeRoom room;

        GameObject enemy;

        private MazeCellEdge[] edges = new MazeCellEdge[MazeDirections.Count];

        private int initializedEdgeCount;

        public bool IsFullyInitialized
        {
            get
            {
                return initializedEdgeCount == MazeDirections.Count;
            }
        }

        public MazeDirection RandomUninitializedDirection
        {
            get
            {
                int skips = Random.Range(0, MazeDirections.Count - initializedEdgeCount);
                for (int i = 0; i < MazeDirections.Count; i++)
                {
                    if (edges[i] == null)
                    {
                        if (skips == 0)
                        {
                            return (MazeDirection)i;
                        }
                        skips -= 1;
                    }
                }
                throw new System.InvalidOperationException("MazeCell has no uninitialized directions left.");
            }
        }

        public MazeCellEdge GetEdge(MazeDirection direction)
        {
            return edges[(int)direction];
        }

        public void SetEdge(MazeDirection direction, MazeCellEdge edge)
        {
            edges[(int)direction] = edge;
            initializedEdgeCount += 1;
        }

        // create a new room for the first cell and each time we spawn a door
        public void Initialize(MazeRoom room)
        {
            room.Add(this);
            transform.GetChild(0).GetComponent<Renderer>().material = room.settings.floorMaterial;

        }

        public void OnPlayerEntered()
        {
            room.Show();

            for (int i = 0; i < edges.Length; i++)
            {
                edges[i].OnPlayerEntered();
            }
        }

        public void OnPlayerExited()
        {
            room.Hide();

            for (int i = 0; i < edges.Length; i++)
            {
                edges[i].OnPlayerExited();
            }
        }

        public void Hide()
        {
            gameObject.SetActive(false);
            if (enemy != null)
            {
                enemy.SetActive(false);
            }
        }

        public void Show()
        {
            gameObject.SetActive(true);
            if (enemy != null)
            {
                enemy.SetActive(true);
            }

        }

        public void DestroyEnemy()
        {
            if (enemy != null)
            {
                Destroy(enemy);
            }
        }

        public void CreateEnemy(GameObject enemyPrefab)
        {
            if (enemy == null && Random.value < 0.1f)
            {
                enemy = Instantiate(enemyPrefab);
                enemy.name = "Enemy";
                enemy.transform.parent = transform;
                enemy.transform.localScale = new Vector3(0.5f, 0.4f, 0.5f);
                enemy.transform.localPosition = Vector3.up * .1f;
                // enemy.transform.localPosition =
                //       new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
            }
        }
    }
}