namespace Serializable;

/// <summary>
/// Random List that supports following serialization:
/// only necessary information is transmitted, in binary form
/// </summary>
public class ListRandomBinary : IListRandom
{
    private ListNode head;
    private ListNode tail;
    private int count;
    
    public ListRandomBinary()
    {
        this.head = null;
        this.tail = null;
        this.count = 0;
    }

    public ListNode Head { get => head; }
    public ListNode Tail { get => tail; }
    public int Count { get => count; }

    private void WriteIntToStream(Int32 number, BinaryWriter writer)
    {
        var buffer = BitConverter.GetBytes(number);
        writer.Write(buffer, 0, buffer.Length);
    }

    private void WriteStringToStream(string data, BinaryWriter writer)
    {
        var buffer = System.Text.Encoding.ASCII.GetBytes(data);
        WriteIntToStream(buffer.Length, writer);
        writer.Write(buffer, 0, buffer.Length);
    }

    /// <summary>
    /// Save necessary information (ListRandomBinary.Count, ListNode.Random, ListNode.Data)
    /// in binary form
    /// </summary>
    /// <param name="s">Stream that transmits data</param>
    public void Serialize(Stream s)
    {
        var writer = new BinaryWriter(s);
        WriteIntToStream(this.Count, writer);
        
        // create dictionary to bind pointer with the serial number
        var nums = new Dictionary<ListNode, int>();
        var temp = this.Head;
        for (var i = 0; i < this.Count; i++)
        {
            nums[temp] = i;
            temp = temp.Next;
        }
        
        temp = Head;
        while (temp != null)
        {
            // don't know whether ListNode.Random can be null, but check just in case
            WriteIntToStream(temp.Random == null ? -1 : nums[temp.Random], writer);
            WriteStringToStream(temp.Data, writer);
            temp = temp.Next;
        }
    }

    private int ReadIntFromStream(BinaryReader reader)
    {
        var buffer = new byte[sizeof(Int32)];
        reader.Read(buffer, 0, buffer.Length);
        return BitConverter.ToInt32(buffer, 0);
    }

    private string ReadStringFromStream(BinaryReader reader)
    {
        var buffer = new byte[ReadIntFromStream(reader)];
        reader.Read(buffer, 0, buffer.Length);
        return System.Text.Encoding.ASCII.GetString(buffer);
    }
    
    /// <summary>
    /// Add node at the end of the list
    /// </summary>
    /// <param name="random">Random node</param>
    /// <param name="data">List data</param>
    /// <param name="newNode">List node to be added, if it already exists</param>
    public void AddAtTheEnd(ListNode random, string data, ListNode newNode = null)
    {
        if (newNode == null) { newNode = new ListNode(); }

        newNode.Random = random;
        newNode.Data = data;
        
        if (this.Count == 0)
        {
            this.head = newNode;
        }
        else
        {
            this.Tail.Next = newNode;
        }

        newNode.Previous = this.Tail;
        newNode.Next = null;
        this.tail = newNode;
        this.count++;
    }

    /// <summary>
    /// Recreate the list from a stream of bytes
    /// </summary>
    /// <param name="s">Stream that transmits data</param>
    public void Deserialize(Stream s)
    {
        this.head = null;
        this.tail = null;
        this.count = 0;

        var reader = new BinaryReader(s);
        var nums = new Dictionary<int, ListNode>();
        var count = ReadIntFromStream(reader);
        
        for (var i = 0; i < count; i++)
        {
            if (!nums.ContainsKey(i)) { nums[i] = new ListNode(); }
            
            var random = ReadIntFromStream(reader);
            if (!nums.ContainsKey(random))
            { 
                nums[random] = random == -1 ? null : new ListNode();
            }

            var data = ReadStringFromStream(reader);
            AddAtTheEnd(nums[random], data, nums[i]);
        }
    }
}
