
namespace EFCoreLib.Core;
public class EntityBase
{
    public EntityBase()
    {

    }
    public Guid Id { get; set; }
    public virtual string? CreatedBy { get; set; }
    public virtual DateTime? CreatedOn { get; set; }
    public virtual string? ModifiedBy { get; set; }
    public virtual DateTime? ModifiedOn { get; set; }
}
