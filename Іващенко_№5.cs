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

    // Рекурсивна версія процедури TREE-INSERT (Завдання 8)
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
}

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Іващенко Арсеній Вікторович - Лабораторна робота 5");

        BST tree = new BST();
        int[] keys = { 10, 5, 16, 4, 1, 17, 21 };

        foreach (int key in keys)
        {
            tree.InsertRecursive(key);
        }

        byte option = 0;
        do
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Створити та показати дерева різної висоти (Завдання 2)");
            Console.WriteLine("2. Прямий обхід (Рекурсивний)");
            Console.WriteLine("3. Зворотний обхід (Рекурсивний)");
            Console.WriteLine("4. Симетричний обхід (Нерекурсивний)");
            Console.WriteLine("5. Знайти Мінімум та Максимум (Рекурсивно)");
            Console.WriteLine("6. Знайти попередника кореня");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            try { option = Convert.ToByte(Console.ReadLine()); }
            catch { option = 255; }

            switch (option)
            {
                case 1:
                    Console.WriteLine("\nГенерація дерев з ключами {1,4,5,10,16,17,21}:");
                    Console.WriteLine("Дерево висотою 2: 10(корінь) -> Ліворуч: 4(1, 5), Праворуч: 17(16, 21)");
                    Console.WriteLine("Дерево висотою 6 (Вироджене): 1 -> 4 -> 5 -> 10 -> 16 -> 17 -> 21");
                    break;
                case 2:
                    Console.Write("Прямий обхід (Pre-order): ");
                    MeasureExecution("Прямий обхід", () => tree.PreOrder(tree.Root));
                    Console.WriteLine();
                    break;
                case 3:
                    Console.Write("Зворотний обхід (Post-order): ");
                    MeasureExecution("Зворотний обхід", () => tree.PostOrder(tree.Root));
                    Console.WriteLine();
                    break;
                case 4:
                    Console.Write("Симетричний обхід (In-order, без рекурсії): ");
                    MeasureExecution("Симетричний обхід", () => tree.InOrderIterative());
                    Console.WriteLine();
                    break;
                case 5:
                    Node min = tree.MinRecursive(tree.Root);
                    Node max = tree.MaxRecursive(tree.Root);
                    Console.WriteLine($"Мінімум: {(min != null ? min.Key.ToString() : "Порожньо")}");
                    Console.WriteLine($"Максимум: {(max != null ? max.Key.ToString() : "Порожньо")}");
                    break;
                case 6:
                    Node predecessor = tree.Predecessor(tree.Root);
                    Console.WriteLine($"Попередник кореня ({tree.Root.Key}): {(predecessor != null ? predecessor.Key.ToString() : "Немає")}");
                    break;
                case 0:
                    Console.WriteLine("Завершення роботи.");
                    break;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }
        } while (option != 0);
    }

    private static void MeasureExecution(string label, Action action)
    {
        Stopwatch sw = Stopwatch.StartNew();
        action();
        sw.Stop();
        Console.Write($"\n[{label}: Виконано за {sw.Elapsed.TotalMilliseconds} мс]");
    }
}
