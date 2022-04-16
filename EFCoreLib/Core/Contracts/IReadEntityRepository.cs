
namespace EFCoreLib.Core.Contracts;

public interface IReadEntityRepository<out TEntity, TId>
{
    IQueryable<TEntity> AsQuerable();

}

public interface IReadEntityRepository<out TEntity> : IReadEntityRepository<TEntity, Guid>
{
}
