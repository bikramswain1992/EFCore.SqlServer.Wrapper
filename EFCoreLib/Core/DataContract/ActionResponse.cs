
namespace EFCoreLib.Core.DataContract;
public class ActionResponse<TResult, TId> : StatusResponse<TId>
{
    public TResult Result { get; set; }
}

public class ActionResponse<TResult> : StatusResponse
{
    public TResult Result { get; set; }
}
