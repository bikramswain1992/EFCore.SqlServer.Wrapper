using EFCoreLib.BuilderConfig;
using EFCoreLib.Core.Contracts;
using EFCoreLib.Core.DataContract;
using EFCoreLib.Extentions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EFCoreLib.Core;
public class ReadEntityRepository<TRoot, TEntity, TId> : IReadEntityRepository<TEntity, TId> where TRoot : EntityBase, new() where TEntity : class, new()
{
    private readonly DbSet<TEntity> _entity;
    private readonly ILogger _logger;

    public ReadEntityRepository(EntityContext<TRoot> context, ILoggerFactory loggerFactory)
    {
        _entity = context.Set<TEntity>();
        _logger = loggerFactory.CreateLogger<ReadEntityRepository<TRoot, TEntity, TId>>();
    }
    public IQueryable<TEntity> AsQuerable()
    {
        return _entity
                .AsQueryable()
                .IgnoreAutoIncludes()
                .AsNoTracking();
    }

    protected T ExecuteActionWithErrorHandeling<T>(Func<T> action, string errorMessage, TId recordId = default) where T : StatusResponse<TId>
    {
        try
        {
            return action();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, errorMessage);
            return (T)SqlProcessResponse.FormSqlException(ex, recordId);
        }
    }
}

public class ReadEntityRepository<TRoot, TEntity> : ReadEntityRepository<TRoot, TEntity, Guid> where TRoot : EntityBase, new() where TEntity : class, new()
{
    public ReadEntityRepository(EntityContext<TRoot> context, ILoggerFactory loggerFactory) : base(context, loggerFactory)
    {
        
    }
}