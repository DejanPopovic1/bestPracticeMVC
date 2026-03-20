using System.Reflection.Metadata;

namespace WebApplication1.Data;

public class Document
{
    public Guid Id { get; set; }
    public string DocumentName {get; set;}

    public byte[] BinaryData { get; set;}

}
