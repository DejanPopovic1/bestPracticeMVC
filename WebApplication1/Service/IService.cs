using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace WebApplication1.Service;

public interface IService <TCreateInput, TUpdateInput, TEntity, Tid>
{
    public Task<TEntity> CreateAsync(TCreateInput input);

    public Task<TEntity> UpdateAsync(Tid id, TUpdateInput input);

}
