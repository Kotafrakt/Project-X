using System;
using System.Collections.Generic;

namespace Snowcap.Utilities
{
    /// <summary>
    /// Interface to implement if a class could be a heap item.
    /// </summary>
    /// <typeparam name="T">Type of the item. Used for comparing.</typeparam>
    public interface IHeapItem<T> : IComparable<T>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Heap storage of data
    /// This is a dynamic resizing variant using a List to prevent a big memory allocation.
    /// More information:
    /// "https://en.wikipedia.org/wiki/Heap_(data_structure)"
    /// </summary>
    /// <typeparam name="T">Type for the items used in the heap.</typeparam>
    public class DynamicHeap<T> where T : IHeapItem<T>
    {
        protected List<T> _items;

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        /// <summary>
        /// Constructor with a maximum size for the heap.
        /// </summary>
        /// <param name="inInitialCapacity">Max size of the heap storage.</param>
        public DynamicHeap(int inInitialCapacity)
        {
            _items = new List<T>(inInitialCapacity);
        }

        /// <summary>
        /// Add an item to the heap and sort it to the right position.
        /// </summary>
        /// <param name="inItem">Item to add.</param>
        public virtual void Add(T inItem)
        {
            inItem.HeapIndex = Count;
            _items.Add(inItem);
            SortUp(inItem);
        }

        /// <summary>
        /// Remove the first item from the heap.
        /// </summary>
        /// <returns>First item.</returns>
        public virtual T RemoveFirst()
        {
            if (Count == 0)
                return default(T);

            T firstItem = _items[0];
            T lastItem = _items[Count - 1];
            _items.RemoveAt(Count - 1);

            if (Count > 0)
            {
                _items[0] = lastItem;
                _items[0].HeapIndex = 0;
                SortDown(_items[0]);
            }

            return firstItem;
        }

        /// <summary>
        /// Update item with a better value and sort it up.
        /// </summary>
        /// <param name="inItem">Item to update.</param>
        public virtual void UpdateItem(T inItem)
        {
            SortUp(inItem);
        }

        /// <summary>
        /// Check if the heap contains a certain item
        /// </summary>
        /// <param name="inItem"></param>
        /// <returns></returns>
        public virtual bool Contains(T inItem)
        {
            return Equals(_items[inItem.HeapIndex], inItem);
        }

        /// <summary>
        /// Sort from bottom to top.
        /// </summary>
        /// <param name="inItem">Item to sort.</param>
        private void SortUp(T inItem)
        {
            int parentIndex = (inItem.HeapIndex - 1) / 2;

            while(true)
            {
                T parentItem = _items[parentIndex];
                if (inItem.CompareTo(parentItem) > 0)
                {
                    Swap(inItem, parentItem);
                }
                else
                {
                    break;
                }
                parentIndex = (inItem.HeapIndex - 1) / 2;
            }
        }

        /// <summary>
        /// Sort from top to bottom.
        /// </summary>
        /// <param name="inItem">Item to sort.</param>
        private void SortDown(T inItem)
        {
            while(true)
            {
                int childIndexLeft = inItem.HeapIndex * 2 + 1;
                int childIndexRight = inItem.HeapIndex * 2 + 2;
                int swapIndex = 0;

                if (childIndexLeft < Count)
                {
                    swapIndex = childIndexLeft;

                    if (childIndexRight < Count)
                    {
                        if (_items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0)
                        {
                            swapIndex = childIndexRight;
                        }
                    }

                    if (inItem.CompareTo(_items[swapIndex]) < 0)
                    {
                        Swap(inItem, _items[swapIndex]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Swap two items with each other.
        /// </summary>
        /// <param name="inItemA">First item to swap.</param>
        /// <param name="inItemB">Second item to swap.</param>
        private void Swap(T inItemA, T inItemB)
        {
            _items[inItemA.HeapIndex] = inItemB;
            _items[inItemB.HeapIndex] = inItemA;
            int itemAIndex = inItemA.HeapIndex;
            inItemA.HeapIndex = inItemB.HeapIndex;
            inItemB.HeapIndex = itemAIndex;
        }
    }
}
