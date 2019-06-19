using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Cr7_Level
{

    public class PathRequestManager : MonoBehaviour
    {
        public static int count = 0;
        Queue<PathRequest> pathRequestQueue;
        PathRequest currentPathRequest;
        static PathRequestManager instance;
        bool isProcessingPath;



        private void Awake()
        {
            instance = this;
            pathRequestQueue = new Queue<PathRequest>();
        }

        public static void RequestPath(Vector3 _start, Vector3 _end, Grid _grid, PathFinding _pathFinding, Action<Vector3[], bool> _callback)
        {
            var newRequest = new PathRequest(_start, _end, _grid, _pathFinding, _callback);
            instance.pathRequestQueue.Enqueue(newRequest);
            count++;
            instance.TryProcessNext();

        }

        void TryProcessNext()
        {
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                currentPathRequest = pathRequestQueue.Dequeue();
                count--;
                
                isProcessingPath = true;
                currentPathRequest.pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.grid);
            }
        }

        public void FinishProcessingPath(Vector3[] path, bool success)
        {
            currentPathRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }

    }

    struct PathRequest
    {
        public Vector3 pathStart;
        public Vector3 pathEnd;
        public Grid grid;
        public PathFinding pathfinding;
        public Action<Vector3[], bool> callback;

        public PathRequest(Vector3 _start, Vector3 _end, Grid _grid, PathFinding _pathFinding, Action<Vector3[], bool> _callback)
        {
            pathStart = _start;
            pathEnd = _end;
            grid = _grid;
            pathfinding = _pathFinding;
            callback = _callback;
        }
    }


}