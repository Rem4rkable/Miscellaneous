using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace learn
{

    class HashTableOpenAddressing
    {
        readonly double phi = (Math.Sqrt(5)-1)/2;
        int[] table;
        int n;
        int m;
        public delegate int HashFunction(int element);
        HashFunction function;
        HashFunction secondFunction;
        private int modHashFunc(int num) {return (int)(num % m);}
        private int multHashFunc(int num) {return (int)(m*(num*phi-(int)(num*phi)));}
        public HashTableOpenAddressing() {table = new int[13];n=0;m=table.Length;
            function  = modHashFunc;}
        public HashTableOpenAddressing(int size) {table = new int[size];n=0;m=table.Length;
           function = multHashFunc;}
        public HashTableOpenAddressing(int size,HashFunction funcToUse) {table = new int[size];n=0;m=table.Length;
            function = funcToUse;}

        public void insert(int element)
        {      
            int position = function(element);
            int counter = m;
            while (0 < table[position] && counter > 0)
                {
                    position = function(element+1);
                    counter--;
                }
            if (1 > table[position] && counter > 0)
                table[position] = element;
            else
            Console.WriteLine("Inserting element wasn't successful.");
        }
        public int find(int element)
        {
            int position = function(element);
            int counter = m;
            while (0 != table[position] && counter > 0)
                {
                    if (table[position] == element)
                        return position;
                    else 
                    {
                        position = function(element+1);
                        counter--;  
                    }
                }
            return -1;
        }
        public void remove(int element)
        {
            int position = find(element);
            if (-1 == position)
                Console.WriteLine("removing was unsuccessful.");
            else
                table[position]=-1;
        }
        public void print()
        {
            for (int i=0;i<m;i++)
                Console.WriteLine("["+i+"]->    "+table[i]);
        }
    }
    class Heap
    {
        int[] heap;
        bool isMax;
        int heapSize;
        public Heap() {heap = new int[3];}
        public Heap(int element,bool type = false) 
        {
            heap = new int[3];
            isMax = type;
            heap[heapSize] = element;
            heapSize++;
        }
        public Heap(int[] array,bool type =false)
        {
            heap = array;
            isMax = type;
            heapSize = heap.Length;
            for (int i =heap.Length/2-1;i>=0;i--)
                fixHeapDown(i);
        }
        int Left(int i) {return 2*i + 1;}
        int Right(int i) {return 2*i + 2;}
        int Parent(int i) {return (i-1)/2;}
        public void insert(int element)
        {
            if (heapSize == heap.Length)
            {
                Console.WriteLine("resizing heap..");
                Array.Resize(ref heap, heap.Length*2 +1);
            }
            heap[heapSize] = element;
            fixHeap(heapSize);
            heapSize++;
        }
        public void delete(int element)
        {
            int index = find(element);
            if (-1 != index)
            {
                Program.Swap(ref heap[heapSize-1],ref heap[index]);
                heapSize--;
                fixHeapDown(index);
            }
        }
        public int find(int element)
        {
            for (int i =0; i<heapSize; i++)
                if (heap[i] == element)
                    return i;
            return -1;
        }
        public int extractRoot()
        {
            if (0 == heapSize)
                return -1;
            int element = heap[0];
            delete(heap[0]);
            return element;
        }
        private void fixHeap(int current)
        {
            if (isMax)
                {
                    while (current!= 0 && heap[current]>heap[Parent(current)])
                    {
                        Program.Swap(ref heap[current], ref heap[Parent(current)]);
                        current = Parent(current);
                    }
                }
                else
                {
                    while (current!= 0 && heap[current]<heap[Parent(current)])
                    {
                        Program.Swap(ref heap[current], ref heap[Parent(current)]);
                        current = Parent(current);
                    }
                }
        }
        enum Type {
            Leaf,
            OnlyLeft,
            Both
        }
        Type kindOf(int index)
        {
            if (Left(index) >= heapSize)
                return Type.Leaf;
            else if( Right(index)>=heapSize)
                return Type.OnlyLeft;
            return Type.Both;
        }
        bool holdsTrue(int index)
        {
            if (isMax)
            {
                if (kindOf(index)==Type.Leaf ||
                (kindOf(index)==Type.OnlyLeft && heap[index]>heap[Left(index)]) ||
                (kindOf(index)==Type.Both && heap[index]>heap[Left(index)] && heap[index]>heap[Right(index)]))
                    return true;
                return false;
            }
            else
            {
                if (kindOf(index)==Type.Leaf ||
                (kindOf(index)==Type.OnlyLeft && heap[index]<heap[Left(index)]) ||
                (kindOf(index)==Type.Both && heap[index]<heap[Left(index)] && heap[index]<heap[Right(index)]))
                    return true;
                return false;
            }
        }
        private void fixHeapDown(int current)
        {
            if (isMax)
                {
                    while (!holdsTrue(current))
                    {
                        if (kindOf(current) == Type.OnlyLeft)
                        {
                            if (heap[current]<=heap[Left(current)])
                            {
                                Program.Swap(ref heap[current], ref heap[Left(current)]);
                                current = Left(current);
                            }
                        }
                        else if (heap[Left(current)] > heap[Right(current)])
                        {
                            Program.Swap(ref heap[current], ref heap[Left(current)]);
                            current = Left(current);
                        }
                        else
                        {
                            Program.Swap(ref heap[current], ref heap[Right(current)]);
                            current = Right(current);
                        }
                    }
                }
                else
                {
                     while (!holdsTrue(current))
                    {
                        if (kindOf(current) == Type.OnlyLeft)
                        {
                            if (heap[current]>=heap[Left(current)])
                            {
                                Program.Swap(ref heap[current], ref heap[Left(current)]);
                                current = Left(current);
                            }
                        }
                        else if (heap[Left(current)] < heap[Right(current)])
                        {
                            Program.Swap(ref heap[current], ref heap[Left(current)]);
                            current = Left(current);
                        }
                        else
                        {
                            Program.Swap(ref heap[current], ref heap[Right(current)]);
                            current = Right(current);
                        }
                    }
                }
        }
        public void print()
        {
            Console.WriteLine("Type of heap is "+(isMax? "max." : "min."));
            for (int i=0;i<=(int)(Math.Log(heap.Length+1));i++)
                {
                    for (int j=(int)(Math.Pow(2,i)-1);j<(int)(Math.Pow(2,i+1)-1) && j<heapSize;j++)
                        Console.Write(heap[j]+" ");
                    Console.WriteLine();
                }
        }
        public System.Collections.Generic.IEnumerable<int> HeapSort()
        {
            int size = heapSize;
            for (int i=0;i<size;i++)
                yield return extractRoot();
        }
    }
    class iQueue<T>
    {
        T[] queue;
        int index;
        int numOfElements;
        public iQueue() {queue=new T[5]; index=0; numOfElements = 0;}
        public T dequeue()
        {
            if (0 == numOfElements)
                Console.WriteLine("There are no elements.");
            T element = queue[index];
            numOfElements--;
            index = index+1 % queue.Length;
            return element;
        }
        public void enqeueue(T element)
        {
            if ((index+numOfElements) % queue.Length == index && 0 != numOfElements)
                //Array.resize ->> Check which size from the index is smaller,
                //                  and attach that part to the end so the sequence
                //                  will be saved.
                {Console.WriteLine("Queue overflow!"); return;}
            else
                queue[(index+numOfElements) % queue.Length] = element;
                numOfElements++;

        }
         public void print()
        {
            Console.Write("Queue print: "+queue[index]);
            for (int i=index+1;i<index+numOfElements;i++)
                Console.Write(","+queue[i%queue.Length]);
            Console.WriteLine();
            Console.WriteLine("Queue size is: "+queue.Length+"; Real size is: "+(numOfElements)+".");

        }
    }
    class Stack<T>
    {
        T[] stack;
        int index;
        public Stack() {stack = new T[1];index = -1;}
        public T pop()
        {
            T element = stack[index];
            //stack[index--] = default(T); this line is unnecessary
            return element;
        }
        public T peek()
        {
            return stack[index];
        }
       public void push(T element)
        {
            if (index+1 > stack.Length-1)
                Array.Resize(ref stack,stack.Length*2);
            index++;
            stack[index] = element;
            Console.WriteLine("Element push successful");
        }
        public void print()
        {
            Console.Write("Stack print: "+stack[0]);
            for (int i=1;i<=index;i++)
                Console.Write(","+stack[i]);
            Console.WriteLine();
            Console.WriteLine("Stack size is: "+stack.Length+"; Real size is: "+(index+1)+".");

        }
    }
    class GraphMatrix
    {
        int numOfVertices;
        bool[,] matrix;
        public GraphMatrix() {numOfVertices = 0;}
        public GraphMatrix(int val) {numOfVertices = val; matrix = new bool[numOfVertices,numOfVertices];}

        public bool isEdgeBetween(int V, int U)
        {
            if (matrix[V,U])
                return true;
            return false;
        }
        public void addEdge(int V, int U)
        {
            if (!matrix[V,U])
               matrix[V,U] = true;
        }
        public void removeEdge(int V, int U)
        {
            if (matrix[V,U])
               matrix[V,U] = false;
        }
        public void addVertex(int V)
        {
            int num = numOfVertices;
            bool[,] mat = new bool[num+1,num+1];
            for (int i=0;i<num;i++)
                for(int j=0;j<num;j++)
                    mat[i,j] = matrix[i,j];
            matrix = mat;
            numOfVertices +=1;
        }
        public void print()
        {
            for (int i=0;i<numOfVertices;i++)
            {
                Console.Write(matrix[i,0]);
                for(int j=1;j<numOfVertices;j++)
                    Console.Write(","+matrix[i,j]);
                Console.WriteLine();
            }
        }
    }
    class Trie
    {
        static string Sequence = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        class Node
        {
            public char Value {get; set;}
            public Node[] children;
            public Node this[char child]
            {
                get => getChild(child);
                set => setChild(child);
            }
            public Node getChild(char child)
            {
                if (children[(int)(child)-65] == null)
                    return null;
                else
                    return children[(int)(child)-65];
            }
            public void setChild(char child)
            {
                if (children[(int)(child)-65] == null)
                    children[(int)(child)-65] = new Node(child);
                else
                    Console.WriteLine("Letter "+child+" already exists.");
            }
            public Node() { children = new Node[26];}
            public Node(char val)
            {
                Value = val;
                children = new Node[26];
            }
        }
        Node head;
        public Trie() {head = new Node();}
        public void addWord(string word)
            {
                Node current = head;
                for (int i=0; i<word.Length;i++)
                {
                    if (current[word[i]] == null)
                        current.setChild(word[i]);
                    current = current[word[i]];
                }                
            }
        public void findWord(string word)
        {
            Node current = head;
            for (int i=0; i<word.Length;i++)
            {
                if (current[word[i]] == null)
                {
                    Console.WriteLine("The word:"+word+" is not in the Trie.");
                    return;
                }
                current = current[word[i]];
            }
            Console.WriteLine("Success! The word:"+word+" is in the Trie.");      
        }
        public void printTrie()
        {
            Console.WriteLine("Work");
        }

    }
    class Node
    {
        public int Value {get; set;}
        public Node Left {get; set;}
        public Node Right {get; set;}

        public Node() {}
        public Node(int val)
        {
            Value = val;
        }
        public Node(int val,Node l)
        {
            Value = val;
            Left = l;
        }
        public Node(int val,Node l,Node r)
        {
            Value = val;
            Left = l;
            Right = r;
        }

    }
    class BST
    {
       public Node root;
       public BST (Node r){ root = r;}
       public BST (int val) { root = new Node(val); Console.WriteLine("constructor value");}

       public void printTree()
       {
           printTree(root);
           Console.WriteLine("Done printTree");
       }
       public void printTree(Node root)
       {
           if (root == null)
            return;
           Console.Write(root.Value+",");
           printTree(root.Left);
           printTree(root.Right);
       }
       public void BFS ()
       {
           BFS(root);
       }
       public void BFS (Node root)
       {
          if (root == null)
            return;
          Queue<Node> node_queue = new Queue<Node>();
          node_queue.Enqueue(root);
          Node temp = new Node();
          while (node_queue.Count !=0 )
          {
              temp = node_queue.Dequeue();
              Console.Write(temp.Value+",");  
              if (temp.Left != null)
                node_queue.Enqueue(temp.Left);
              if (temp.Right != null)
                node_queue.Enqueue(temp.Right);
          }
          Console.WriteLine("Done BFS");
       }
       
        public void insert (int v)
       {
           if (root == null)
                root = new Node(v);
           else
           {
                Node temp = root;
                while (temp!=null)
                {
                    if (v <= temp.Value)
                    {
                        if (temp.Left == null) {
                            temp.Left = new Node(v);
                            return;
                        }
                        else
                            temp = temp.Left;
                    }
                    else if (v > temp.Value)
                    {
                        if (temp.Right == null) {
                            temp.Right = new Node(v);
                            return;
                        }
                        else
                            temp = temp.Right;
                    }
                }
            }
       }
    }
    class Program
    {
      static void bubbleSort(int []arr) 
    { 
        int n = arr.Length; 
        for (int i = 0; i < n - 1; i++) 
            for (int j = 0; j < n - i - 1; j++) 
                if (arr[j] > arr[j + 1]) 
                { 
                    // swap temp and arr[i] 
                    int temp = arr[j]; 
                    arr[j] = arr[j + 1]; 
                    arr[j + 1] = temp; 
                } 
    } 

    static int findMax(int []arr)
    {
        int max = arr[0];
        for (int i=1;i<arr.Length;i++)
            if (arr[i]>max)
                max = arr[i];
        return max;

    }
    /* Prints the array */
    static void printArray(int []arr) 
    { 
        int n = arr.Length; 
        for (int i = 0; i < n; ++i) 
            Console.Write(arr[i] + " "); 
        Console.WriteLine(); 
    }

    static string reverseString(string str)
    {
        char[] arr = str.ToCharArray();
        for (int i=str.Length-1;i>=str.Length/2;i--)
            Swap<char>(ref arr[str.Length-1-i],ref arr[i]);
        return new String(arr);
    }
    static char[] reverseStringRecursive(char[] str)
    {
        if (str.Length == 1)
            return str;
        else if (str.Length == 2)
            return new char[] {str[1],str[0]};
        else return str;
    }

    public static void Swap<T>(ref T lhs, ref T rhs)
    {
        T temp;
        temp = lhs;
        lhs = rhs;
        rhs = temp;
    }
    static void permuteString(char[] str)
    {
        return;
    }
    static bool uniqueChar(string str)
    {
        int uniqCheck,val;
        uniqCheck = val = 0;
        for (int i=0; i<str.Length;i++)
        {
            val = str[i] - 'a';
            if ((uniqCheck & (1 << val)) > 0)
                return false;
            else
                uniqCheck |= (1 << val);
        }
        return true;
    }

    static bool isPermutation(string baseString, string perm)
    {
        int[] letters = new int [80];
        if (baseString.Length != perm.Length)
            return false;
        for (int i=0; i<baseString.Length;i++)
            letters[baseString[i] - 'a']++;
        for (int i=0; i<perm.Length;i++)
        {
            if (letters[perm[i] - 'a'] > 0)
                letters[perm[i] - 'a']--;
            else 
                return false;
        }
        return true;
    }

    static int multi(int a, int b)
    {
        int sum = 0;
        return multi(sum,a,b);
    }
    static int multi(int sum,int a, int b)
    {
        if (a == 0)
            return sum;
        else
        {
            sum += b;
            return multi(sum,a-1,b);
        }
    }
    static void fibolichi(int n) 
    {
        int x0 = 0;
        int x1 = 1;
        int sum = 0;
        for (int i = 3 ; i< n ; i++)
        {
             sum = x1 + x0;
             x0 = x1;
             x1 = sum;
        }
        Console.WriteLine(sum);
    }
    public static int BinarySearch(int[] sortedArray,int element)
    {
        return innerBinarySearch(sortedArray,element,0,sortedArray.Length-1);

    }
    private static int innerBinarySearch(int[] sortedArray, int element,int start,int end)
    {
        int middle = start + (end - start) /2; 
        if (start>end)
            return -1;
        if (sortedArray[middle]>element)
            return innerBinarySearch(sortedArray,element,start,middle-1);
        if (sortedArray[middle]<element)
            return innerBinarySearch(sortedArray,element,middle+1,end);
        return middle;
    }
    public static int BinaryNoRecursion(int[] sortedArray,int element)
    {
        int middle;
        int left = 0; 
        int right=sortedArray.Length-1;
        while (left<=right)
        {
            middle = left + (right-left)/2;
            if (sortedArray[middle]>element)
                right = middle-1;
            else if (sortedArray[middle]<element)
                left = middle+1;
            else
                return middle;
        }
        return -1;
    }
    public static void Mergesort(int[] array) {
        int[] temp = new int[array.Length];
        innerMergesort(array,0,array.Length-1,temp);
    }
    public static void innerMergesort(int[] array,int left,int right,int[] temp)
    {
        if (left >= right)
            return;
        int middle = left + (right-left)/2;
        innerMergesort(array,left,middle,temp);
        innerMergesort(array,middle+1,right,temp);
        partition(array,left,right,temp);
    }
    public static void partition(int[] array,int left,int rightEnd,int[] temp)
    {
        int leftPos = left;
        int leftEnd = left + (rightEnd-left)/2;
        int right = leftEnd+1;
        int index = left;
        while (left<=leftEnd && right<=rightEnd)
        {
            if (array[left]<=array[right])
            {
                temp[index] = array[left];
                left++;
            }
            else
            {
                temp[index] = array[right];
                right++;
            }
            index++;
            
        }
        if (left >leftEnd)
            for (int i=index;right<rightEnd+1;i++,right++)
                temp[i]=array[right];
        else if (right >rightEnd)
            for (int i=index;left<leftEnd+1;i++,left++)
                temp[i]=array[left];
        for (int i=leftPos;i<=rightEnd;i++)
            array[i]=temp[i];

    }
    public static void Quicksort(int[] array) {
        innerQuicksort(array,0,array.Length-1,array.Length/2);
     }
    public static void innerQuicksort(int[] array,int left, int right,int pivotIndex) {
        if (left>=right)
            return;
        int pivot = array[pivotIndex];
        int leftPos = left;
        int rightPos = right;
        while (left <= right)
        {
            while (array[left]<pivot)
                left++;
            while (array[right]>pivot)
                right--;
            if(left <= right)
            {
                Swap(ref array[left],ref array[right]); 
                left++;
                right--;
            }
        }
        innerQuicksort(array,leftPos,left-1,leftPos + (left-1-leftPos)/2);
        innerQuicksort(array,left,rightPos,left + (rightPos-left)/2);
    }
    // From here starts Codility Tasks.
    public static int BinaryGap(int N) {
        int count = 0;
        while (0 != N)
        {
            if((N&1)==1)
            {
                int gapCount = 0;
                N = N << 1;
                while((N&1)==0 && N!=0)
                {
                    gapCount++;
                    N = N<<1;
                }
                if (gapCount > count)
                    count = gapCount;
            }
            N = N << 1;
        }
        return count;
    }
    public int[] CyclicRotation(int[] A, int K) {
        // write your code in C# 6.0 with .NET 4.5 (Mono)
        if (A.Length == 0)
            return A;
        int k = K%A.Length;
        int[] kArray = new int[A.Length];
        for (int i=0;i<kArray.Length;i++)
            kArray[i]=A[(i+A.Length-k)%A.Length];
        return kArray;
    
    }
    public int OddOccurrencesInArray(int[] A) {
        // write your code in C# 6.0 with .NET 4.5 (Mono)
        if (A.Length < 2)
            return A[0];
        Quicksort(A);
        int current=0;
        int count = 1;
        while (current+1 < A.Length)
        {
            if (A[current]==A[current+1])
                count++;
            else
            {
                if ((count%2)==1)
                    return A[current];
                else
                {
                    count = 1;
                }
            }
            current++;
        }
        return A[current];
        // Interesting - can XOR for the answer:
        /*
        int result = 0;
        foreach (int i in A)
            result = result ^ A[i];
        */

    }
     public static int Chips(int[] A) {
        if (A.Length == 0)
            return 1;
        int smallest = 1;
        Quicksort(A);
        int index = 0;
        while (A[index]<=0)
        {
            index++;
            Console.WriteLine(index);
            Console.WriteLine(A.Length);
        }
        if (index == A.Length)
            return 1;
        for (int i=index;i<A.Length;i++)
        {
            if (smallest<A[i])
            {
                return smallest;
            }
            else if (smallest == A[i])
            {
                while (A[i]==(smallest-1)) {
                    i++;
                }
                smallest++;
            }
        }
        return smallest;
    }
    public static int sol(int a)
    {return a;}
       public int solution(int N) {
        // write your code in C# 6.0 with .NET 4.5 (Mono)
        //It looks like the position in which the 5 should be put
        //is actually right after the sequnce of numbers bigger than 5 is.
        //For negatives, it's the opposite. (If I understand correctly that we're
        //talking about the highest positive and not maximizing)
        try {
        string digits = N.ToString();
        if (digits[0] == '-')
            for (int i=1;i<digits.Length;i++)
                if(5 < Int32.Parse(digits[i]))
                {
                    digits = digits.Insert(i,"5");
                    break;
                }
        else
            for (int j=0;j<digits.Length;j++)
                if(5 >Int32.Parse(digits[j])) {
                    digits = digits.Insert(j,"5");
                        break;
                }
        int num = 0;
        
            num = Int32.Parse(digits);
        }
        catch {}
        return num;
    }
    // Driver method 
    public static void Main() 
    {
        int[] a = {-1,-3};
        a.Min();
        string[] daysOfWeek = {"Mon","Tue","Wed","Thu","Fri",
            "Sat","Sun"};
        daysOfWeek.ToList().IndexOf();
        Console.WriteLine(Chips(a));
        /*
        int[] manera ={44,56,8,74,3,8,4,41,3,87,5,41,6,6,35,6,65,44,56,654,45};
        Quicksort(manera);
         foreach (int element in manera)
           Console.Write(element+",");
        
        
        Heap maxheap = new Heap(5,true);
        Heap minheap = new Heap(5,false);
        maxheap.insert(7); maxheap.insert(4); maxheap.insert(3); maxheap.insert(8);
        maxheap.insert(1); maxheap.insert(2);
        minheap.insert(7); minheap.insert(4); minheap.insert(3); minheap.insert(8);
        minheap.insert(1); minheap.insert(2);
        maxheap.print();minheap.print();
        maxheap.delete(4);maxheap.insert(9);maxheap.delete(2);
        minheap.delete(4);minheap.insert(9);minheap.delete(2);
        maxheap.print();minheap.print();

        Heap checkMax = new Heap(new int[] {44,56,8,74,3,8,4,41,3,87,5,41,6,6,35,6,65,44,56,654,45},true);
        Heap checkMin = new Heap(new int[] {44,56,8,74,3,8,4,41,3,87,5,41,6,6,35,6,65,44,56,654,45},false);
        foreach (int element in checkMax.HeapSort())
           Console.Write(element+",");
        Console.WriteLine();
        foreach (int element in checkMin.HeapSort())
           Console.Write(element+",");
        
        
        int[] manera ={44,56,8,74,3,8,4,41,3,87,5,41,6,6,35,6,65,44,56,654,45};
        Mergesort(manera);
        Array.ForEach(manera, Console.WriteLine);
        
        int[] ortega = {1,4,8,10,18,35,46,55,58,66,69,79,161,331,716,1174,2414,4217,6161,6178};
        Console.WriteLine(BinaryNoRecursion(ortega,19));
        Console.WriteLine(BinaryNoRecursion(ortega,161)); // should be 12
        Console.WriteLine(BinaryNoRecursion(ortega,1));
        Console.WriteLine(BinaryNoRecursion(ortega,6178));
        Console.WriteLine(BinaryNoRecursion(ortega,4));
        
        HashTableOpenAddressing ht = new HashTableOpenAddressing(9);
        ht.insert(8);ht.insert(18);ht.insert(43);ht.insert(19);
        ht.insert(32);ht.insert(58);ht.insert(66);ht.insert(2);
        ht.print();
        Console.WriteLine(ht.find(32));
        Console.WriteLine(ht.find(87));
        ht.remove(19); ht.remove(8); ht.remove(63);
        ht.print();
        
        Heap maxheap = new Heap(5,true);
        Heap minheap = new Heap(5,false);
        maxheap.insert(7); maxheap.insert(4); maxheap.insert(3); maxheap.insert(8);
        maxheap.insert(1); maxheap.insert(2);
        minheap.insert(7); minheap.insert(4); minheap.insert(3); minheap.insert(8);
        minheap.insert(1); minheap.insert(2);
        maxheap.print();minheap.print();
        maxheap.delete(4);maxheap.insert(9);maxheap.delete(2);
        minheap.delete(4);minheap.insert(9);minheap.delete(2);
        maxheap.print();minheap.print();


        
        iQueue<int> stack  = new iQueue<int>();
        stack.print();
        stack.enqeueue(5);stack.enqeueue(10);stack.enqeueue(53);
        stack.enqeueue(99);stack.enqeueue(0);stack.enqeueue(0);
        stack.print();
        stack.dequeue();stack.dequeue();
        stack.print();
        stack.enqeueue(83);
        stack.print();
         
        Stack<int> stack  = new Stack<int>();
        stack.print();
        stack.push(5);stack.push(10);stack.push(53);stack.push(99);stack.push(0);
        stack.print();
        stack.peek();
        stack.pop();stack.pop();
        stack.print();
       
        GraphMatrix graph = new GraphMatrix(3);
        graph.addEdge(1,2);
        graph.addEdge(0,1);
        graph.addEdge(0,2);
        graph.removeEdge(0,1);
        graph.addVertex(5);
        graph.print();
        
        Trie trie = new Trie();
        trie.addWord("ABA");
        trie.addWord("BWT");
        trie.addWord("ATOP");
        trie.findWord("BWT");
        trie.findWord("BWW");
        int x = 5;
        int y = 4;
        System.Console.WriteLine(multi(x,y));
         string s = "afbzshf"; 
  
        if (uniqueChar(s)) 
            Console.WriteLine("Yes"); 
        else
            Console.WriteLine("No"); 

        string a = "dudedu";
        string b = "deudud";
        if (isPermutation(a,b)) 
            Console.Write("Permutation"); 
        else
            Console.Write("No Permutation"); 
        
        //does BST tree, then insertion, printing (inorder) and BFS.
        BST tree = new BST(50);
        tree.insert(51);
        tree.insert(12);
        tree.insert(54);
        tree.insert(33);
        tree.insert(24);
        tree.insert(86);
        tree.insert(10);
        tree.printTree();
        tree.BFS();
        
        int []arr = {64, 34, 25, 12, 22, 11, 90};
        char[] perm_str = {'a','b','c'};
        string front = "mansion yeh hey";
        //reversing string
        string backwards = reverseString(front);
        Console.WriteLine(backwards);
        Console.WriteLine(findMax(arr));
        bubbleSort(arr); 
        Console.WriteLine("Sorted array"); 
        printArray(arr); 
         */
    } 
    }
}
