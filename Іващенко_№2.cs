using System.Diagnostics;
using System.Text;
using System.Transactions;

public static class Program
{
    private static Random rand = new Random();

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Іващенко Арсеній Вікторович - Лабораторна робота 2");

        byte option = 0;
        do
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. QuickSort");
            Console.WriteLine("2. Randomized QuickSort");
            Console.WriteLine("3. Counting Sort");
            Console.WriteLine("4. Radix Sort");
            Console.WriteLine("5. Bucket Sort");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            try { option = Convert.ToByte(Console.ReadLine()); }
            catch { option = 255; }

            switch (option)
            {
                case 1:
                    int[] array1 = { 13, 19, 9, 5, 12, 8, 7, 4, 21, 2, 6, 11 };
                    MeasureExecution("QuickSort (Спадання)", () => QuickSortDescending(array1, 0, 11));
                    foreach (int i in array1)
                        Console.Write(i.ToString()+", ");
                    Console.Write("\n");
                    break;
                case 2:
                    int[] array2 = { 13, 19, 9, 5, 12, 8, 7, 4, 21, 2, 6, 11 };
                    MeasureExecution("Randomized QuickSort (Спадання)", () => RandomizedQuickSort(array2, 0, 11));
                    foreach (int i in array2)
                        Console.Write(i.ToString() + ", ");
                    Console.Write("\n");
                    break;
                case 3:
                    int[] array3 = { 6, 0, 2, 0, 1, 3, 4, 6, 1, 3, 2 };
                    MeasureExecution("Counting Sort", () => array3 = CountingSort(array3, 21));
                    foreach (int i in array3)
                        Console.Write(i.ToString() + ", ");
                    Console.Write("\n");
                    break;
                case 4:
                    string[] array4 = { "COW", "DOG", "SEA", "RUG", "ROW", "MOB", "BOX", "TAB", "BAR", "EAR", "TAR", "DIG", "BIG", "TEA", "NOW", "FOX" };
                    MeasureExecution("Radix Sort", () => RadixSortWords(array4, 3));
                    foreach (string i in array4)
                        Console.Write(i + ", ");
                    Console.Write("\n");
                    break;
                case 5:
                    double[] array5 = { 0.79, 0.13, 0.16, 0.64, 0.39, 0.20, 0.89, 0.53, 0.71, 0.42 };
                    List<double> output = new List<double> { };
                    MeasureExecution("Bucket Sort", () => output = BucketSort(array5));
                    foreach (double i in output)
                        Console.Write(i.ToString() + ", ");
                    Console.Write("\n");
                    break;

            }
        } while (option != 0);
    }

    private static void MeasureExecution(string label, Action action)
    {
        Stopwatch sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        Console.WriteLine($"{label}: Виконано за {sw.Elapsed.TotalMilliseconds} мс");
    }

    // --- АЛГОРИТМИ ---

    // 
    private static int Partition(int[] A, int p, int r)
    {
        // Перевірка на однакові елементи 
        bool allEqual = true;
        for (int k = p; k < r; k++)
        {
            if (A[k] != A[k + 1]) { allEqual = false; break; }
        }
        if (allEqual) return (p + r) / 2;

        int x = A[r];
        int i = p - 1;

        // Сортування у незростаючому порядку (за спаданням) 
        for (int j = p; j < r; j++)
        {
            if (A[j] >= x)
            {
                i++;
                Swap(A, i, j);
            }
        }
        Swap(A, i + 1, r);
        return i + 1;
    }
    private static void QuickSortDescending(int[] A, int p, int r)
    {
        if (p < r)
        {
            int q = Partition(A, p, r);
            QuickSortDescending(A, p, q - 1);
            QuickSortDescending(A, q + 1, r);
        }
    }
    private static void RandomizedQuickSort(int[] A, int p, int r)
    {
        if (p < r)
        {
            int i = rand.Next(p, r + 1);
            Swap(A, r, i);
            int q = Partition(A, p, r);
            RandomizedQuickSort(A, p, q - 1);
            RandomizedQuickSort(A, q + 1, r);
        }
    }

    
    private static int[] CountingSort(int[] A, int k)
    {
        int[] C = new int[k + 1];
        int[] B = new int[A.Length];

        for (int j = 0; j < A.Length; j++) C[A[j]]++;
        for (int i = 1; i <= k; i++) C[i] += C[i - 1];

        for (int j = A.Length - 1; j >= 0; j--)
        {
            B[C[A[j]] - 1] = A[j];
            C[A[j]]--;
        }
        return B;
    }

    private static List<double> BucketSort(double[] A)
    {
        int n = A.Length;
        List<double>[] B = new List<double>[n];
        for (int i = 0; i < n; i++) B[i] = new List<double>();
        foreach (var val in A) B[(int)(n * val)].Add(val);
        List<double> result = new List<double>();
        foreach (var bucket in B)
        {
            bucket.Sort();
            result.AddRange(bucket);
        }
        return result;
    }

    private static void RadixSortWords(string[] A, int d)
    {
        int n = A.Length;
        int alphabetSize = 256;

        for (int charIdx = d - 1; charIdx >= 0; charIdx--)
        {
            string[] output = new string[n];
            int[] count = new int[alphabetSize];

            for (int i = 0; i < n; i++)
            {
                int c = charIdx < A[i].Length ? (int)A[i][charIdx] : 0;
                count[c]++;
            }

            for (int i = 1; i < alphabetSize; i++)
            {
                count[i] += count[i - 1];
            }

            for (int i = n - 1; i >= 0; i--)
            {
                int c = charIdx < A[i].Length ? (int)A[i][charIdx] : 0;
                output[count[c] - 1] = A[i];
                count[c]--;
            }

            for (int i = 0; i < n; i++)
            {
                A[i] = output[i];
            }
        }
    }

    private static void Swap(int[] A, int i, int j)
    {
        int temp = A[i];
        A[i] = A[j];
        A[j] = temp;
    }
}
