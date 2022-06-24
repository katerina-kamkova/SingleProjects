namespace SerializableTests;

public class ListRandomBinaryTests
{
    private IListRandom EmptyListBinary;
    private IListRandom SmallListBinary;
    private IListRandom BigListBinary;
    
    private IListRandom EmptyListReadable;
    private IListRandom SmallListReadable;
    private IListRandom BigListReadable;
    
    string path = @"../newText.txt";

    private void FillSmallList(IListRandom list)
    {
        var node1 = new ListNode();
        var node2 = new ListNode();
        var node3 = new ListNode();
        list.AddAtTheEnd(node2, "a", node1);
        list.AddAtTheEnd(node3, "b", node2);
        list.AddAtTheEnd(node1, "c", node3);
    }
    
    private void FillBigList(IListRandom list)
    {
        var node1 = new ListNode();
        var node2 = new ListNode();
        var node3 = new ListNode();
        var node4 = new ListNode();
        var node5 = new ListNode();
        list.AddAtTheEnd(node2, "aaa", node1);
        list.AddAtTheEnd(null, "", node2);
        list.AddAtTheEnd(node3, "aaa", node3);
        list.AddAtTheEnd(node2, "bBBb", node4);
        list.AddAtTheEnd(node4, "c", node5);
    }
    
    [SetUp]
    public void Setup()
    {
        EmptyListBinary = new ListRandomBinary();
        EmptyListReadable = new ListRandomReadable();

        SmallListBinary = new ListRandomBinary();
        FillSmallList(SmallListBinary);
        SmallListReadable = new ListRandomReadable();
        FillSmallList(SmallListReadable);

        BigListBinary = new ListRandomBinary();
        FillBigList(BigListBinary);
        BigListReadable = new ListRandomReadable();
        FillBigList(BigListReadable);
    }

    private void SerializeDeserialize(ISerializable list)
    {
        using (FileStream stream = File.Create(path))
        {
            list.Serialize(stream);
        }
        
        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            list.Deserialize(stream);
        }
        File.Delete(path);
    }

    private void TestEmptyList(IListRandom list)
    {
        SerializeDeserialize(list);
        
        Assert.That(list.Count, Is.EqualTo(0));
        Assert.That(list.Head, Is.EqualTo(null));
        Assert.That(list.Tail, Is.EqualTo(null));
    }

    [Test]
    public void TestEmptyListBinary()
    {
        TestEmptyList(EmptyListBinary);
    }
    
    [Test]
    public void TestEmptyListReadable()
    {
        TestEmptyList(EmptyListReadable);
    }

    private void TestSmallList(IListRandom list)
    {
        SerializeDeserialize(list);
        
        Assert.That(list.Count, Is.EqualTo(3));
        Assert.That(list.Head.Data, Is.EqualTo("a"));
        Assert.That(list.Tail.Data, Is.EqualTo("c"));

        var temp = list.Head;
        Assert.That(temp.Previous, Is.EqualTo(null));
        Assert.That(temp.Next.Data, Is.EqualTo("b"));
        Assert.That(temp.Random.Data, Is.EqualTo("b"));
        Assert.That(temp.Data, Is.EqualTo("a"));

        temp = temp.Next;
        Assert.That(temp.Previous.Data, Is.EqualTo("a"));
        Assert.That(temp.Next.Data, Is.EqualTo("c"));
        Assert.That(temp.Random.Data, Is.EqualTo("c"));
        Assert.That(temp.Data, Is.EqualTo("b"));
        
        temp = temp.Next;
        Assert.That(temp.Previous.Data, Is.EqualTo("b"));
        Assert.That(temp.Next, Is.EqualTo(null));
        Assert.That(temp.Random.Data, Is.EqualTo("a"));
        Assert.That(temp.Data, Is.EqualTo("c"));
    }

    [Test]
    public void TestSmallListBinary()
    {
        TestSmallList(SmallListBinary);
    }
    
    [Test]
    public void TestSmallListReadable()
    {
        TestSmallList(SmallListReadable);
    }

    private void TestBigList(IListRandom list)
    {
        SerializeDeserialize(list);
        
        Assert.That(list.Count, Is.EqualTo(5));
        Assert.That(list.Head.Data, Is.EqualTo("aaa"));
        Assert.That(list.Tail.Data, Is.EqualTo("c"));

        var temp = list.Head;
        Assert.That(temp.Previous, Is.EqualTo(null));
        Assert.That(temp.Next.Data, Is.EqualTo(""));
        Assert.That(temp.Random.Data, Is.EqualTo(""));
        Assert.That(temp.Data, Is.EqualTo("aaa"));

        temp = temp.Next;
        Assert.That(temp.Previous.Data, Is.EqualTo("aaa"));
        Assert.That(temp.Next.Data, Is.EqualTo("aaa"));
        Assert.That(temp.Random, Is.EqualTo(null));
        Assert.That(temp.Data, Is.EqualTo(""));
        
        temp = temp.Next;
        Assert.That(temp.Previous.Data, Is.EqualTo(""));
        Assert.That(temp.Next.Data, Is.EqualTo("bBBb"));
        Assert.That(temp.Random.Data, Is.EqualTo("aaa"));
        Assert.That(temp.Data, Is.EqualTo("aaa"));
        
        temp = temp.Next;
        Assert.That(temp.Previous.Data, Is.EqualTo("aaa"));
        Assert.That(temp.Next.Data, Is.EqualTo("c"));
        Assert.That(temp.Random.Data, Is.EqualTo(""));
        Assert.That(temp.Data, Is.EqualTo("bBBb"));
        
        temp = temp.Next;
        Assert.That(temp.Previous.Data, Is.EqualTo("bBBb"));
        Assert.That(temp.Next, Is.EqualTo(null));
        Assert.That(temp.Random.Data, Is.EqualTo("bBBb"));
        Assert.That(temp.Data, Is.EqualTo("c"));
    }

    [Test]
    public void TestBigListBinary()
    {
        TestBigList(BigListBinary);
    }
    
    [Test]
    public void TestBigListReadable()
    {
        TestBigList(BigListReadable);
    }
}