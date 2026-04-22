using System;
using System.Collections.Generic;
using System.Text;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Іващенко Арсеній Вікторович - Лабораторна робота 4");

        byte option = 0;
        do
        {
            Console.WriteLine("\n--- МЕНЮ ---");
            Console.WriteLine("1. Стек (PUSH/POP)");
            Console.WriteLine("2. Черга (ENQUEUE/DEQUEUE)");
            Console.WriteLine("3. Дек (Двостороння черга)");
            Console.WriteLine("4. SetOfStacks (Стопка тарілок)");
            Console.WriteLine("5. Перевірка виразу з дужками");
            Console.WriteLine("6. Об'єднання відсортованих черг");
            Console.WriteLine("7. Видимість споруд");
            Console.WriteLine("8. Два стеки в одному масиві");
            Console.WriteLine("9. Черга за допомогою двох стеків");
            Console.WriteLine("0. Вихід");
            Console.Write("Виберіть опцію: ");

            try { option = Convert.ToByte(Console.ReadLine()); }
            catch { option = 255; }

            switch (option)
            {
                case 1: RunStackTask(); break;
                case 2: RunQueueTask(); break;
                case 3: RunDequeTask(); break;
                case 4: RunSetOfStacksTask(); break;
                case 5: RunParenthesesTask(); break;
                case 6: RunMergeQueuesTask(); break;
                case 7: RunBuildingsTask(); break;
                case 8: RunTwoStacksTask(); break;
                case 9: RunQueueUsingStacksTask(); break;
            }
        } while (option != 0);
    }

    // --- БАЗОВІ СТРУКТУРИ ДАНИХ ---

    public class Stack
    {
        private int[] S;
        private int top;

        public int Count { get => top; }

        public Stack(int size)
        {
            S = new int[size + 1];
            top = 0;
        }

        public void Push(int x)
        {
            if (top == S.Length - 1) throw new Exception("Overflow: Стек переповнений");
            S[++top] = x;
            Print();
        }

        public int Pop()
        {
            if (top == 0) throw new Exception("Underflow: Стек порожній");
            int val = S[top--];
            Print();
            return val;
        }

        private void Print()
        {
            Console.Write("Стек: ");
            for (int i = 1; i <= top; i++) Console.Write($"{S[i]} ");
            Console.WriteLine();
        }
    }

    public class Deque
    {
        private int[] Q;
        private int head;
        private int tail;
        private int n;

        public int Count => (tail + n - head) % n;

        public Deque(int size)
        {
            n = size;
            Q = new int[n + 1];
            head = 1;
            tail = 1;
        }

        public void Enqueue(int x)
        {
            if (IsFull()) throw new Exception("Overflow: Черга переповнена");
            Q[tail] = x;
            tail = (tail % n) + 1;
            Print();
        }

        public int Dequeue()
        {
            if (IsEmpty()) throw new Exception("Underflow: Черга порожня");
            int x = Q[head];
            head = (head % n) + 1;
            Print();
            return x;
        }

        public void PushFront(int x)
        {
            if (IsFull()) throw new Exception("Overflow: Дек переповнений");
            head = (head == 1) ? n : head - 1;
            Q[head] = x;
            Print();
        }

        public int PopBack()
        {
            if (IsEmpty()) throw new Exception("Underflow: Дек порожній");
            tail = (tail == 1) ? n : tail - 1;
            int x = Q[tail];
            Print();
            return x;
        }

        public int Peek()
        {
            if (IsEmpty()) throw new Exception("Underflow: Черга порожня");
            return Q[head];
        }

        public int PeekBack()
        {
            if (IsEmpty()) throw new Exception("Underflow: Дек порожній");
            int lastIndex = (tail == 1) ? n : tail - 1;
            return Q[lastIndex];
        }

        private bool IsFull() => (tail % n) + 1 == head;
        private bool IsEmpty() => head == tail;

        private void Print()
        {
            Console.Write("Черга/Дек: ");
            int temp = head;
            while (temp != tail)
            {
                Console.Write($"{Q[temp]} ");
                temp = (temp % n) + 1;
            }
            Console.WriteLine();
        }
    }

    public class SetOfStacks
    {
        private List<Stack> stacks = new List<Stack>();
        private int threshold;

        public SetOfStacks(int threshold) => this.threshold = threshold;

        public void Push(int val)
        {
            if (stacks.Count == 0 || stacks[stacks.Count - 1].Count >= threshold)
                stacks.Add(new Stack(10));

            stacks[stacks.Count - 1].Push(val);
            Console.WriteLine($"Додано {val} у стек {stacks.Count}");
        }

        public int Pop()
        {
            if (stacks.Count == 0) throw new Exception("Empty");
            int val = stacks[stacks.Count - 1].Pop();
            if (stacks[stacks.Count - 1].Count == 0) stacks.RemoveAt(stacks.Count - 1);
            return val;
        }
    }

    public class TwoStacks
    {
        private int[] arr;
        private int top1, top2;

        public TwoStacks(int n)
        {
            arr = new int[n];
            top1 = -1;
            top2 = n;
        }

        public void Push1(int x)
        {
            if (top1 < top2 - 1) arr[++top1] = x;
            else Console.WriteLine("Overflow: Масив переповнений");
        }

        public void Push2(int x)
        {
            if (top1 < top2 - 1) arr[--top2] = x;
            else Console.WriteLine("Overflow: Масив переповнений");
        }

        public int Pop1()
        {
            if (top1 >= 0) return arr[top1--];
            throw new Exception("Underflow: Стек 1 порожній");
        }

        public int Pop2()
        {
            if (top2 < arr.Length) return arr[top2++];
            throw new Exception("Underflow: Стек 2 порожній");
        }
    }

    public class QueueUsingStacks
    {
        private Stack s1 = new Stack(10);
        private Stack s2 = new Stack(10);

        public void Enqueue(int x) => s1.Push(x);

        public int Dequeue()
        {
            if (s1.Count == 0 && s2.Count == 0) throw new Exception("Underflow: Черга порожня");
            if (s2.Count == 0)
            {
                while (s1.Count > 0) s2.Push(s1.Pop());
            }
            return s2.Pop();
        }
    }

    static void RunStackTask()
    {
        var s = new Stack(6);
        s.Push(4); 
        s.Push(1); 
        s.Push(3);
        s.Pop();
        s.Push(8);
        s.Pop();
    }

    static void RunQueueTask()
    {
        var q = new Deque(6);
        q.Enqueue(4); 
        q.Enqueue(1); 
        q.Enqueue(3);
        q.Dequeue();
        q.Enqueue(8);
        q.Dequeue();
    }

    static void RunDequeTask()
    {
        var dq = new Deque(6);
        Console.WriteLine("Вставка в початок (10), вставка в кінець (20):");
        dq.PushFront(10);
        dq.Enqueue(20);
        Console.WriteLine($"Видалено з кінця: {dq.PopBack()}");
    }

    static void RunSetOfStacksTask()
    {
        var sos = new SetOfStacks(2);
        sos.Push(1); 
        sos.Push(2); 
        sos.Push(3); 
        sos.Push(4);
        Console.WriteLine($"Видалено: {sos.Pop()}");
        Console.WriteLine($"Видалено: {sos.Pop()}");
    }

    static void RunTwoStacksTask()
    {
        var ts = new TwoStacks(5);
        ts.Push1(10); 
        ts.Push2(20); 
        ts.Push1(30); 
        ts.Push2(40); 
        ts.Push1(50);
        Console.WriteLine("Спроба додати в переповнений масив:");
        ts.Push1(60);
        Console.WriteLine($"Pop з першого: {ts.Pop1()}");
        Console.WriteLine($"Pop з другого: {ts.Pop2()}");
    }

    static void RunQueueUsingStacksTask()
    {
        var q = new QueueUsingStacks();
        q.Enqueue(10); 
        q.Enqueue(20); 
        q.Enqueue(30);
        Console.WriteLine($"Dequeue: {q.Dequeue()} (Очікується 10)");
        Console.WriteLine($"Dequeue: {q.Dequeue()} (Очікується 20)");
    }

    // --- ПРАКТИЧНІ АЛГОРИТМІЧНІ ЗАВДАННЯ ---

    static void RunParenthesesTask()
    {
        Console.Write("Введіть послідовність дужок (наприклад, (()) або )() ): ");
        string input = Console.ReadLine() ?? "";
        int balance = 0;
        bool possible = true;

        foreach (char c in input)
        {
            if (c == '(') balance++;
            else if (c == ')') balance--;

            if (balance < 0) { possible = false; break; }
        }
        if (balance != 0) possible = false;

        if (possible) Console.WriteLine("Так, можна додати цифри/знаки, щоб вийшов правильний вираз.");
        else Console.WriteLine("Ні, з цієї послідовності неможливо утворити правильний вираз.");
    }

    static void RunMergeQueuesTask()
    {
        Deque q1 = new Deque(10);
        q1.Enqueue(1);
        q1.Enqueue(3);
        q1.Enqueue(5);
        q1.Enqueue(7);
        Deque q2 = new Deque(10);
        q2.Enqueue(2);
        q2.Enqueue(4);
        q2.Enqueue(6);
        q2.Enqueue(8);
        Deque result = new Deque(10);

        Console.WriteLine("Черга 1: 1, 3, 5, 7");
        Console.WriteLine("Черга 2: 2, 4, 6, 8");

        while (q1.Count > 0 && q2.Count > 0)
        {
            if (q1.Peek() <= q2.Peek()) result.Enqueue(q1.Dequeue());
            else result.Enqueue(q2.Dequeue());
        }
        while (q1.Count > 0) result.Enqueue(q1.Dequeue());
        while (q2.Count > 0) result.Enqueue(q2.Dequeue());

        Console.Write("Об'єднана черга: ");
        while (result.Count > 0) Console.Write(result.Dequeue() + " ");
        Console.WriteLine();
    }

    static void RunBuildingsTask()
    {
        Console.Write("Введіть висоти споруд через пробіл (приклад: 6 8 2 3 11 11 10): ");
        string[] inputs = Console.ReadLine()?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (inputs == null || inputs.Length == 0) return;

        int[] heights = new int[inputs.Length];
        for (int i = 0; i < inputs.Length; i++) heights[i] = int.Parse(inputs[i]);

        List<int> visible = new List<int>();
        int maxHeight = -1;

        foreach (int h in heights)
        {
            if (h > maxHeight)
            {
                visible.Add(h);
                maxHeight = h;
            }
        }

        Console.WriteLine($"Кількість видимих споруд: {visible.Count}");
        Console.WriteLine($"Висоти видимих споруд: {string.Join(" ", visible)}");
    }
}
