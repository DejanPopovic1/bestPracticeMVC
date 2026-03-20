namespace WebApplication1.Inputs;

public class DocumentUpdateInput
{
    public Guid Id { get; set; }
    public string DocumentName { get; set; }
    public byte[] BinaryData { get; set; }
}
