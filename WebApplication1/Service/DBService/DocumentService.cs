using ConfigService.Database;
using System.Security.Cryptography;
using WebApplication1.Data;
using WebApplication1.Inputs;

namespace WebApplication1.Service.DBService;

public class DocumentService : IService<DocumentCreateInput, DocumentUpdateInput, Document, Guid>
//public class DocumentService : IDocumentService
{
    IEntityRepository<Document, Guid> _documentRepository;

    public DocumentService(IEntityRepository<Document, Guid> documentRepository)
    {
        _documentRepository = documentRepository;
    }

    public async Task<Document> CreateAsync(DocumentCreateInput input)
    {
        Document newDocument = new Document();
        newDocument.Id = Guid.NewGuid();
        newDocument.BinaryData = input.BinaryData;
        newDocument.DocumentName = input.DocumentName;
        await _documentRepository.AddAsync(newDocument);

        await _documentRepository.SaveChangesAsync();
        return newDocument;
    }

    public async Task<Document> UpdateAsync(Guid id, DocumentUpdateInput input)
    {
        Document item = await _documentRepository.FindByConditionOrFailAsync(x => x.Where(y => y.Id == id), false);
        item.BinaryData = input.BinaryData;
        item.DocumentName = input.DocumentName;
        return item;
    }
}
