namespace CycleBreakCalculator.Helpers
{
    internal class QuickSort<T>(Func<T, T, int> comparer, bool isAscending = true)
    {
        public void Sort(ref T[] arr)
        {
            Sort(ref arr, 0, arr.Length - 1);
        }

        private void Sort(ref T[] arr, int startIndex, int endIndex)
        {
            if (startIndex >= endIndex)
                return;
                
            var partition = Partition(ref arr, startIndex, endIndex);

            Sort(ref arr, startIndex, partition - 1);
            Sort(ref arr, partition + 1, endIndex);
        }

        private int Partition(ref T[] arr, int startIndex, int endIndex)
        {
            var pivot = arr[endIndex];
            var i = startIndex - 1;

            for (var j = startIndex; j <= endIndex - 1; j++)
            {
                var comparisonResult = comparer(arr[j], pivot);
                if (isAscending ? comparisonResult == -1 : comparisonResult == 1)
                {
                    i++;
                    QuickSort<T>.Swap(ref arr, i, j);
                }
            }
            QuickSort<T>.Swap(ref arr, i + 1, endIndex);

            return i + 1;
        }

        private static void Swap(ref T[] arr, int i, int j)
        {
            (arr[j], arr[i]) = (arr[i], arr[j]);
        }
    }
}
