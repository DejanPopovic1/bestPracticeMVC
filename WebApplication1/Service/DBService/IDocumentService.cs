using WebApplication1.Inputs;
using WebApplication1.Data;

namespace WebApplication1.Service.DBService;

public interface IDocumentService
{
    public Task<Document> Create(DocumentCreateInput input);

    public Task<Document> Update(Guid id, DocumentUpdateInput input);

}
