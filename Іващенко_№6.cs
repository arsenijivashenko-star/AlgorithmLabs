using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Лабораторна робота - Алгоритми роботи із деревами");

        byte option = 0;
        do
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Червоно-чорне дерево (Вставка, Повороти, Перефарбування)");
            Console.WriteLine("2. Дерево відрізків (LEFT-ROTATE з оновленням max)");
            Console.WriteLine("3. Пошук перекриття прямокутників");
            Console.WriteLine("4. АВЛ-дерево (Вставка)");
            Console.WriteLine("5. Splay-дерево (Splay алгоритм)");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            try { option = Convert.ToByte(Console.ReadLine()); }
            catch { option = 255; }

            switch (option)
            {
                case 1:
                    RunRBTreeDemo();
                    break;
                case 2:
                    RunIntervalTreeDemo();
                    break;
                case 3:
                    RunRectangleOverlapDemo();
                    break;
                case 4:
                    RunAVLTreeDemo();
                    break;
                case 5:
                    RunSplayTreeDemo();
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

    // --- ДЕМОНСТРАЦІЙНІ МЕТОДИ ---

    private static void RunRBTreeDemo()
    {
        RBTree tree = new RBTree();
        int[] keys = { 41, 38, 31, 12, 19, 8 };
        MeasureExecution("Вставка в Червоно-чорне дерево", () =>
        {
            foreach (int key in keys)
            {
                tree.Insert(key);
            }
        });
        Console.WriteLine("Елементи вставлено. Корінь: " + tree.Root.Value);
    }

    private static void RunIntervalTreeDemo()
    {
        IntervalTree tree = new IntervalTree();
        var x = new IntervalNode(15, 20, 30);
        var y = new IntervalNode(10, 30, 30);
        x.Right = y; y.Parent = x;

        Console.WriteLine($"До LEFT-ROTATE: x.max = {x.Max}, y.max = {y.Max}");
        tree.LeftRotate(x);
        Console.WriteLine($"Після LEFT-ROTATE: x.max = {x.Max}, y.max = {y.Max}");
    }

    private static void RunRectangleOverlapDemo()
    {
        List<Rectangle> rects = new List<Rectangle>
        {
            new Rectangle(0, 0, 10, 10),
            new Rectangle(20, 20, 30, 30),
            new Rectangle(5, 5, 15, 15) // Перекривається з першим
        };

        MeasureExecution("Перевірка перекриття прямокутників", () =>
        {
            bool hasOverlap = RectangleHelper.HasOverlappingRectangles(rects);
            Console.WriteLine($"Чи є перекриття? {hasOverlap}");
        });
    }

    private static void RunAVLTreeDemo()
    {
        AVLTree tree = new AVLTree();
        int[] keys = { 10, 20, 30, 40, 50, 25 };
        MeasureExecution("Вставка в АВЛ дерево", () =>
        {
            foreach (int key in keys) tree.Insert(key);
        });
        Console.WriteLine("Елементи вставлено.");
    }

    private static void RunSplayTreeDemo()
    {
        SplayTree tree = new SplayTree();
        tree.Insert(10);
        tree.Insert(20);
        tree.Insert(30);
        Console.WriteLine("Корінь після вставки 30 (через Splay): " + tree.Root.Value);
    }
}

// ==============================================================================
// 1. ЧЕРВОНО-ЧОРНЕ ДЕРЕВО (RED-BLACK TREE)
// ==============================================================================
public enum NodeColor { Red, Black }

public class RBNode
{
    public int Value;
    public NodeColor Color;
    public RBNode Left, Right, Parent;

    public RBNode(int value)
    {
        Value = value;
        Color = NodeColor.Red;
    }
}

public class RBTree
{
    public RBNode Root;

    // Лівий поворот
    public void LeftRotate(RBNode x)
    {
        RBNode y = x.Right;
        x.Right = y.Left;
        if (y.Left != null) y.Left.Parent = x;
        y.Parent = x.Parent;
        if (x.Parent == null) Root = y;
        else if (x == x.Parent.Left) x.Parent.Left = y;
        else x.Parent.Right = y;
        y.Left = x;
        x.Parent = y;
    }

    // Правий поворот
    public void RightRotate(RBNode x)
    {
        RBNode y = x.Left;
        x.Left = y.Right;
        if (y.Right != null) y.Right.Parent = x;
        y.Parent = x.Parent;
        if (x.Parent == null) Root = y;
        else if (x == x.Parent.Right) x.Parent.Right = y;
        else x.Parent.Left = y;
        y.Right = x;
        x.Parent = y;
    }

