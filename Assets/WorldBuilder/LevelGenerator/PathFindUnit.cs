using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Cr7_Level
{
    [RequireComponent(typeof(PathFinding))]
    public class PathFindUnit : MonoBehaviour
    {
        PathRequestManager pathManager;
        public PathFinding pathFinding;
        Vector3[] path;
        public Transform target;
        int targetIndex;
        public Grid grid;
        public float speed = 50f;


        void Start()
        {
            if (pathFinding == null)
                pathFinding = GetComponent<PathFinding>();
                
            StartFindPath();
        }

        public void StartFindPath()
        {
            PathRequestManager.RequestPath(transform.position, target.position, grid, pathFinding, PathFind);
        }

        void PathFind(Vector3[] newPath, bool pathFindSuccessful)
        {
            if (pathFindSuccessful)
            {
                path = newPath;
                StopCoroutine("FollowPath");
                StartCoroutine("FollowPath");
            }
        }

        IEnumerator FollowPath()
        {
            Vector3 currentWayPoint = path[0];

            while (true)
            {
                if (transform.position == currentWayPoint)
                {
                    targetIndex++;
                    if (targetIndex >= path.Length)
                    {
                        yield break;
                    }
                    currentWayPoint = path[targetIndex];
                }

                transform.position = Vector3.MoveTowards(transform.position, currentWayPoint, speed * Time.deltaTime);
                yield return null;
            }
        }

        public void OnDrawGizmos()
        {
            if (path != null)
            {
                for (int i = targetIndex; i < path.Length; i++)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(path[i], Vector3.one);

                    if (i == targetIndex)
                    {
                        Gizmos.DrawLine(transform.position, path[i]);
                    }
                }
            }
        }
    }

}