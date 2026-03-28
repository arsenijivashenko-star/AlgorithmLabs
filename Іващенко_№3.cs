using System.Diagnostics;
using System.Text;
using System.Transactions;

public static class Program
{
    private static Random rand = new Random();
    private static string words = "";
    private static string subWords = "";

    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Іващенко Арсеній Вікторович - Лабораторна робота 2");

        byte option = 0;
        do
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Ввести рядок");
            Console.WriteLine("2. Ввести підрядок");
            Console.WriteLine("3. Прямий пошук");
            Console.WriteLine("4. КМП пошук");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            try { option = Convert.ToByte(Console.ReadLine()); }
            catch { option = 255; }

            switch (option)
            {
                case 1:
                    Console.WriteLine("Введіть рядок:");
                    words = Console.ReadLine() ?? "";
                    break;
                case 2:
                    Console.WriteLine("Введіть підрядок:");
                    words = Console.ReadLine() ?? "";
                    break;
                case 3:
                    Console.WriteLine("Індекс початку підрядку у рядку: " + MeasureExecution("Прямий пошук", () => NormalSearch(words, subWords)).ToString());
                    break;

            }
        } while (option != 0);
    }

    private static T MeasureExecution<T>(string label, Func<T> action)
    {
        Stopwatch sw = Stopwatch.StartNew();
        T a = action();
        sw.Stop();
        Console.WriteLine($"{label}: Виконано за {sw.Elapsed.TotalMilliseconds} мс");
        return a;
    }

    // --- АЛГОРИТМИ ---

    // 
    private static int NormalSearch(string word, string subWord) 
    {
        for (int i = 0; i < word.Length - subWord.Length; i++)
        {
            int match = 0;
            for (int j = 0; j < subWord.Length; i++)
            {
                if(subWord[i] == word[i + j])
                {
                    match++;
                }
            }

            if (match == subWord.Length);
                return i;
        }

        return -1;
    }

    private static int KMPSearch(string word, string subWord)
    {
        int n = word.Length;
        int m = subWord.Length;

        int[] lps = ComputeLPSArray(subWord);

        int i = 0;
        int j = 0;
        while (i < n)
        {
            if (subWord[j] == word[i])
            {
                i++;
                j++;
            }

            if (j == m)
            {
                return i - j;
            }
            else if (i < n && subWord[j] != word[i])
            {
                if (j != 0)
                    j = lps[j - 1];
                else
                    i++;
            }
        }
        return -1;
    }

    private static int[] ComputeLPSArray(string pattern)
    {
        int m = pattern.Length;
        int[] lps = new int[m];
        int len = 0;
        int i = 1;

        while (i < m)
        {
            if (pattern[i] == pattern[len])
            {
                len++;
                lps[i] = len;
                i++;
            }
            else
            {
                if (len != 0) len = lps[len - 1];
                else { lps[i] = 0; i++; }
            }
        }
        return lps;
    }
}