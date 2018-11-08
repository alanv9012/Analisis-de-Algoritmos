using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RedBlackTree
{
    public enum Color
    {
        Red,
        Black
    }

    public class Node
    {
        public int Value;
        public readonly int NodeCount;
        public readonly Color Color;
        public readonly int BlackHeight;
        public readonly Node Left, Right;
        
        public Node(Color color, Node left = null, Node right = null)
        {
            if(color == Color.Red)
            {
                if(left != null && left.Color == Color.Red)
                    throw new ArgumentException(nameof(left));
                if(right != null && right.Color == Color.Red)
                    throw new ArgumentException(nameof(right));
            } 
            if(GetBlackHeightOfNode(left) != GetBlackHeightOfNode(right))
                throw new InvalidOperationException("Height of subtrees does not match");
            BlackHeight = GetBlackHeightOfNode(left);
            if(color == Color.Black)
                BlackHeight++;
            NodeCount = GetChildCountOfNode(left) + GetChildCountOfNode(right) + 1;
            Left = left;
            Right = right;
            Color = color;    
        }
        
        private int GetBlackHeightOfNode(Node node)
        {
            return node == null ? 1 : node.BlackHeight;
        }
        
        private int GetChildCountOfNode(Node node)
        {
            return node == null ? 0 : node.NodeCount;
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

        public Node Untangle()
        {
            Node untangledLeft = null, untangledRight = null;
            if(Left != null)
                untangledLeft = Left.Untangle();        
            if(Right != null)
                untangledRight = Right.Untangle();
            return new Node(Color, untangledLeft, untangledRight);
        }
        
        public void FillWithValues(List<int> values)
        {
            var middle = GetChildCountOfNode(Left);
            Value = values[middle];
            Left?.FillWithValues(values, 0,  middle - 1);
            Right?.FillWithValues(values, middle + 1, values.Count);
        }
        
        private void FillWithValues(List<int> values, int from, int to)
        {
            var middle = from + GetChildCountOfNode(Left);
            Value = values[middle];
            Left?.FillWithValues(values, from, middle - 1);
            Right?.FillWithValues(values, middle + 1, to);
        }
    }
    
    public static class RedBlackTreeBuilder
    {
        public class TreeCache
        {
            public class NodeCountToTrees : IEnumerable<Node>
            {
                private class BlackHeightComparer : IComparer<Node>
                {
                    /// <inheritdoc />
                    public int Compare(Node x, Node y)
                    {
                        return x.BlackHeight.CompareTo(y.BlackHeight);
                    }
                }

                private readonly SortedSet<Node> Trees = new SortedSet<Node>(new BlackHeightComparer());
                
                public void TryAddTree(Node tree)
                {
                    if (tree == null || !Trees.Contains(tree))
                        Trees.Add(tree);
                }
                
                public Node GetTreeClosestToBlackHeight(int blackHeight)
                {
                    Node closestMatch = null;
                    int closestMatchDifference = int.MaxValue;
                    foreach (var tree in Trees)
                    {
                        var heightDifference = Math.Abs(tree.BlackHeight - blackHeight);
                        if(heightDifference < closestMatchDifference)
                        {
                            closestMatchDifference = heightDifference;
                            closestMatch = tree;
                        }
                    }
                    return closestMatch;
                }
                
                /// <inheritdoc />
                public IEnumerator<Node> GetEnumerator()
                {
                    foreach (var tree in Trees)
                    {
                        yield return tree;
                    }
                }

                /// <inheritdoc />
                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }

            public readonly List<NodeCountToTrees> TreesWithRedRoot = new List<NodeCountToTrees>();
            public readonly List<NodeCountToTrees> TreesWithBlackRoot = new List<NodeCountToTrees>();

            public TreeCache(int amountOfNodes)
            {
                for (int i = 0; i <= amountOfNodes; i++)
                {
                    TreesWithBlackRoot.Add(BuildTreesWithGivenNodeCount(Color.Black, i));
                    TreesWithRedRoot.Add(BuildTreesWithGivenNodeCount(Color.Red, i));
                }
                
                NodeCountToTrees BuildTreesWithGivenNodeCount(Color rootColor, int nodeCount)
                {
                    if(nodeCount == 0)
                    {
                        if (rootColor == Color.Black)
                        {
                            var nullBaseCase = new NodeCountToTrees();
                            nullBaseCase.TryAddTree(null);
                            return nullBaseCase;
                        }
                        return new NodeCountToTrees();
                    }
                    var trees = new NodeCountToTrees();
                    if (rootColor == Color.Black)
                        BuildTreesWithGivenNodeCountFromSubTrees(EnumerateTreesWithNodeCount);
                    else
                        BuildTreesWithGivenNodeCountFromSubTrees(x => TreesWithBlackRoot[x]);
                    return trees;

                    void BuildTreesWithGivenNodeCountFromSubTrees(Func<int, IEnumerable<Node>> getSubtreesWithNodeCount)
                    {
                        for (int i = 0; i < (nodeCount + 1) / 2; i++)
                        {
                            var leftSubtreeNodeCount = i;
                            var rightSubtreeNodeCount = nodeCount - 1 - leftSubtreeNodeCount;
                            foreach (var leftTree in getSubtreesWithNodeCount(leftSubtreeNodeCount))
                            {
                                foreach (var rightTree in getSubtreesWithNodeCount(rightSubtreeNodeCount))
                                {
                                    if (GetBlackHeight(leftTree) == GetBlackHeight(rightTree))
                                        trees.TryAddTree(new Node(rootColor, leftTree, rightTree));
                                }
                            }
                        }
                    }
                }
                
                int GetBlackHeight(Node node)
                {
                    return node == null ? 1 : node.BlackHeight;
                }
            }
            
            private IEnumerable<Node> EnumerateTreesWithNodeCount(int nodeCount)
            {
                foreach (var tree in TreesWithBlackRoot[nodeCount])
                {
                    yield return tree;
                }
                foreach (var tree in TreesWithRedRoot[nodeCount])
                {
                    yield return tree;
                }
            }
            
            public Node GetTreeClosestToBlackHeight(int nodeCount, int blackHeight)
            {
                var targetTree = TreesWithBlackRoot[nodeCount].GetTreeClosestToBlackHeight(blackHeight);
                return targetTree.Untangle();
            }
        }
        
        private static List<int> _elements;
        private static TreeCache _cache;
        
        public static Node FindTreeClosestToBlackHeight(List<int> elements, int blackHeight)
        {
            _elements = elements;    
            _elements.Sort();
            _cache = new TreeCache(elements.Count);
            
            var tree = _cache.GetTreeClosestToBlackHeight(elements.Count, blackHeight);
            tree.FillWithValues(elements);
            return tree;
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
            var elementsAsString = Console.ReadLine().Split(' ');
            for (var i = 0; i < amountOfElements; i++)
                _elements.Add(Convert.ToInt32(elementsAsString[i]));
            Console.WriteLine("Introducir altura black del arbol");
            var blackHeight = Convert.ToInt32(Console.ReadLine());
            
            var root = RedBlackTreeBuilder.FindTreeClosestToBlackHeight(_elements, blackHeight + 1);
            if (root != null)
            {
                if(root.BlackHeight != blackHeight + 1)
                    Console.WriteLine($"No se encontro una solucion con black height = {blackHeight} pero se encontro para black height = {root.BlackHeight-1}");
                Console.WriteLine("La solucion es");
                root.Print();
            }
            else
                Console.WriteLine($"No tiene solucion para black height = {blackHeight}");
            Console.Read();
        }
    }
}