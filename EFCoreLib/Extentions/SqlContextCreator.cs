using EFCoreLib.BuilderConfig;
using EFCoreLib.Core;
using EFCoreLib.Core.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace EFCoreLib.Extentions;
public static class SqlContextCreator
{
    public static void CreateSqlContext<T>(this IServiceCollection services, IConfiguration configuration, string sectionName = "DefaultConnection")  where T : EntityBase, new()
    {
        string connectionString = configuration.GetConnectionString(sectionName);

        if(connectionString == null)
        {
            throw new ArgumentNullException($"Could not find {sectionName} connection string");
        }

        services.AddDbContext<EntityContext<T>>(delegate (DbContextOptionsBuilder options)
        {
            options.UseSqlServer(connectionString);
        });

        services.AddTransient<IEntityRepository<T>, EntityRepository<T>>();
    } 
}
