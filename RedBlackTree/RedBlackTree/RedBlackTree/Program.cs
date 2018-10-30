using System;
using System.Collections.Generic;

namespace RedBlackTree
{
    public enum Color
    {
        Red,
        Black
    }

    public class Node
    {
        public readonly int Value;
        public Color Color;
        public Node Left, Right;
        
        public Node(int value, Node left, Node right)
        {
            Value = value;
            Left = left;
            Right = right;
        }

        public Node(int value, Color color, Node left = null, Node right = null)
        {
            Value = value;
            Left = left;
            Right = right;
            Color = color;    
        }
        
        public int GetBlackHeight()
        {
            return GetBlackHeightFromChildNode(Left);
        }
        
        public void Print()
        {
            Console.Write(Value + " " + Color);
            if (Left != null)
                Console.Write(" Left " + Left.Value + " ");
            if (Right != null)
                Console.Write(" Right " + Right.Value + " ");
            Console.WriteLine();
            Left?.Print();
            Right?.Print();
        }
        
        public bool IsValid()
        {
            if(Color != Color.Black)
                return false;
            if(!AreChildColorsValid())
                return false;
            var blackHeight = GetBlackHeight();
            if(!AreHeightsCorrect(blackHeight, 0))
                return false;
            return true;
        }

        private bool AreHeightsCorrect(int expected, int heightSum)
        {
            if (Left != null && !Left.AreHeightsCorrect(expected, Left.Color == Color.Black ? heightSum + 1 : heightSum))
                return false;
            if (Left == null && expected != heightSum + 1)
                return false;
            if (Right != null && !Right.AreHeightsCorrect(expected, Right.Color == Color.Black ? heightSum + 1 : heightSum))
                return false;
            if (Right == null && expected != heightSum + 1)
                return false;
            return true;
        }

        private bool AreChildColorsValid()
        {
            if (Color == Color.Red)
            {
                if (Left != null && Left.Color == Color.Red)
                    return false;
                if (Right != null && Right.Color == Color.Red)
                    return false;
            }
            if (Left != null && !Left.AreChildColorsValid())
                return false;
            if (Right != null && !Right.AreChildColorsValid())
                return false;
            return true;
        }

        private int GetBlackHeightFromChildNode(Node node)
        {            
            if(node != null)
                return node.Color == Color.Black ? node.GetBlackHeight() + 1 : node.GetBlackHeight();
            return 1;
        }
    }
    
    public static class RedBlackTreeBuilder
    {
        private static List<int> _elements;
        private static int _targetBlackHeight;
        
        public static Node Build(List<int> elements, int blackHeight)
        {
            _targetBlackHeight = blackHeight;
            _elements = elements;    
            _elements.Sort();
            
            var trees = GenerateTrees(0, _elements.Count - 1);

            foreach (var tree in trees)
            {
                if(CanBeColoredWithHeight(tree)) 
                    return tree;
            }
            return null;
        }
         
        public static List<Node> GenerateTrees(int lower, int upper){
            var result = new List<Node>();
            if(lower>upper){
                result.Add(null);
                return result;
            }
 
            for(var i=lower; i<=upper; i++){
                var leftSide = GenerateTrees(lower, i-1);
                var rightSide = GenerateTrees(i+1, upper);
                foreach(var left in leftSide){
                    foreach(var right in rightSide){
                        result.Add(new Node(_elements[i], left, right));
                    }
                }
            }
 
            return result;
        }
                
        public static bool CanBeColoredWithHeight(Node root)
        {
            var treeAsList = new List<Node>();
            GetTreeAsNodeList(root, treeAsList);
            return CanBeColoredHelper(treeAsList);
        }
        
        private static void GetTreeAsNodeList(Node node, List<Node> nodes)
        {
            if(node == null)
                return;
            nodes.Add(node);
            
            GetTreeAsNodeList(node.Left, nodes);
            GetTreeAsNodeList(node.Right, nodes);
        }
        
        private static bool CanBeColoredHelper(List<Node> nodes, int currentIndex = 0)
        {
            if (currentIndex == nodes.Count)
            {
                if (nodes[0].IsValid() && _targetBlackHeight == nodes[0].GetBlackHeight())
                    return true;
                return false;
            }

            nodes[currentIndex].Color = Color.Black;
            if (CanBeColoredHelper(nodes, currentIndex + 1))
                return true;
            if (currentIndex == 0) // Root must be black
                return false;
            nodes[currentIndex].Color = Color.Red;
            if (CanBeColoredHelper(nodes, currentIndex + 1))
                return true;
            return false;
        }
    }
        
    internal class Program
    {
        private static readonly List<int> _elements = new List<int>();
        
        public static void Main(string[] args)
        {
            Console.WriteLine("Introducir la cantidad de elementos");
            var amountOfElements = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Introducir los elementos");
            for (var i = 0; i < amountOfElements; i++)
                _elements.Add(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Introducir altura black del arbol");
            var blackHeight = Convert.ToInt32(Console.ReadLine());
            
            var root = RedBlackTreeBuilder.Build(_elements, blackHeight);
            if (root != null)
            {
                Console.WriteLine("La solucion es");
                root.Print();
                Console.Read();
                return;
            }

            Console.WriteLine("No tiene solucion para black height = " + blackHeight);
            SearchForSolutionClosestToBlackHeight(blackHeight);
        }
        
        private static void SearchForSolutionClosestToBlackHeight(int blackHeight)
        {
            int lower = blackHeight -1, upper = blackHeight + 1;
            while(true)
            {
                var root = RedBlackTreeBuilder.Build(_elements, lower);
                if (PrintIfNotNull(root, lower))
                    return;
                
                root = RedBlackTreeBuilder.Build(_elements, upper);
                if (PrintIfNotNull(root, upper))
                    return;
                
                lower--;
                upper++;
            }
        }

        private static bool PrintIfNotNull(Node root, int blackHeightFound)
        {
            if (root != null)
            {
                Console.WriteLine("Se encontro solucion para black height = " + blackHeightFound);
                root.Print();
                Console.Read();
                return true;
            }

            return false;
        }
    }
}