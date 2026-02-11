using System.Diagnostics;
using System.Text;

public static class Program
{
    private static int[] array;
    private static int target;
    private static int size;

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Іващенко Арсеній Вікторович");

        byte option = 0;
        do
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Створити структури даних");
            Console.WriteLine("2. Задати елемент пошуку");
            Console.WriteLine("3. Пошук перебором)");
            Console.WriteLine("4. Пошук з бар'єром");
            Console.WriteLine("5. Бінарний пошук");
            Console.WriteLine("6. Пошук золотого перерізу");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            try { option = Convert.ToByte(Console.ReadLine()); }
            catch { option = 255; }

            switch (option)
            {
                case 1:
                    Console.Write("Введіть розмір: ");
                    size = int.Parse(Console.ReadLine());
                    GenerateData(size);
                    break;
                case 2:
                    Console.Write("Введіть число для пошуку: ");
                    target = int.Parse(Console.ReadLine());
                    break;
                case 3:
                    MeasureExecution("Лінійний (Масив)", () => LinearSearchArray(array, target));
                    break;
                case 4:
                    MeasureExecution("З бар'єром (Масив)", () => SentinelSearch(array, target));
                    break;
                case 5:
                    Array.Sort(array); // Бінарний пошук потребує сортування
                    MeasureExecution("Бінарний (Масив)", () => BinarySearch(array, target));
                    break;
                case 6:
                    Array.Sort(array);
                    MeasureExecution("Золотий переріз (Масив)", () => GoldenSectionSearch(array, target));
                    break;
            }
        } while (option != 0);
    }

    private static void GenerateData(int n)
    {
        Random rand = new Random();
        array = new int[n];
        for (int i = 0; i < n; i++) array[i] = rand.Next(0, n * 2);
        Console.WriteLine($"Згенеровано {n} елементів.");
    }

    private static void MeasureExecution(string label, Func<int> action)
    {
        Stopwatch sw = Stopwatch.StartNew();
        int result = action();
        sw.Stop();
        Console.WriteLine($"{label}: Індекс = {result}, Час = {sw.Elapsed.TotalMilliseconds} мс");
    }

    // 1. Пошук перебором
    private static int LinearSearchArray(int[] data, int key)
    {
        for (int i = 0; i < data.Length; i++)
            if (data[i] == key) return i;
        return -1;
    }

    // 2. Пошук з бар'єром
    private static int SentinelSearch(int[] data, int key)
    {
        int n = data.Length;
        if (n == 0) return -1;
        int last = data[n - 1];
        data[n - 1] = key; // Бар'єр
        int i = 0;
        while (data[i] != key) i++;
        data[n - 1] = last; // Повертаємо назад
        if (i < n - 1 || last == key) return i;
        return -1;
    }

    // 3. Бінарний пошук
    private static int BinarySearch(int[] data, int key)
    {
        int left = 0, right = data.Length - 1;
        while (left <= right)
        {
            int mid = left + (right - left) / 2;
            if (data[mid] == key) return mid;
            if (data[mid] < key) left = mid + 1;
            else right = mid - 1;
        }
        return -1;
    }

    // 4. Пошук золотого перерізу
    private static int GoldenSectionSearch(int[] data, int key)
    {
        double phi = (Math.Sqrt(5) - 1) / 2; // ~0.618
        int left = 0, right = data.Length - 1;
        while (left <= right)
        {
            int mid = left + (int)(phi * (right - left));
            if (data[mid] == key) return mid;
            if (data[mid] < key) left = mid + 1;
            else right = mid - 1;
        }
        return -1;
    }
}

