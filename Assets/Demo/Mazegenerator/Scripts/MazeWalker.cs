using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cr7_Demo
{
    public class MazeWalker : MonoBehaviour
    {
        MazeCell currentCell;
        MazeDirection currentDirection;


        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(currentDirection);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(currentDirection.GetNextClockwise());
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Move(currentDirection.GetOpposite());
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Move(currentDirection.GetNextCounterClockwise());
            }
            else if (Input.GetKeyDown(KeyCode.Q))
            {
                Look(currentDirection.GetNextCounterClockwise());
            }
            else if (Input.GetKeyDown(KeyCode.E))
            {
                Look(currentDirection.GetNextClockwise());
            }
        }

        public void SetLocation(MazeCell cell)
        {
            if (currentCell != null)
            {
                currentCell.OnPlayerExited();
            }
            currentCell = cell;
            transform.localPosition = cell.transform.localPosition + Vector3.up * .5f;
            currentCell.OnPlayerEntered();
        }

        private void Move(MazeDirection direction)
        {
            var edge = currentCell.GetEdge(direction);
            if (edge is MazePassage)
            {
                SetLocation(edge.otherCell);
            }
        }

        void Look(MazeDirection direction)
        {
            transform.localRotation = direction.ToRotation();
            currentDirection = direction;
        }

    }
}
