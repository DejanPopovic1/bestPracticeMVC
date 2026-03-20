namespace WebApplication1.Inputs;

public class DocumentCreateInput
{
    public Guid Id { get; set; }
    public string DocumentName { get; set; }
    public byte[] BinaryData { get; set; }
}
