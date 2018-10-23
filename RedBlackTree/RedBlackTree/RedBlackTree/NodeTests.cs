using System.Collections.Generic;
using NUnit.Framework;
using RedBlackTree;

[TestFixture]
class NodeTests
{
    [Test]
    public void GetBlackHeight_Test1()
    {
        var left = new Node(0, Color.Black);
        var right = new Node(0, Color.Red);
        var node = new Node(0, Color.Black, left, right);
        
        Assert.AreEqual(2, node.GetBlackHeight());
    }
    
    [Test]
    public void GetBlackHeight_Test2()
    {
        var left = new Node(0, Color.Black, new Node(0, Color.Black));
        var right = new Node(0, Color.Red);
        var node = new Node(0, Color.Black, left, right);
        
        Assert.AreEqual(3, node.GetBlackHeight());
    }
    
    [Test]
    public void GetBlackHeight_Test3()
    {
        var node = new Node(0, Color.Black);
        
        Assert.AreEqual(1, node.GetBlackHeight());
    }
    
    [Test]
    public void GetBlackHeight_Test4()
    {
        var left = new Node(0, Color.Red, new Node(0, Color.Black));
        var right = new Node(0, Color.Red);
        var node = new Node(0, Color.Black, left, right);
        
        Assert.AreEqual(2, node.GetBlackHeight());
    }
    
    [Test]
    public void Validate_Test1_ThrowsException()
    {
        var left = new Node(0, Color.Black, new Node(0, Color.Black));
        var right = new Node(0, Color.Red);
        var node = new Node(0, Color.Black, left, right);
        
        Assert.IsFalse(node.IsValid());
    }
    
    [Test]
    public void Validate_Test2_ThrowsException()
    {
        var left = new Node(0, Color.Black, new Node(0, Color.Black));
        var right = new Node(0, Color.Black);
        var node = new Node(0, Color.Black, left, right);
        
        Assert.IsFalse(node.IsValid());
    }
    
    [Test]
    public void Validate_Test3_ThrowsException()
    {
        var left = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Black));
        var right = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Red));
        var node = new Node(0, Color.Black, left, right);
        
        Assert.IsFalse(node.IsValid());
    }
    
    [Test]
    public void Validate_Test4_ThrowsException()
    {
        var left = new Node(0, Color.Black);
        var right = new Node(0, Color.Red, new Node(0, Color.Black), new Node(0, Color.Red));
        var node = new Node(0, Color.Black, left, right);

        Assert.IsFalse(node.IsValid());
    }
        
    [Test]
    public void Validate_Test1_DoesNotThrowException()
    {
        var left = new Node(0, Color.Black);
        var right = new Node(0, Color.Black);
        var node = new Node(0, Color.Black, left, right);
        
        Assert.IsTrue(node.IsValid());
    }
    
    [Test]
    public void Validate_Test2_DoesNotThrowException()
    {
        var left = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Black));
        var right = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Black));
        var node = new Node(0, Color.Black, left, right);
     
        Assert.IsTrue(node.IsValid());
    }
    
    [Test]
    public void Validate_Test3_DoesNotThrowsException()
    {
        var left = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Black));
        var right = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Red, new Node(0, Color.Black), new Node(0, Color.Black)));
        var node = new Node(0, Color.Black, left, right);

        Assert.IsTrue(node.IsValid());
    }
}

[TestFixture]
public class RedBlackTreeBuilderTests
{
    [Test]
    public void CanBeColored()
    {
        var left = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Black));
        var right = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Red, new Node(0, Color.Black), new Node(0, Color.Black)));
        var node = new Node(0, Color.Black, left, right);

        Assert.IsTrue(RedBlackTreeBuilder.CanBeColored(node));
    }
    
    
    [Test]
    public void CanBeColored_Test2()
    {
        var left = new Node(0, Color.Black, new Node(0, Color.Black), new Node(0, Color.Black));
        var right = new Node(0, Color.Black, new Node(0, Color.Black));
        var node = new Node(0, Color.Black, left, right);

        Assert.IsTrue(RedBlackTreeBuilder.CanBeColored(node));
    }
    
    [Test]
    public void CanBeColored_Test3()
    {
        var left = new Node(0, Color.Black);
        var right = new Node(0, Color.Black);
        var node = new Node(0, Color.Black, left, right);

        Assert.IsTrue(RedBlackTreeBuilder.CanBeColored(node));
    }
}