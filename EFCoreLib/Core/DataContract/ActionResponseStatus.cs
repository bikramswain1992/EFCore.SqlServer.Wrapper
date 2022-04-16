namespace EFCoreLib.Core.DataContract;
public enum ActionResponseStatus
{
    Success,
    RecordNotFound,
    RecordAlreadyExists,
    ConcurrencyConflict,
    Unknown,
    Timeout
}
