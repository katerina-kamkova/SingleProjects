using Serializable;

public class Program
{
    private static string IsNull(ListNode node)
    {
        return node == null ? "null" : node.Data;
    }
    
    /// <summary>
    /// Print list node fields
    /// </summary>
    /// <param name="node">Node to be printed</param>
    private static void PrintNode(ListNode node)
    {
        Console.WriteLine("Previous: " + IsNull(node.Previous));
        Console.WriteLine("Next: " + IsNull(node.Next));
        Console.WriteLine("Random: " + IsNull(node.Random));
        Console.WriteLine("Data: " + node.Data);
        Console.WriteLine();
    }

    /// <summary>
    /// Print list
    /// </summary>
    /// <param name="list">List to be printed</param>
    private static void PrintList(IListRandom list)
    {
        Console.WriteLine("List fields:");
        Console.WriteLine("Head: " + IsNull(list.Head));
        Console.WriteLine("Tail: " + IsNull(list.Tail));
        Console.WriteLine("Count: " + list.Count);
        Console.WriteLine();

        var temp = list.Head;
        while (temp != null)
        {
            PrintNode(temp);
            temp = temp.Next;
        }
        
        Console.WriteLine();
    }

    /// <summary>
    /// Serialize and deserialize list,
    /// print it before and after transformations
    /// </summary>
    /// /// <param name="list">List to be tested</param>
    private static void TestAndPrint(IListRandom list)
    {
        var path = @"../newText.txt";

        Console.WriteLine("List before serialization");
        PrintList(list);
        using (FileStream stream = File.Create(path))
        {
            list.Serialize(stream);
        }
        
        using (FileStream stream = File.Open(path, FileMode.Open))
        {
            list.Deserialize(stream);
        }
        Console.WriteLine("List after deserialization");
        PrintList(list);
        
        File.Delete(path);
    }

    private static void FillLists(IListRandom smallList, IListRandom bigList)
    {
        var node1 = new ListNode();
        var node2 = new ListNode();
        var node3 = new ListNode();

        smallList.AddAtTheEnd(node2, "a", node1);
        smallList.AddAtTheEnd(node3, "b", node2);
        smallList.AddAtTheEnd(node1, "c", node3);
        
        var temp1 = new ListNode();
        var temp2 = new ListNode();
        var temp3 = new ListNode();
        var temp4 = new ListNode();
        var temp5 = new ListNode();

        bigList.AddAtTheEnd(temp2, "aaa", temp1);
        bigList.AddAtTheEnd(null, "", temp2);
        bigList.AddAtTheEnd(temp3, "aaa", temp3);
        bigList.AddAtTheEnd(temp2, "bBBb", temp4);
        bigList.AddAtTheEnd(temp4, "c", temp5);
    }
    
    public static void Main(string[] args)
    {
        IListRandom emptyListBinary = new ListRandomBinary();
        IListRandom smallListBinary = new ListRandomBinary();
        IListRandom bigListBinary = new ListRandomBinary();
        FillLists(smallListBinary, bigListBinary);
        
        Console.WriteLine("Empty list binary serialization");
        TestAndPrint(emptyListBinary);
        
        Console.WriteLine("Small list binary serialization");
        TestAndPrint(smallListBinary);
        
        Console.WriteLine("Big list binary serialization");
        TestAndPrint(bigListBinary);
        
        IListRandom emptyListReadable = new ListRandomReadable();
        IListRandom smallListReadable = new ListRandomReadable();
        IListRandom bigListReadable = new ListRandomReadable();
        FillLists(smallListReadable, bigListReadable);
        
        Console.WriteLine("Empty list readable serialization");
        TestAndPrint(emptyListReadable);
        
        Console.WriteLine("Small list readable serialization");
        TestAndPrint(smallListReadable);
        
        Console.WriteLine("Big list readable serialization");
        TestAndPrint(bigListReadable);
    }
}
