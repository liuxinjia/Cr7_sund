using System.Collections;
using System.Collections.Generic;
using System;


public class Heap<T> where T : IHeapItem<T>
{
    T[] items;
    int currentItemCount;

    public Heap(int maxHeapSize)
    {
        items = new T[maxHeapSize];
    }

    public void Add(T item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public T removeFirst()
    {
        T firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;

        return firstItem;

    }

    public bool Contains(T item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    public int Count
    {
        get { return currentItemCount; }
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }
            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void SortDown(T item)
    {
        while (true)
        {
            int lChildIndex = item.HeapIndex * 2 + 1;
            int rChildIndex = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (lChildIndex < currentItemCount)
            {
                swapIndex = lChildIndex;
                if (rChildIndex < currentItemCount)
                {
                    if (items[rChildIndex].CompareTo(items[lChildIndex]) > 0)
                    {
                        swapIndex = rChildIndex;
                    }
                }

                if (item.CompareTo(items[swapIndex]) > 0)
                {
                    Swap(item, items[swapIndex]);
                }
                else
                { break; }
            }
            else
            { break; }
        }
    }

    void Swap(T itemA, T itemB)
    {
        items[itemB.HeapIndex] = itemA;
        items[itemA.HeapIndex] = itemB;

        int index = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = index;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex { get; set; }
}