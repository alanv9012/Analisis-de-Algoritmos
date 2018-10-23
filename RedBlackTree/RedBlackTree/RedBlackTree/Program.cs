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
            Console.WriteLine(Value + " " + Color);
            if(Left != null)
            {
                Console.WriteLine("Left");
                Left.Print();                
            }
            if(Right != null)
            {
                Console.WriteLine("Right");
                Right.Print();
            }
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
        private static Node _root;
        private static bool _wasSolutionFound;
        
        public static Node Build(List<int> elements, int blackHeight)
        {
            _elements = elements;    
            _elements.Sort();
            
            var trees = GenerateTrees(0, _elements.Count - 1);

            foreach (var tree in trees)
            {
                if(CanBeColored(tree)) 
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
                
        public static bool CanBeColored(Node root)
        {
            _root = root;
            _wasSolutionFound = false;
            var treeAsList = new List<Node>();
            GetTreeAsNodeList(root, treeAsList);
            CanBeColoredHelper(treeAsList);
            return _wasSolutionFound;
        }
        
        private static void GetTreeAsNodeList(Node node, List<Node> nodes)
        {
            if(node == null)
                return;
            nodes.Add(node);
            
            GetTreeAsNodeList(node.Left, nodes);
            GetTreeAsNodeList(node.Right, nodes);
        }
        
        private static void CanBeColoredHelper(List<Node> nodes, int currentIndex = 0)
        {
            if(currentIndex == nodes.Count)
            {
                if(nodes[0].IsValid())
                    _wasSolutionFound = true;
                return;
            }
            nodes[currentIndex].Color = Color.Black;
            CanBeColoredHelper(nodes, currentIndex + 1);
            nodes[currentIndex].Color = Color.Red;
            CanBeColoredHelper(nodes, currentIndex + 1);
        }
    }
        
    internal class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Introducir la cantidad de elementos");
            var amountOfElements = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Introducir los elementos");
            var elements = new List<int>();
            for (var i = 0; i < amountOfElements; i++)
                elements.Add(Convert.ToInt32(Console.ReadLine()));
            Console.WriteLine("Introducir altura black del arbol");
            var blackHeight = Convert.ToInt32(Console.ReadLine());
            
            var root = RedBlackTreeBuilder.Build(elements, blackHeight);
            
            if(root == null)
            {
                Console.WriteLine("No hay solucion");
                return;                
            }

            Console.WriteLine("La solucion es");
            root.Print();
        }
    }
}