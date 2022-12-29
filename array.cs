namespace lab3_PersonalTask
{
    public class Sort
    {        
        public void BubbleSort(ref int[] array, int length, ref int countCompare, ref int countSwap, ref int countOperation) //estimation of machine time, recursion sort
        {
            if (length == 1)
                return;

            for (int i = 0; i < length - 1; i++)
            {
              
                countCompare++;
                if (array[i] > array[i + 1])
                {
                    int temp = array[i];
                    array[i] = array[i + 1];
                    array[i + 1] = temp;
                    countSwap++;

                }
            }
            
            BubbleSort(ref array, length - 1, ref countCompare, ref countSwap, ref countOperation);
        }

        int Partition(int[] array, int start, int end)
        {
            int temp;
            int marker = start;

            for (int i = start; i < end; i++)
                if (array[i] < array[end]) 
                {
                    temp = array[marker]; 
                    array[marker] = array[i];
                    array[i] = temp;
                    marker += 1;
                }
            
            temp = array[marker];
            array[marker] = array[end];
            array[end] = temp;

            return marker;
        }

        public void QuickSort(ref int[] array, int start, int end)
        {
            if (start >= end)
                return;
            
            int pivot = Partition(array, start, end);
            QuickSort(ref array, start, pivot - 1);
            QuickSort(ref array, pivot + 1, end);
        }

        public void SelectionSort(ref int[] array, int start, ref int countCompare, ref int countSwap, ref int countOperation) //estimation of machine time, recursion sort
        {

            if (start == array.Length)
                return;

            int min = start;

            for (int i = start + 1; i < array.Length; i++)
            {
                countCompare++;

                if (array[i] < array[min])
                {
                    min = i;
                }
              
            }
            
            int tmp = array[start];
            array[start] = array[min];
            array[min] = tmp;

            countSwap++;

            SelectionSort(ref array, start + 1, ref countCompare, ref countSwap, ref countOperation);
        }
    }
}
