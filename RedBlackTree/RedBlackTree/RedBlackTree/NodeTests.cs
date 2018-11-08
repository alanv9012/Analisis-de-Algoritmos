using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using RedBlackTree;

[TestFixture]
class NodeTests
{
    [Test]
    public void Untangle_Should_ReturnTreeWithNoRepeatedNodes()
    {
        var repeated = new Node(Color.Red);
        var root = new Node(Color.Black, repeated, repeated);
        var untangled = root.Untangle();
        
        Assert.AreNotEqual(untangled.Left, untangled.Right);
        Assert.AreEqual(Color.Red, untangled.Left.Color);
        Assert.AreEqual(Color.Red, untangled.Right.Color);
        Assert.AreEqual(Color.Black, untangled.Color);
    }

    [Test]
    public void FillWithValues_Test1_FillsWithValues()
    {
        var left = new Node(Color.Black, new Node(Color.Black), new Node(Color.Black));
        var right = new Node(Color.Black, new Node(Color.Black), new Node(Color.Black));
        var node = new Node(Color.Black, left, right);
        
        node.FillWithValues(new List<int>() { 1, 5, 10, 15, 20, 25, 30 });
        
        Assert.AreEqual(15, node.Value);
        Assert.AreEqual(5, node.Left.Value);
        Assert.AreEqual(1, node.Left.Left.Value);
        Assert.AreEqual(10, node.Left.Right.Value);
        Assert.AreEqual(25, node.Right.Value);
        Assert.AreEqual(20, node.Right.Left.Value);
        Assert.AreEqual(30, node.Right.Right.Value);
    }
    
    [Test]
    public void FillWithValues_Test2_FillsWithValues()
    {
        var left = new Node(Color.Black);
        var right = new Node(Color.Red, new Node(Color.Black), new Node(Color.Black));
        var node = new Node(Color.Black, left, right);
        
        node.FillWithValues(new List<int>() { 1, 5, 10, 15, 20 });
        
        Assert.AreEqual(5, node.Value);
        Assert.AreEqual(1, node.Left.Value);
        Assert.AreEqual(15, node.Right.Value);
        Assert.AreEqual(10, node.Right.Left.Value);
        Assert.AreEqual(20, node.Right.Right.Value);
    }

    [Test]
    public void NodeCount_Test1()
    {
        var left = new Node(Color.Red);
        var right = new Node(Color.Red);
        var node = new Node(Color.Black, left, right);
        
        Assert.AreEqual(3, node.NodeCount);
    }

    [Test]
    public void NodeCount_Test2()
    {
        var node = new Node(Color.Black);
        
        Assert.AreEqual(1, node.NodeCount);
    }
    
    [Test]
    public void GetBlackHeight_Test1()
    {
        var right = new Node(Color.Red);
        var node = new Node(Color.Black, null, right);
        
        Assert.AreEqual(2, node.BlackHeight);
    }
    
    [Test]
    public void GetBlackHeight_Test2()
    {
        var left = new Node(Color.Red);
        var right = new Node(Color.Red);
        var node = new Node(Color.Black, left, right);
        
        Assert.AreEqual(2, node.BlackHeight);
    }
    
    [Test]
    public void GetBlackHeight_Test3()
    {
        var node = new Node(Color.Black);
        
        Assert.AreEqual(2, node.BlackHeight);
    }
    
    [Test]
    public void GetBlackHeight_Test4()
    {
        var left = new Node(Color.Red, new Node(Color.Black), new Node(Color.Black));
        var right = new Node(Color.Red, new Node(Color.Black), new Node(Color.Black));
        var node = new Node(Color.Black, left, right);
        
        Assert.AreEqual(3, node.BlackHeight);
    }
    
    [Test]
    public void Validate_Test1_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var left = new Node(Color.Black);
            var right = new Node(Color.Red);
            var node = new Node(Color.Black, left, right);
        });
    }
    
    [Test]
    public void Validate_Test2_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var left = new Node(Color.Black, new Node(Color.Black));
            var right = new Node(Color.Black);
            var node = new Node(Color.Black, left, right);
        });
    }
    
    [Test]
    public void Validate_Test3_ThrowsException()
    {
        
        Assert.Throws<InvalidOperationException>(() =>
        {
            var left = new Node(Color.Black, new Node(Color.Black), new Node(Color.Black));
            var right = new Node(Color.Black, new Node(Color.Black), new Node(Color.Red));
            var node = new Node(Color.Black, left, right);
        });
    }
    
    [Test]
    public void Validate_Test4_ThrowsException()
    {
        Assert.Throws<InvalidOperationException>(() =>
        {
            var left = new Node(Color.Black);
            var right = new Node(Color.Red, new Node(Color.Black));
            var node = new Node(Color.Black, left, right);
        });
    }
        
    [Test]
    public void Validate_Test1_DoesNotThrowException()
    {
        Assert.DoesNotThrow(() =>
        {
            var left = new Node(Color.Black);
            var right = new Node(Color.Black);
            var node = new Node(Color.Black, left, right);
        });
    }
    
    [Test]
    public void Validate_Test2_DoesNotThrowException()
    {
        Assert.DoesNotThrow(() =>
        {
            var left = new Node(Color.Black, new Node(Color.Black), new Node(Color.Black));
            var right = new Node(Color.Black, new Node(Color.Black), new Node(Color.Black));
            var node = new Node(Color.Black, left, right);
        });
    }
    
    [Test]
    public void Validate_Test3_DoesNotThrowsException()
    {
        Assert.DoesNotThrow(() =>
        {
            var left = new Node(Color.Black, new Node(Color.Black), new Node(Color.Black));
            var right = new Node(Color.Black, new Node(Color.Black), new Node(Color.Red, new Node(Color.Black), new Node(Color.Black)));
            var node = new Node(Color.Black, left, right);
        });
    }
}