    // Вставка вузла
    public void Insert(int value)
    {
        RBNode z = new RBNode(value);
        RBNode y = null;
        RBNode x = Root;

        while (x != null)
        {
            y = x;
            if (z.Value < x.Value) x = x.Left;
            else x = x.Right;
        }

        z.Parent = y;
        if (y == null) Root = z;
        else if (z.Value < y.Value) y.Left = z;
        else y.Right = z;

        InsertFixup(z);
    }

    private void InsertFixup(RBNode z)
    {
        while (z.Parent != null && z.Parent.Color == NodeColor.Red)
        {
            if (z.Parent == z.Parent.Parent.Left)
            {
                RBNode y = z.Parent.Parent.Right;
                if (y != null && y.Color == NodeColor.Red)
                {
                    z.Parent.Color = NodeColor.Black;
                    y.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Right)
                    {
                        z = z.Parent;
                        LeftRotate(z);
                    }
                    z.Parent.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    RightRotate(z.Parent.Parent);
                }
            }
            else
            {
                RBNode y = z.Parent.Parent.Left;
                if (y != null && y.Color == NodeColor.Red)
                {
                    z.Parent.Color = NodeColor.Black;
                    y.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    z = z.Parent.Parent;
                }
                else
                {
                    if (z == z.Parent.Left)
                    {
                        z = z.Parent;
                        RightRotate(z);
                    }
                    z.Parent.Color = NodeColor.Black;
                    z.Parent.Parent.Color = NodeColor.Red;
                    LeftRotate(z.Parent.Parent);
                }
            }
        }
        Root.Color = NodeColor.Black;
    }

    // Перефарбування двох однакових вузлів братів у протилежний колір
    public void RecolorIdenticalSiblings(RBNode node)
    {
        if (node == null || node.Parent == null) return;
        RBNode sibling = (node == node.Parent.Left) ? node.Parent.Right : node.Parent.Left;

        if (sibling != null && sibling.Color == node.Color)
        {
            NodeColor newColor = (node.Color == NodeColor.Red) ? NodeColor.Black : NodeColor.Red;
            node.Color = newColor;
            sibling.Color = newColor;
        }
    }
}

// ==============================================================================
// 2. ДЕРЕВО ВІДРІЗКІВ (INTERVAL TREE - LEFT ROTATE)
// ==============================================================================
public class IntervalNode
{
    public int Low, High, Max;
    public IntervalNode Left, Right, Parent;

    public IntervalNode(int low, int high, int max)
    {
        Low = low; High = high; Max = max;
    }
}

public class IntervalTree
{
    // Оновлення атрибуту max за O(1)
    public void LeftRotate(IntervalNode x)
    {
        IntervalNode y = x.Right;
        if (y == null) return;

        x.Right = y.Left;
        if (y.Left != null) y.Left.Parent = x;
        y.Parent = x.Parent;

        y.Left = x;
        x.Parent = y;

        // Оновлення max за O(1)
        y.Max = x.Max;
        x.Max = Math.Max(x.High, Math.Max(GetMax(x.Left), GetMax(x.Right)));
    }

    private int GetMax(IntervalNode node) => node == null ? int.MinValue : node.Max;
}

// ==============================================================================
// 3. ПЕРЕКРИТТЯ ПРЯМОКУТНИКІВ
// ==============================================================================
public struct Rectangle
{
    public int X1, Y1, X2, Y2; // X1,Y1 - нижній лівий; X2,Y2 - верхній правий

    public Rectangle(int x1, int y1, int x2, int y2)
    {
        X1 = x1; Y1 = y1; X2 = x2; Y2 = y2;
    }
}

public static class RectangleHelper
{
    public static bool HasOverlappingRectangles(List<Rectangle> rects)
    {
        for (int i = 0; i < rects.Count; i++)
        {
            for (int j = i + 1; j < rects.Count; j++)
            {
                if (IsOverlapping(rects[i], rects[j])) return true;
            }
        }
        return false;
    }

    private static bool IsOverlapping(Rectangle r1, Rectangle r2)
    {
        // Якщо один знаходиться повністю зліва, справа, зверху або знизу іншого
        if (r1.X2 <= r2.X1 || r1.X1 >= r2.X2 || r1.Y2 <= r2.Y1 || r1.Y1 >= r2.Y2)
            return false;
        return true; // Включає ситуацію, коли один повністю всередині іншого
    }
}

