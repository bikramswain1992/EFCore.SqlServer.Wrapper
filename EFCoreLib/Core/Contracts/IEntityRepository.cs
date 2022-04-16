
using EFCoreLib.Core.DataContract;

namespace EFCoreLib.Core.Contracts;

public interface IEntityRepository<TEntity, TId> : IReadEntityRepository<TEntity, TId>
{
    ActionResponse<IEnumerable<TEntity>, TId> GetAll();
    ActionResponse<TEntity, TId> Get(TId id);

    StatusResponse<TId> Insert(TEntity entity);
    StatusResponse<TId> Update(TEntity entity);
    StatusResponse<TId> Delete(TId id);

}

public interface IEntityRepository<TEntity> : IEntityRepository<TEntity, Guid>
{
}