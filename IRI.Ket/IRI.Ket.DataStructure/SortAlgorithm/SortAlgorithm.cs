// besmellahe rahmane rahim
// Allahomma ajjel le-valiyek al-faraj

using System;
using System.Collections.Generic;
using System.Text;

namespace IRI.Ket.DataStructure
{
    public static class SortAlgorithm
    {
        public static void BubbleSort<T>(T[] array, Func<T, T, int> comparer)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (comparer(array[j], array[j + 1]) > 0)
                    {
                        var temp = array[j];

                        array[j] = array[j + 1];

                        array[j + 1] = temp;
                    }
                }
            }
        }

        public static T[] Heapsort<T>(T[] array, SortDirection direction) where T : IComparable<T>
        {
            IBinaryHeap<T> heap;

            if (direction == SortDirection.Ascending)
            {
                heap = new MaxBinaryHeap<T>(array);
            }
            else
            {
                heap = new MinBinaryHeap<T>(array);
            }

            T[] result = new T[array.Length];

            int counter = 0;

            while (heap.Length != 0)
            {
                result[counter] = heap.ReleaseValue();

                counter++;
            }

            return result;
        }

        public static void Heapsort<T>(ref T[] array, SortDirection direction) where T : IComparable<T>
        {
            IBinaryHeap<T> heap;

            if (direction == SortDirection.Ascending)
            {
                heap = new MaxBinaryHeap<T>(array);
            }
            else
            {
                heap = new MinBinaryHeap<T>(array);
            }

            int counter = 0;

            while (heap.Length != 0)
            {
                array[counter] = heap.ReleaseValue();

                counter++;
            }
        }

        public static void Heapsort<T>(ref List<T> array, SortDirection direction) where T : IComparable<T>
        {
            IBinaryHeap<T> heap;

            if (direction == SortDirection.Ascending)
            {
                heap = new MaxBinaryHeap<T>(array.ToArray());
            }
            else
            {
                heap = new MinBinaryHeap<T>(array.ToArray());
            }

            int counter = 0;

            while (heap.Length != 0)
            {
                array[counter] = heap.ReleaseValue();

                counter++;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="comparer">the resulting integer must be -1, 0, or 1</param>
        /// <returns></returns>
        public static T[] Heapsort<T>(T[] array, Func<T, T, int> comparer)
        {
            BinaryHeap<T> heap = new BinaryHeap<T>(array, comparer);

            T[] result = new T[array.Length];

            int counter = 0;

            while (heap.Length != 0)
            {
                result[counter] = heap.ReleaseValue();

                counter++;
            }

            return result;
        }



        //******STANFORD: First H.W.
        public static T[] MergeSort<T>(T[] array, Func<T, T, int> comparer)
        {
            if (array.Length <= 1)
            {
                return array;
            }
            else
            {
                T[] tempFirstArray = new T[array.Length / 2];

                T[] tempSecondArray = new T[array.Length / 2 + array.Length % 2];

                Array.Copy(array, 0, tempFirstArray, 0, tempFirstArray.Length);

                Array.Copy(array, tempFirstArray.Length, tempSecondArray, 0, tempSecondArray.Length);

                T[] firstArray = MergeSort(tempFirstArray, comparer);

                T[] secondArray = MergeSort(tempSecondArray, comparer);

                return MergeArrayProperly(firstArray, secondArray, comparer);
            }
        }

        private static T[] MergeArrayProperly<T>(T[] firstArray, T[] secondArray, Func<T, T, int> comparer)
        {
            T[] result = new T[firstArray.Length + secondArray.Length];

            int firstIndex = 0, secondIndex = 0;

            for (int i = 0; i < result.Length; i++)
            {
                if (comparer(firstArray[firstIndex], secondArray[secondIndex]) > 0)
                {
                    result[i] = secondArray[secondIndex];

                    secondIndex++;

                    if (secondIndex == secondArray.Length)
                    {
                        Array.Copy(firstArray, firstIndex, result, i + 1, firstArray.Length - firstIndex);

                        break;
                    }
                }
                else
                {
                    result[i] = firstArray[firstIndex];

                    firstIndex++;

                    if (firstIndex == firstArray.Length)
                    {
                        Array.Copy(secondArray, secondIndex, result, i + 1, secondArray.Length - secondIndex);

                        break;
                    }
                }
            }

            return result;
        }

        private static T[] CountInversionAndSort<T>(T[] array, Func<T, T, int> comparer, ref long count)
        {
            if (array.Length <= 1)
            {

                return array;
            }
            else
            {
                T[] tempFirstArray = new T[array.Length / 2];

                T[] tempSecondArray = new T[array.Length / 2 + array.Length % 2];

                Array.Copy(array, 0, tempFirstArray, 0, tempFirstArray.Length);

                Array.Copy(array, tempFirstArray.Length, tempSecondArray, 0, tempSecondArray.Length);

                T[] firstArray = CountInversionAndSort(tempFirstArray, comparer, ref count);

                T[] secondArray = CountInversionAndSort(tempSecondArray, comparer, ref count);

                return MergeArrayProperly(firstArray, secondArray, comparer, ref count);
            }
        }

        private static T[] MergeArrayProperly<T>(T[] firstArray, T[] secondArray, Func<T, T, int> comparer, ref long count)
        {

            T[] result = new T[firstArray.Length + secondArray.Length];

            int firstIndex = 0, secondIndex = 0;

            for (int i = 0; i < result.Length; i++)
            {
                if (comparer(firstArray[firstIndex], secondArray[secondIndex]) > 0)
                {
                    count += firstArray.Length - firstIndex;

                    result[i] = secondArray[secondIndex];

                    secondIndex++;

                    if (secondIndex == secondArray.Length)
                    {
                        Array.Copy(firstArray, firstIndex, result, i + 1, firstArray.Length - firstIndex);

                        break;
                    }
                }
                else
                {
                    result[i] = firstArray[firstIndex];

                    firstIndex++;

                    if (firstIndex == firstArray.Length)
                    {
                        Array.Copy(secondArray, secondIndex, result, i + 1, secondArray.Length - secondIndex);

                        break;
                    }
                }
            }

            return result;
        }

        //******STANFORD: Second H.W.
        public static void QuickSort<T>(T[] array, Func<T, T, int> comparer)
        {
            long count = 0;
            QuickSort(array, comparer, 0, array.Length - 1, ref count);
        }

        private static void QuickSort<T>(T[] array, Func<T, T, int> comparer, int startIndex, int endIndex, ref long count)
        {
            if (startIndex > endIndex) return;

            count = count + (endIndex - startIndex + 1) - 1;

            int q = PartitionWithMedianElement(array, comparer, startIndex, endIndex);

            QuickSort(array, comparer, startIndex, q - 1, ref count);

            QuickSort(array, comparer, q + 1, endIndex, ref count);
        }

        private static int PartitionWithFirstElement<T>(T[] array, Func<T, T, int> comparer, int startIndex, int endIndex)
        {
            T pivot = array[startIndex];

            int i = startIndex + 1;

            for (int j = startIndex + 1; j <= endIndex; j++)
            {
                if (comparer(array[j], pivot) < 0)
                {
                    T temp = array[j];
                    array[j] = array[i];
                    array[i] = temp;
                    i++;
                }
            }

            T temp02 = array[startIndex];
            array[startIndex] = array[i - 1];
            array[i - 1] = temp02;

            return i - 1;
        }

        //private static int PartitionWithLastElement<T>(T[] array, Func<T, T, int> comparer, int startIndex, int endIndex)
        //{
        //    T pivot = array[endIndex];

        //    int i = startIndex;

        //    for (int j = startIndex; j <= (endIndex - 1); j++)
        //    {
        //        if (comparer(array[j], pivot) < 0)
        //        {
        //            T temp = array[j];
        //            array[j] = array[i];
        //            array[i] = temp;
        //            i++;
        //        }
        //    }

        //    T temp02 = array[endIndex];
        //    array[endIndex] = array[i];
        //    array[i] = temp02;

        //    return i;
        //}

        private static int PartitionWithLastElement<T>(T[] array, Func<T, T, int> comparer, int startIndex, int endIndex)
        {
            T temp02 = array[endIndex];
            array[endIndex] = array[startIndex];
            array[startIndex] = temp02;
            return PartitionWithFirstElement(array, comparer, startIndex, endIndex);
        }

        private static int PartitionWithMedianElement<T>(T[] array, Func<T, T, int> comparer, int startIndex, int endIndex)
        {
            T first = array[startIndex];
            T middle = array[(int)Math.Floor((endIndex + startIndex) / 2.0)];
            T last = array[endIndex];
            int index;

            if (comparer(first, middle) <= 0 && comparer(middle, last) <= 0)
            {
                index = (int)Math.Floor((endIndex + startIndex) / 2.0);
            }
            else if (comparer(first, last) <= 0 && comparer(last, middle) <= 0)
            {
                index = endIndex;
            }
            else if (comparer(middle, first) <= 0 && comparer(first, last) <= 0)
            {
                index = startIndex;
            }
            else if (comparer(middle, last) <= 0 && comparer(last, first) <= 0)
            {
                index = endIndex;
            }
            else if (comparer(last, first) <= 0 && comparer(first, middle) <= 0)
            {
                index = startIndex;
            }
            else if (comparer(last, middle) <= 0 && comparer(middle, first) <= 0)
            {
                index = (int)Math.Floor((endIndex + startIndex) / 2.0);
            }
            else
            {
                throw new NotImplementedException();
            }
            T temp02 = array[index];
            array[index] = array[startIndex];
            array[startIndex] = temp02;
            return PartitionWithFirstElement(array, comparer, startIndex, endIndex);
        }


    }
}
