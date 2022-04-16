using EFCoreLib.BuilderConfig;
using EFCoreLib.Core.Contracts;
using EFCoreLib.Core.DataContract;
using EFCoreLib.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreLib.Core;
public class EntityRepository<TEntity> : ReadEntityRepository<TEntity, TEntity, Guid>, IEntityRepository<TEntity> where TEntity : EntityBase, new()
{
    private readonly EntityContext<TEntity> _context;
    private readonly DbSet<TEntity> _entity;

    public EntityRepository(EntityContext<TEntity> context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
        _context = context;
        _entity = context.Set<TEntity>();
    }

    public ActionResponse<TEntity, Guid> Get(Guid id)
    {
        return ExecuteActionWithErrorHandeling(() => {
            var result = _entity.AsQueryable().Where(x => x.Id == id).FirstOrDefault();

            var status = result != null ? ActionResponseStatus.Success : ActionResponseStatus.RecordNotFound;

            var x = SqlProcessResponse.FormSqlResponse<Guid>(id, status);

            return SqlProcessResponse.FormSqlResponse<Guid>(id, status).GetResult<TEntity, Guid>(result);

        }, $"Failed to get {nameof(TEntity)} for Id {id}.");
    }

    public ActionResponse<IEnumerable<TEntity>, Guid> GetAll()
    {
        return ExecuteActionWithErrorHandeling(() =>
        {
            var result = _entity.ToList();

            return SqlProcessResponse.FormSqlResponse<Guid>().GetResult<IEnumerable<TEntity>, Guid>(result);
        }, $"Failed to get {nameof(TEntity)} data.");
    }
    

    public StatusResponse<Guid> Delete(Guid id)
    {
        if (id == default)
        {
            return new StatusResponse<Guid>()
            {
                Status = ActionResponseStatus.RecordNotFound,
                ErrorMessage = $"Failed to delete {nameof(TEntity)}: {nameof(id)} was null."
            };
        }

        return ExecuteActionWithErrorHandeling(() =>
        {
            var record = _entity.FirstOrDefault(x => x.Id == id) ?? new TEntity() { Id = id }; //_context.GetEntity(id) ?? new TEntity() { Id = id };

            _entity.Remove(record);
            _context.SaveChanges();

            return SqlProcessResponse.FormSqlResponse(record.Id);

        }, $"Failed to delete {nameof(TEntity)} for Id {id}.");
    }

    

    public StatusResponse<Guid> Insert(TEntity entity)
    {
        if(entity == null)
        {
            return new StatusResponse<Guid>()
            {
                Status = ActionResponseStatus.RecordNotFound,
                ErrorMessage = $"Failed to insert {nameof(TEntity)}: {nameof(entity)} was null."
            };
        }

        return ExecuteActionWithErrorHandeling(() =>
        {
            _entity.Add(entity);
            _context.SaveChanges();

            return SqlProcessResponse.FormSqlResponse(entity.Id);

        }, $"Failed to insert {nameof(TEntity)} for Id {nameof(entity)}.");
    }

    public StatusResponse<Guid> Update(TEntity entity)
    {
        if (entity == null)
        {
            return new StatusResponse<Guid>()
            {
                Status = ActionResponseStatus.RecordNotFound,
                ErrorMessage = $"Failed to update {nameof(TEntity)}: {nameof(entity)} was null."
            };
        }

        return ExecuteActionWithErrorHandeling(() =>
        {
            _entity.Update(entity);
            _context.SaveChanges();

            return SqlProcessResponse.FormSqlResponse(entity.Id);

        }, $"Failed to update {nameof(TEntity)} for Id {nameof(entity)}.");
    }
}
