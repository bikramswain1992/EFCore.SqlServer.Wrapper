using EFCoreLib.BuilderConfig.Contract;
using EFCoreLib.Core;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace EFCoreLib.BuilderConfig;
public class EntityContext<TEntity> : DbContext where TEntity : EntityBase
{
    private readonly List<IEntityBuilder<TEntity>> _entityBuilders;

    public EntityContext(DbContextOptions<EntityContext<TEntity>> options) : base(options)
    {

    }

    public EntityContext(DbContextOptions<EntityContext<TEntity>> options, IEnumerable<IEntityBuilder<TEntity>> entityBuilders) : base(options)
    {
        if (!entityBuilders.Any())
        {
            throw new ArgumentException("No entity builder found");
        }
        _entityBuilders = entityBuilders.ToList();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var tableAttribute = (TableAttribute)typeof(TEntity)
            .GetCustomAttributes(false)
            .FirstOrDefault(x => x is TableAttribute);

        if(tableAttribute != null && !string.IsNullOrEmpty(tableAttribute.Schema))
        {
            modelBuilder.HasDefaultSchema(tableAttribute.Schema);
        }

        _entityBuilders.ForEach(builder =>
        {
            builder.BuildModel(modelBuilder);
        });

    }

    public int SaveChanges()
    {
        return base.SaveChanges();
    }

    public TEntity GetEntity(Guid id)
    {
        return ChangeTracker.Entries<TEntity>()
            .FirstOrDefault(x => x.Entity.Id == id)?.Entity;
    }
}
