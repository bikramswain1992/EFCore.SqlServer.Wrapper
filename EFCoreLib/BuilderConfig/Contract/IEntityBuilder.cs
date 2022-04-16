using Microsoft.EntityFrameworkCore;

namespace EFCoreLib.BuilderConfig.Contract;
public interface IEntityBuilder<TEntity>
{
    void BuildModel(ModelBuilder builder);
}
