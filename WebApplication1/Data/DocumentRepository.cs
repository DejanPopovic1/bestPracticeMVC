using ConfigService.Database;

namespace WebApplication1.Data
{
    public class DocumentRepository : EntityRepository<Document, Guid>
    {
        public readonly ApplicationContext _applicationContext; 

        public DocumentRepository(ApplicationContext applicationContext) : base(applicationContext)
        {
            _applicationContext = applicationContext;
        }
    }
}
