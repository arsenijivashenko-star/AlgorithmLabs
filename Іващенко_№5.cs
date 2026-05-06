using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public class Node
{
    public int Key;
    public Node Left, Right, Parent;

    public Node(int item)
    {
        Key = item;
        Left = Right = Parent = null;
    }
}

public class BST
{
    public Node Root;

    // Вставка з підтримкою Parent
    public void InsertRecursive(int key)
    {
        Root = InsertRecursive(Root, null, key);
    }

    private Node InsertRecursive(Node current, Node parent, int key)
    {
        if (current == null)
        {
            Node newNode = new Node(key);
            newNode.Parent = parent;
            return newNode;
        }

        if (key < current.Key)
            current.Left = InsertRecursive(current.Left, current, key);
        else if (key > current.Key)
            current.Right = InsertRecursive(current.Right, current, key);

        return current;
    }

    // --- РЕАЛІЗАЦІЯ ВИДАЛЕННЯ (TREE-DELETE) ---
    public void DeleteByKey(int key)
    {
        Node z = Search(Root, key);
        if (z == null) return; // Вузол не знайдено

        if (z.Left == null)
            Transplant(z, z.Right);
        else if (z.Right == null)
            Transplant(z, z.Left);
        else
        {
            // Знаходимо наступника (мінімум у правому піддереві)
            Node y = MinRecursive(z.Right);
            if (y.Parent != z)
            {
                Transplant(y, y.Right);
                y.Right = z.Right;
                y.Right.Parent = y;
            }
            Transplant(z, y);
            y.Left = z.Left;
            y.Left.Parent = y;
        }
    }

    // Допоміжна функція для заміни піддерев
    private void Transplant(Node u, Node v)
    {
        if (u.Parent == null)
            Root = v;
        else if (u == u.Parent.Left)
            u.Parent.Left = v;
        else
            u.Parent.Right = v;

        if (v != null)
            v.Parent = u.Parent;
    }

    public Node Search(Node current, int key)
    {
        while (current != null && current.Key != key)
        {
            if (key < current.Key) current = current.Left;
            else current = current.Right;
        }
        return current;
    }
    // -------------------------------------------

    public void PreOrder(Node node)
    {
        if (node != null)
        {
            Console.Write(node.Key + " ");
            PreOrder(node.Left);
            PreOrder(node.Right);
        }
    }

    public void PostOrder(Node node)
    {
        if (node != null)
        {
            PostOrder(node.Left);
            PostOrder(node.Right);
            Console.Write(node.Key + " ");
        }
    }

    public void InOrderIterative()
    {
        Stack<Node> stack = new Stack<Node>();
        Node current = Root;

        while (current != null || stack.Count > 0)
        {
            while (current != null)
            {
                stack.Push(current);
                current = current.Left;
            }

            current = stack.Pop();
            Console.Write(current.Key + " ");
            current = current.Right;
        }
    }

    public Node MinRecursive(Node node)
    {
        if (node == null || node.Left == null)
            return node;
        return MinRecursive(node.Left);
    }

    public Node MaxRecursive(Node node)
    {
        if (node == null || node.Right == null)
            return node;
        return MaxRecursive(node.Right);
    }

    public Node Predecessor(Node node)
    {
        if (node == null) return null;

        if (node.Left != null)
        {
            Node temp = node.Left;
            while (temp.Right != null) temp = temp.Right;
            return temp;
        }

        Node y = node.Parent;
        while (y != null && node == y.Left)
        {
            node = y;
            y = y.Parent;
        }
        return y;
    }

    // Візуалізація дерева у консолі
    public void PrintTree(Node root, string indent = "", bool isRight = true)
    {
        if (root != null)
        {
            Console.Write(indent);
            if (isRight)
            {
                Console.Write("R----");
                indent += "     ";
            }
            else
            {
                Console.Write("L----");
                indent += "|    ";
            }
            Console.WriteLine(root.Key);
            PrintTree(root.Left, indent, false);
            PrintTree(root.Right, indent, true);
        }
    }