// ==============================================================================
// 4. АВЛ-ДЕРЕВО (AVL TREE)
// ==============================================================================
public class AVLNode
{
    public int Value, Height;
    public AVLNode Left, Right;
    public AVLNode(int value) { Value = value; Height = 1; }
}

public class AVLTree
{
    public AVLNode Root;

    private int Height(AVLNode node) => node == null ? 0 : node.Height;
    private int GetBalance(AVLNode node) => node == null ? 0 : Height(node.Left) - Height(node.Right);

    private AVLNode RightRotate(AVLNode y)
    {
        AVLNode x = y.Left;
        AVLNode T2 = x.Right;
        x.Right = y;
        y.Left = T2;
        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
        return x;
    }

    private AVLNode LeftRotate(AVLNode x)
    {
        AVLNode y = x.Right;
        AVLNode T2 = y.Left;
        y.Left = x;
        x.Right = T2;
        x.Height = Math.Max(Height(x.Left), Height(x.Right)) + 1;
        y.Height = Math.Max(Height(y.Left), Height(y.Right)) + 1;
        return y;
    }

    public void Insert(int key) => Root = InsertRec(Root, key);

    private AVLNode InsertRec(AVLNode node, int key)
    {
        if (node == null) return new AVLNode(key);

        if (key < node.Value) node.Left = InsertRec(node.Left, key);
        else if (key > node.Value) node.Right = InsertRec(node.Right, key);
        else return node;

        node.Height = 1 + Math.Max(Height(node.Left), Height(node.Right));
        int balance = GetBalance(node);

        if (balance > 1 && key < node.Left.Value) return RightRotate(node);
        if (balance < -1 && key > node.Right.Value) return LeftRotate(node);
        if (balance > 1 && key > node.Left.Value)
        {
            node.Left = LeftRotate(node.Left);
            return RightRotate(node);
        }
        if (balance < -1 && key < node.Right.Value)
        {
            node.Right = RightRotate(node.Right);
            return LeftRotate(node);
        }

        return node;
    }
}

// ==============================================================================
// 5. SPLAY-ДЕРЕВО (SPLAY TREE)
// ==============================================================================
public class SplayNode
{
    public int Value;
    public SplayNode Left, Right;
    public SplayNode(int value) { Value = value; }
}

public class SplayTree
{
    public SplayNode Root;

    private SplayNode RightRotate(SplayNode x)
    {
        SplayNode y = x.Left;
        x.Left = y.Right;
        y.Right = x;
        return y;
    }

    private SplayNode LeftRotate(SplayNode x)
    {
        SplayNode y = x.Right;
        x.Right = y.Left;
        y.Left = x;
        return y;
    }

    public SplayNode Splay(SplayNode root, int key)
    {
        if (root == null || root.Value == key) return root;

        if (root.Value > key)
        {
            if (root.Left == null) return root;
            if (root.Left.Value > key)
            {
                root.Left.Left = Splay(root.Left.Left, key);
                root = RightRotate(root);
            }
            else if (root.Left.Value < key)
            {
                root.Left.Right = Splay(root.Left.Right, key);
                if (root.Left.Right != null) root.Left = LeftRotate(root.Left);
            }
            return (root.Left == null) ? root : RightRotate(root);
        }
        else
        {
            if (root.Right == null) return root;
            if (root.Right.Value > key)
            {
                root.Right.Left = Splay(root.Right.Left, key);
                if (root.Right.Left != null) root.Right = RightRotate(root.Right);
            }
            else if (root.Right.Value < key)
            {
                root.Right.Right = Splay(root.Right.Right, key);
                root = LeftRotate(root);
            }
            return (root.Right == null) ? root : LeftRotate(root);
        }
    }

    public void Insert(int key)
    {
        if (Root == null)
        {
            Root = new SplayNode(key);
            return;
        }

        Root = Splay(Root, key);

        if (Root.Value == key) return;

        SplayNode newNode = new SplayNode(key);
        if (Root.Value > key)
        {
            newNode.Right = Root;
            newNode.Left = Root.Left;
            Root.Left = null;
        }
        else
        {
            newNode.Left = Root;
            newNode.Right = Root.Right;
            Root.Right = null;
        }
        Root = newNode;
    }
}
