using System;
using System.Collections.Generic;
using System.Text;

namespace VL.Consoling.Utils
{
    public static class AlgorithmHelper
    {
        #region 冒泡
        public static void BubbleSort(this int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - 1 - i; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        var temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }
        #endregion

        #region 快排
        internal static int[] QuickSort(this int[] array)
        {
            if (array == null || array.Length == 0)
                return array;
            QuickSortStructure(array, 0, array.Length - 1);
            return array;
        }

        private static void QuickSortStructure(int[] array, int leftIndex, int rightIndex)
        {
            if (leftIndex<rightIndex)//考点1 结构分层
            {
                var keyIndex = DoSort(array, leftIndex, rightIndex);
                QuickSortStructure(array, 0, keyIndex);
                QuickSortStructure(array, keyIndex + 1, rightIndex);
            }
        }

        private static int DoSort(int[] array, int leftIndex, int rightIndex)
        {
            var keyIndex = leftIndex;
            var keyValue = array[keyIndex];
            while (leftIndex < rightIndex)
            {
                while (leftIndex < rightIndex && array[rightIndex] >= keyValue)//考点2 来回倒腾
                {
                    rightIndex--;
                }
                array[leftIndex] = array[rightIndex];
                while (leftIndex < rightIndex && array[leftIndex] <= keyValue)
                {
                    leftIndex++;
                }
                array[rightIndex] = array[leftIndex];
            }
            array[leftIndex] = keyValue;
            return leftIndex;
        }
        #endregion
    }
}
