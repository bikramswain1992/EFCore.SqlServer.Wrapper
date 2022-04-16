
namespace EFCoreLib.Core.DataContract;
public class StatusResponse<TId>
{
    public TId RecordId { get; set; }
    public ActionResponseStatus Status { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}


public class StatusResponse : StatusResponse<string> { }