[TestFixture]
public class TreeCacheTests
{
    [Test]
    public void Constructor_AmountNodesIsZero_BuildsTrees()
    {
        var treeCache = new RedBlackTreeBuilder.TreeCache(0);
        
        Assert.AreEqual(1, treeCache.TreesWithBlackRoot[0].Count());
        Assert.AreEqual(null, treeCache.TreesWithBlackRoot[0].First());
        Assert.AreEqual(0, treeCache.TreesWithRedRoot[0].Count());
    }
    
    [Test]
    public void Constructor_AmountNodesIsOne_BuildsTrees()
    {
        var treeCache = new RedBlackTreeBuilder.TreeCache(1);
        
        var firstTree = treeCache.TreesWithBlackRoot[1].First();
        
        Assert.AreEqual(Color.Black, firstTree.Color);
        Assert.IsNull(firstTree.Left);
        Assert.IsNull(firstTree.Right);
        
        var secondTree = treeCache.TreesWithRedRoot[1].First();
        
        Assert.AreEqual(Color.Red, secondTree.Color);
        Assert.IsNull(secondTree.Left);
        Assert.IsNull(secondTree.Right);
    }
    
    [Test]
    public void Constructor_AmountNodesIsTwo_BuildsTrees()
    {
        var treeCache = new RedBlackTreeBuilder.TreeCache(2);
        
        var firstTree = treeCache.TreesWithBlackRoot[2].First();
        
        Assert.AreEqual(Color.Black, firstTree.Color);
        Assert.AreEqual(Color.Red, firstTree.Right.Color);
        Assert.IsNull(firstTree.Left);
    }

    [Test]
    public void Constructor_AmountNodesIsThree_BuildsTrees()
    {
        var treeCache = new RedBlackTreeBuilder.TreeCache(3);
        
        var firstTree = treeCache.TreesWithBlackRoot[3].First();
        
        Assert.AreEqual(Color.Black, firstTree.Color);
        Assert.AreEqual(Color.Red, firstTree.Left.Color);
        Assert.AreEqual(Color.Red, firstTree.Right.Color);
        
        var secondThree = treeCache.TreesWithBlackRoot[3].ElementAt(1);
        
        Assert.AreEqual(Color.Black, secondThree.Color);
        Assert.AreEqual(Color.Black, secondThree.Left.Color);
        Assert.AreEqual(Color.Black, secondThree.Right.Color);
    }
    
    [Test]
    public void Constructor_AmountNodesIsFour_BuildsTrees()
    {
        var treeCache = new RedBlackTreeBuilder.TreeCache(4);
        
        var firstTree = treeCache.TreesWithBlackRoot[4].First();
        
        Assert.AreEqual(Color.Black, firstTree.Color);
        Assert.AreEqual(Color.Black, firstTree.Left.Color);
        Assert.AreEqual(Color.Black, firstTree.Right.Color);
        Assert.AreEqual(Color.Red, firstTree.Right.Right.Color);
        
        var secondThree = treeCache.TreesWithRedRoot[4].First();
        
        Assert.AreEqual(Color.Red, secondThree.Color);
        Assert.AreEqual(Color.Black, secondThree.Left.Color);
        Assert.AreEqual(Color.Black, secondThree.Right.Color);
        Assert.AreEqual(Color.Red, secondThree.Right.Right.Color);
    }
    
    [Test]
    public void Constructor_AmountNodesIsSeven_BuildsTrees()
    {
        var treeCache = new RedBlackTreeBuilder.TreeCache(7);
     
        Assert.AreEqual(1, treeCache.TreesWithBlackRoot[0].Count());
        Assert.AreEqual(0, treeCache.TreesWithRedRoot[0].Count());
        Assert.AreEqual(1, treeCache.TreesWithRedRoot[1].Count());
        Assert.AreEqual(1, treeCache.TreesWithBlackRoot[1].Count());
        Assert.AreEqual(0, treeCache.TreesWithRedRoot[2].Count());
        Assert.AreEqual(1, treeCache.TreesWithBlackRoot[2].Count());
        Assert.AreEqual(1, treeCache.TreesWithRedRoot[3].Count());
        Assert.AreEqual(2, treeCache.TreesWithBlackRoot[3].Count());
        Assert.AreEqual(1, treeCache.TreesWithRedRoot[4].Count());
        Assert.AreEqual(1, treeCache.TreesWithBlackRoot[4].Count());
        Assert.AreEqual(1, treeCache.TreesWithRedRoot[5].Count());
        Assert.AreEqual(1, treeCache.TreesWithBlackRoot[5].Count());
        Assert.AreEqual(1, treeCache.TreesWithRedRoot[6].Count());
        Assert.AreEqual(1, treeCache.TreesWithBlackRoot[6].Count());
        Assert.AreEqual(2, treeCache.TreesWithRedRoot[7].Count());
        Assert.AreEqual(2, treeCache.TreesWithBlackRoot[7].Count());
    }
}

[TestFixture]
public class RedBlackTreeBuilderTests
{
}