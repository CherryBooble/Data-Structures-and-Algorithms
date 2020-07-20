using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortComparison
{
    struct LeftRight
    {
        public int Left;
        public int Right;

        public LeftRight(int l, int r)
        {
            Left = l;
            Right = r;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Random rand = new Random();
            int[] arr = new int[100000];

            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = rand.Next();
            }
            //Array.Sort(arr);

            var clone = (int[])arr.Clone();

            //Console.WriteLine("\n\n");
            //Console.WriteLine(String.Join(" ", arr));
            //Console.WriteLine("\n\n");

            double t1, t2;
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //Array.Sort(arr);
            QuickSort(arr, 0, arr.Length - 1);
            //HeapSort(arr);
            //InsertSort(arr);
            //MergeSort(arr, 0, arr.Length - 1);
            watch.Stop();
            t1 = watch.Elapsed.TotalMilliseconds;
            Console.WriteLine($"QuickSort time: {t1}");

            watch = System.Diagnostics.Stopwatch.StartNew();
            HeapSort(clone);
            //BoobleSort(clone);
            //QuickSort(clone, 0, clone.Length - 1);
            //QuickSortWithStack(clone, 0, clone.Length - 1);
            //TestQuckSort(clone, 0, clone.Length - 1);
            //BoobleSort(clone);
            //InsertSort(clone, 0, clone.Length - 1);
            //CountingSort(clone, m);
            //MergeSort(clone, 0, clone.Length - 1);
            //TestHeapSort(clone);
            //SelectionSort(clone);

            watch.Stop();
            t2 = watch.Elapsed.TotalMilliseconds;

            Console.WriteLine($"HeapSort time: {t2}");

            Console.WriteLine($"\nTime difference t2 - t1: {t2 - t1}");

            //Console.WriteLine();
            //foreach (var item in arr)
            //    Console.Write($"{item} ");

            //Console.WriteLine("\n\n");

            //foreach (var item in clone)
            //    Console.Write($"{item} ");

            Console.WriteLine("\nEquality of arrays \"arr\" and \"clone\" after sorting: " + Enumerable.SequenceEqual(arr, clone));
            Console.WriteLine("\n\n\n\n\n");
        }

        static void QuickSort(int[] arr, int first, int last)
        {
            if (first < last)
            {
                int pivot = Partition(arr, first, last);

                QuickSort(arr, first, pivot - 1);
                QuickSort(arr, pivot + 1, last);
            }
        }

        static void TestQuckSort(int[] arr, int first, int last)
        {
            if (last - first > 16) //128 200-13.68 190-13.61 170-13.52(7.6) 150-13.50(6.6)
            {
                int pivot = Partition(arr, first, last);

                TestQuckSort(arr, first, pivot - 1);
                TestQuckSort(arr, pivot + 1, last);
            }
            else
                InsertSort(arr, first, last);
        }

        static int Partition(int[] arr, int first, int last)
        {
            int left = first, right = last, pivot = arr[first];
            while (left < right)
            {
                while (pivot < arr[right])
                    right--;
                if (left < right)
                {
                    arr[left] = arr[right];
                    arr[right] = pivot;
                    left++;
                }

                while (arr[left] < pivot)
                    left++;
                if (left < right)
                {
                    arr[right] = arr[left];
                    arr[left] = pivot;
                    right--;
                }
            }

            return left;
        }

        static void QuickSortWithStack(int[] arr, int first, int last)
        {
            Stack<LeftRight> stack = new Stack<LeftRight>();
            stack.Push(new LeftRight(first, last));

            while (stack.Count > 0)
            {
                var lr = stack.Pop();

                if (lr.Left < lr.Right)
                {
                    int partition = Partition(arr, lr.Left, lr.Right);

                    if (lr.Left < partition - 1)
                        stack.Push(new LeftRight(lr.Left, partition - 1));

                    if (partition + 1 < lr.Right)
                        stack.Push(new LeftRight(partition + 1, lr.Right));
                }
            }
        }

        static void BoobleSort(int[] arr)
        {
            int temp;
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length - 1 - i; j++)
                {
                    if (arr[j] > arr[j + 1])
                    {
                        temp = arr[j];
                        arr[j] = arr[j + 1];
                        arr[j + 1] = temp;
                    }
                }
            }
        }

        static void InsertSort(int[] arr, int left, int right)
        {
            int j = left + 1;
            while (j <= right)
            {
                int key = arr[j];

                int i = j;
                while (i > 0 && arr[i - 1] > key)
                {
                    arr[i] = arr[i - 1];
                    i--;
                }

                arr[i] = key;
                j++;
            }
        }

        static void SelectionSort(int[] arr)
        {
            int smallest, temp;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                smallest = i;

                for (int j = i + 1; j < arr.Length; j++)
                {
                    if (arr[j] < arr[smallest])
                        smallest = j;
                }

                temp = arr[smallest];
                arr[smallest] = arr[i];
                arr[i] = temp;

            }
        }

        static void MergeSort(int[] arr, int first, int last)
        {
            if (first < last)
            {
                int middle = (first + last) / 2;
                MergeSort(arr, first, middle);
                MergeSort(arr, middle + 1, last);

                Merge(arr, first, middle, last);
            }
        }

        static void Merge(int[] arr, int l, int m, int r)
        {
            int i, j, k;
            int n1 = m - l + 1;
            int n2 = r - m;

            int[] L = new int[n1];
            int[] R = new int[n2];

            for (i = 0; i < n1; i++)
                L[i] = arr[l + i];
            for (j = 0; j < n2; j++)
                R[j] = arr[m + 1 + j];

            i = 0;
            j = 0;
            k = l;

            while (i < n1 && j < n2)
            {
                if (L[i] <= R[j])
                    arr[k] = L[i++];
                else
                    arr[k] = R[j++];
                k++;
            }

            while (i < n1)
                arr[k++] = L[i++];
           
            while (j < n2)
                arr[k++] = R[j++];
        }

        static void CountingSort(int[] arr, int m)
        {
            int[] hash = new int[m];
            for (int i = 0; i < arr.Length; i++)
                hash[arr[i]]++;

            int pos = 0;
            for (int i = 0; i < hash.Length; i++)
            {
                int count = hash[i];
                for (int n = 0; n < count; n++)
                {
                    arr[pos++] = i;
                }
            }
        }

        static void HeapSort(int[] arr)
        {
            int n = arr.Length;

            // Build heap (rearrange array) 
            for (int i = n / 2 - 1; i >= 0; i--)
                heapify(arr, n, i);

            // One by one extract an element from heap 
            for (int i = n - 1; i > 0; i--)
            {
                // Move current root to end 
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;

                // call max heapify on the reduced heap 
                heapify(arr, i, 0);
            }
        }

        static void heapify(int[] arr, int n, int i)
        {
            int largest = i; // Initialize largest as root 
            int l = 2 * i + 1; // left = 2*i + 1 
            int r = 2 * i + 2; // right = 2*i + 2 

            // If left child is larger than root 
            if (l < n && arr[l] > arr[largest])
                largest = l;

            // If right child is larger than largest so far 
            if (r < n && arr[r] > arr[largest])
                largest = r;

            // If largest is not root 
            if (largest != i)
            {
                int swap = arr[i];
                arr[i] = arr[largest];
                arr[largest] = swap;

                // Recursively heapify the affected sub-tree 
                heapify(arr, n, largest);
            }
        }

        static void TestHeapSort(int[] arr)
        {
            int n = arr.Length;


            for (int i = n / 2 - 1; i >= 0; i--)
                MakeHeapify(arr, n, i);

            for (int i = n - 1; i > 0; i--)
            {
                int temp = arr[0];
                arr[0] = arr[i];
                arr[i] = temp;

                MakeHeapify(arr, i, 0);
            }

        }

        static void MakeHeapify(int[] arr, int n, int parent)
        {
            int index, cur, temp;
            do
            {
                cur = parent;
                index = 2 * parent + 1;

                if (index < n && arr[index] > arr[parent])
                    parent = index;

                index++;

                if (index < n && arr[index] > arr[parent])
                    parent = index;

                if (cur != parent)
                {
                    temp = arr[cur];
                    arr[cur] = arr[parent];
                    arr[parent] = temp;
                }
                else
                    break;
            }
            while (true);

        }
    }
}
