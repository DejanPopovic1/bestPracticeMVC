using ConfigService.Database;

namespace WebApplication1.Data;

public class ActorRepository(ApplicationContext context) : EntityRepository<Actor, Guid>(context)
{
}
