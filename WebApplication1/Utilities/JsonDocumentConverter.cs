using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Text.Json;

namespace WebApplication1.Utilities;

public class JsonDocumentConverter : ValueConverter<JsonDocument, string>
{
    public JsonDocumentConverter() : base(
        v => v.RootElement.GetRawText(), //Convert to provider (DB)
        v => JsonDocument.Parse(v, default) //Convert from provider (DB)
        )
    { 
    
    }
}
