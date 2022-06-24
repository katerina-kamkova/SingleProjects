namespace Serializable;

/// <summary>
/// Objects that can be serialized and deserialized
/// </summary>
public interface ISerializable
{
    /// <summary>
    /// Convert an object into a stream of bytes to store or transmit it
    /// </summary>
    /// <param name="s">Stream that transmits data</param>
    public void Serialize(Stream s);
    
    /// <summary>
    /// Recreate the object from a stream of bytes
    /// </summary>
    /// <param name="s">Stream that transmits data</param>
    public void Deserialize(Stream s);
}