    // Перевірка структурної рівності дерев (для доведення)
    public static bool AreIdentical(Node a, Node b)
    {
        if (a == null && b == null) return true;
        if (a == null || b == null) return false;
        return (a.Key == b.Key) && AreIdentical(a.Left, b.Left) && AreIdentical(a.Right, b.Right);
    }
}

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Іващенко Арсеній Вікторович - Лабораторна робота 5");

        BST tree = new BST();
        int[] keys = { 10, 5, 16, 4, 1, 17, 21 };

        foreach (int key in keys) tree.InsertRecursive(key);

        byte option = 0;
        do
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Створити та візуалізувати дерева висотою 2-6 (Завдання 2)");
            Console.WriteLine("2. Прямий обхід (Рекурсивний)");
            Console.WriteLine("3. Зворотний обхід (Рекурсивний)");
            Console.WriteLine("4. Симетричний обхід (Нерекурсивний)");
            Console.WriteLine("5. Знайти Мінімум та Максимум (Рекурсивно)");
            Console.WriteLine("6. Знайти попередника кореня");
            Console.WriteLine("7. Видалити вузол з поточного дерева");
            Console.WriteLine("8. ПРАКТИЧНЕ ДОВЕДЕННЯ: Некомутативність видалення");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            try { option = Convert.ToByte(Console.ReadLine()); }
            catch { option = 255; }

            switch (option)
            {
                case 1:
                    GenerateAndPrintTreesOfVariousHeights();
                    break;
                case 2:
                    Console.Write("Прямий обхід (Pre-order): ");
                    tree.PreOrder(tree.Root); Console.WriteLine();
                    break;
                case 3:
                    Console.Write("Зворотний обхід (Post-order): ");
                    tree.PostOrder(tree.Root); Console.WriteLine();
                    break;
                case 4:
                    Console.Write("Симетричний обхід (In-order): ");
                    tree.InOrderIterative(); Console.WriteLine();
                    break;
                case 5:
                    Node min = tree.MinRecursive(tree.Root);
                    Node max = tree.MaxRecursive(tree.Root);
                    Console.WriteLine($"Мінімум: {(min != null ? min.Key.ToString() : "Порожньо")}");
                    Console.WriteLine($"Максимум: {(max != null ? max.Key.ToString() : "Порожньо")}");
                    break;
                case 6:
                    Node predecessor = tree.Predecessor(tree.Root);
                    Console.WriteLine($"Попередник кореня ({(tree.Root != null ? tree.Root.Key.ToString() : "null")}): {(predecessor != null ? predecessor.Key.ToString() : "Немає")}");
                    break;
                case 7:
                    Console.Write("Введіть ключ для видалення: ");
                    if (int.TryParse(Console.ReadLine(), out int delKey))
                    {
                        tree.DeleteByKey(delKey);
                        Console.WriteLine($"Вузол {delKey} видалено. Поточне дерево:");
                        tree.PrintTree(tree.Root);
                    }
                    break;
                case 8:
                    ProveNonCommutativity();
                    break;
                case 0:
                    Console.WriteLine("Завершення роботи.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір.");
                    break;
            }
        } while (option != 0);
    }

    // Метод для Завдання 2 (побудова дерев різної висоти)
    private static void GenerateAndPrintTreesOfVariousHeights()
    {
        // Різні порядки вставки генерують різну висоту
        int[][] insertionOrders = new int[][]
        {
            new int[] { 10, 4, 17, 1, 5, 16, 21 }, // Висота 2 (Збалансоване)
            new int[] { 10, 4, 16, 1, 5, 17, 21 }, // Висота 3
            new int[] { 5, 1, 10, 4, 16, 17, 21 }, // Висота 4
            new int[] { 4, 1, 5, 10, 16, 21, 17 }, // Висота 5
            new int[] { 1, 4, 5, 10, 16, 17, 21 }  // Висота 6 (Вироджене дерево)
        };

        for (int i = 0; i < insertionOrders.Length; i++)
        {
            BST t = new BST();
            foreach (int k in insertionOrders[i]) t.InsertRecursive(k);
            Console.WriteLine($"\n===============================");
            Console.WriteLine($"Дерево висотою {i + 2} (Ключі вставлялись: {string.Join(", ", insertionOrders[i])})");
            t.PrintTree(t.Root);
        }
    }

    // Динамічний пошуковик контрприкладу
    private static void ProveNonCommutativity()
    {
        Console.WriteLine("\n[Доведення] Шукаю контрприклад шляхом перебору...");
        Random rnd = new Random(100); // Фіксований seed для відтворюваності

        while (true)
        {
            int[] keys = new int[8];
            for (int i = 0; i < keys.Length; i++) keys[i] = rnd.Next(1, 50);

            int u = keys[0];
            int v = keys[1];
            if (u == v) continue;

            BST t1 = new BST();
            BST t2 = new BST();
            foreach (int k in keys) { t1.InsertRecursive(k); t2.InsertRecursive(k); }

            // Порядок 1: Видаляємо U, потім V
            t1.DeleteByKey(u);
            t1.DeleteByKey(v);

            // Порядок 2: Видаляємо V, потім U
            t2.DeleteByKey(v);
            t2.DeleteByKey(u);

            // Перевіряємо чи збереглась ідентичність структур
            if (!BST.AreIdentical(t1.Root, t2.Root))
            {
                Console.WriteLine("-> ЗНАЙДЕНО КОНТРПРИКЛАД!\n");
                Console.WriteLine($"Масив вставки: {string.Join(", ", keys)}");
                Console.WriteLine($"Вузли для перевірки: X={u}, Y={v}\n");

                Console.WriteLine($"--- Результат 1: Видалили {u}, ПОТІМ {v} ---");
                t1.PrintTree(t1.Root);

                Console.WriteLine($"\n--- Результат 2: Видалили {v}, ПОТІМ {u} ---");
                t2.PrintTree(t2.Root);

                Console.WriteLine("\nВисновок: Як наочно видно з консолі, форма дерев відрізняється.");
                Console.WriteLine("Отже, операція видалення у бінарному дереві пошуку НЕ Є комутативною.");
                break;
            }
        }
    }
}
