using UnityEngine;
using System.Collections.Generic;

namespace Cr7_Demo
{
    public class MazeRoom : ScriptableObject
    {
        public int settingsIndex;
        public MazeRoomSettings settings;

        List<MazeCell> cells = new List<MazeCell>();

        public void Add(MazeCell cell)
        {
            cell.room = this;
            cells.Add(cell);
        }

        public void Assimilate(MazeRoom room)
        {
            for (int i = 0; i < room.cells.Count; i++)
            {
                Add(room.cells[i]);
            }
        }

        public void Show()
        {
            foreach (var item in cells)
            {
                item.Show();
            }
        }

        public void Hide()
        {
            foreach (var item in cells)
            {
                item.Hide();
            }
        }

    }
}