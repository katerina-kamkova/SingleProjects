using System.Text.RegularExpressions;
namespace Serializable;

/// <summary>                                                                                                                    
/// Random List that supports following serialization:                                                                           
/// only necessary information is transmitted, in readable form                                                                    
/// </summary>                                                                                                                   
public class ListRandomReadable : IListRandom                                                                                      
{                                                                                                                                
    private ListNode head;                                                                                                       
    private ListNode tail;                                                                                                       
    private int count;                                                                                                           
                                                                                                                                 
    public ListRandomReadable()                                                                                                    
    {                                                                                                                            
        this.head = null;                                                                                                        
        this.tail = null;                                                                                                        
        this.count = 0;                                                                                                          
    }                                                                                                                            
                                                                                                                                 
    public ListNode Head { get => head; }                                                                                        
    public ListNode Tail { get => tail; }                                
    public int Count { get => count; }

    private void WriteListNode(StreamWriter writer, int number, int random, string data)
    {
        writer.WriteLine("Node number = " + number);
        writer.WriteLine("Random node number = " + random);
        writer.WriteLine("Data = " + data);
        writer.WriteLine();
    }
    
    /// <summary>                                                                                                                
    /// Save necessary information in readable form                                                                                                           
    /// </summary>                                                                                                               
    /// <param name="s">Stream that transmits data</param>                                                                       
    public void Serialize(Stream s)                                                                                              
    {                                                                                                                            
        var writer = new StreamWriter(s) { AutoFlush = true };
        writer.WriteLine("Number of nodes in the list = " + this.Count);
        writer.WriteLine();
        
        // create dictionary to bind pointer with serial number                                                                  
        var nums = new Dictionary<ListNode, int>();                                                                              
        var temp = this.Head;                                                                                                    
        for (var i = 0; i < this.Count; i++)                                                                                     
        {                                                                                                                        
            nums[temp] = i;                                                                                                      
            temp = temp.Next;                                                                                                    
        }                                                                                                                        
                                                                                                                                 
        temp = Head;                                                                                                             
        for (var i = 0; i < this.Count; i++)
        {
            WriteListNode(writer, i, temp.Random == null ? -1 : nums[temp.Random], temp.Data);
            temp = temp.Next;
        }                                                                                                                        
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

    private string GetValueFromString(StreamReader reader)
    {
        var pattern = ".*=\\s";
        var rgx = new Regex(pattern);
        return rgx.Replace(reader.ReadLine(), "");
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
                                                                                                                                 
        var reader = new StreamReader(s);                                                                                        
        var nums = new Dictionary<int, ListNode>();

        var count = Convert.ToInt32(GetValueFromString(reader));
        reader.ReadLine();
        
        for (var i = 0; i < count; i++)                                                                                          
        {                                                                                                                        
            if (!nums.ContainsKey(i)) { nums[i] = new ListNode(); }

            reader.ReadLine();
            var random = Convert.ToInt32(GetValueFromString(reader));                                                                              
            if (!nums.ContainsKey(random))                                                                                       
            {                                                                                                                    
                nums[random] = random == -1 ? null : new ListNode();                                                             
            }                                                                                                                    
                                                      
            var data = GetValueFromString(reader);                                                                             
            AddAtTheEnd(nums[random], data, nums[i]);
            reader.ReadLine();
        }                                                                                                                        
    }                                                                                                                            
}                                                                                                                                
                                                                                                                                 