namespace Serializable;

/// <summary>
/// Doubly linked list with an extra pointer to a random list element in every node,
/// can be serialized and deserialized
/// </summary>
public interface IListRandom : ISerializable
{
    /// <summary>                                                                                 
    /// Add node at the end of the list                                                           
    /// </summary>                                                                                
    /// <param name="random">Random node</param>                                                  
    /// <param name="data">List data</param>                                                      
    /// <param name="newNode">List node to be added, if it already exists</param>                 
    public void AddAtTheEnd(ListNode random, string data, ListNode newNode = null);
    
    public ListNode Head { get; }  
    public ListNode Tail { get; }  
    public int Count { get; }      
}