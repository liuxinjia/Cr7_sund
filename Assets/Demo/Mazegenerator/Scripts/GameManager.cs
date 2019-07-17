using UnityEngine;
using System.Collections;

namespace Cr7_Demo
{
    public class GameManager : MonoBehaviour
    {

        public Maze mazePrefab;

        private Maze mazeInstance;

        public MazeWalker walkerPrefab;
        private MazeWalker walkerInstance;

        public bool stepBystep = false;


        private void Start()
        {
            BeginGame();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
        }

        private void BeginGame()
        {
            mazeInstance = Instantiate(mazePrefab) as Maze;
            walkerInstance = Instantiate(walkerPrefab) as MazeWalker;


            Camera.main.clearFlags = CameraClearFlags.Skybox;
            Camera.main.rect = new Rect(0f, 0f, 1f, 1f);

            if (stepBystep)
                StartCoroutine(mazeInstance.GenerateStep());
            else
                mazeInstance.GenerateResult();

            walkerInstance.SetLocation(mazeInstance.GetCell(mazeInstance.RandomCoordinates));


            Camera.main.clearFlags = CameraClearFlags.Depth;
            Camera.main.rect = new Rect(0f, 0f, .5f, .5f);


        }

        private void RestartGame()
        {
            StopAllCoroutines();
            Destroy(mazeInstance.gameObject);
            if (walkerInstance != null)
            {
                Destroy(walkerInstance.gameObject);
            }
            BeginGame();
        }
    }

}