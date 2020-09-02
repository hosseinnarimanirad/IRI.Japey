using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IRI.Ket.DataStructure
{
    public class BinaryHeap<T>
    {
        public T[] values;

        private int pointer;

        public int Length
        {
            get { return this.pointer; }
        }

        Func<T, T, int> comparer;

        public BinaryHeap(T[] array, Func<T, T, int> comparer)
        {
            pointer = 0;

            values = new T[array.Length];

            values[pointer] = array[0];

            pointer++;

            this.comparer = comparer;

            while (pointer < array.Length)
            {
                this.Add(array[pointer]);
            }
        }

        private void Add(T value)
        {
            values[pointer] = value;

            int bubbleIndex = this.pointer;

            while (bubbleIndex != 0)
            {
                int parentIndex = (int)Math.Floor((bubbleIndex - 1) / 2.0);

                if (comparer(values[bubbleIndex], values[parentIndex]) > 0)
                {
                    T temp = values[parentIndex];

                    values[parentIndex] = values[bubbleIndex];

                    values[bubbleIndex] = temp;

                    bubbleIndex = parentIndex;
                }
                else
                    break;
            }

            this.pointer++;
        }

        public T ReleaseValue()
        {
            this.pointer--;

            T result = this.values[0];

            this.values[0] = this.values[this.pointer];

            int index, swapIndex;

            swapIndex = 0;

            do
            {
                index = swapIndex;

                if ((pointer - 1) >= 2 * index + 2)
                {
                    if (comparer(values[index], values[2 * index + 2]) < 0 ||
                       comparer(values[index], values[2 * index + 1]) < 0)
                    {
                        if (comparer(values[2 * index + 2], values[2 * index + 1]) > 0)
                        {
                            swapIndex = 2 * index + 2;
                        }
                        else
                        {
                            swapIndex = 2 * index + 1;
                        }
                    }
                    if (comparer(values[swapIndex], values[2 * index + 1]) < 0)
                    {
                        swapIndex = 2 * index + 1;
                    }
                }
                else if ((pointer - 1) >= 2 * index + 1)
                {
                    if (comparer(values[index], values[2 * index + 1]) < 0)
                    {
                        swapIndex = 2 * index + 1;
                    }
                }

                if (index != swapIndex)
                {
                    T temp = values[index];

                    values[index] = values[swapIndex];

                    values[swapIndex] = temp;
                }

            } while (index != swapIndex);

            return result;
        }

        public T SeekValue()
        {
            return this.values[0];
        }
    }
}
