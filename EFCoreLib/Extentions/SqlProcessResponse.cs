using EFCoreLib.Core.DataContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace EFCoreLib.Extentions;
public static class SqlProcessResponse
{
    public static StatusResponse<TId> FormSqlResponse<TId>(TId recordId = default, ActionResponseStatus status = ActionResponseStatus.Success)
    {
        return new StatusResponse<TId>()
        {
            RecordId = recordId,
            Status = status,
        };
    }

    //public static StatusResponse<Guid> FormSqlResponse(Guid recordId = default, ActionResponseStatus status = ActionResponseStatus.Success)
    //{
    //    return new StatusResponse<Guid>
    //    {
    //        RecordId = recordId,
    //        Status = status,
    //    };
    //}

    public static StatusResponse<TId> FormSqlException<TId>(Exception ex, TId recordId)
    {
        var response = new StatusResponse<TId>()
        {
            RecordId = recordId,
            ErrorMessage = ex.Message,
            StackTrace = ex.StackTrace
        };

        if(ex is DbUpdateConcurrencyException)
        {
            response.Status = ActionResponseStatus.ConcurrencyConflict;
            return response;
        }

        if(ex.GetBaseException() is SqlException sqlEx)
        {
            response.ErrorCode = sqlEx.Number.ToString();
            response.Status = GetStatus(sqlEx.Number);

            return response;
        }

        response.Status = ActionResponseStatus.Unknown;
        return response;
    }

    public static StatusResponse<Guid> FormSqlException<TId>(Exception ex, Guid recordId)
    {
        var response = new StatusResponse<Guid>()
        {
            RecordId = recordId,
            ErrorMessage = ex.Message,
        };

        if (ex is DbUpdateConcurrencyException)
        {
            response.Status = ActionResponseStatus.ConcurrencyConflict;
            return response;
        }

        if (ex.GetBaseException() is SqlException sqlEx)
        {
            response.ErrorCode = sqlEx.Number.ToString();
            response.Status = GetStatus(sqlEx.Number);

            return response;
        }

        response.Status = ActionResponseStatus.Unknown;
        return response;
    }

    private static ActionResponseStatus GetStatus(int errorCode)
    {
        switch (errorCode)
        {
            case 2601:
                return ActionResponseStatus.RecordAlreadyExists;
            case 2627:
                return ActionResponseStatus.RecordAlreadyExists;
            default:
                return ActionResponseStatus.Unknown;
        }
    }

    public static ActionResponse<TResult, TId> GetResult<TResult, TId>(this StatusResponse<TId> statusResponse, TResult result)
    {
        statusResponse ??= new StatusResponse<TId>();

        return new ActionResponse<TResult, TId>()
        {
            Result = result,
            Status = statusResponse.Status,
            RecordId = statusResponse.RecordId,
            ErrorMessage = statusResponse.ErrorMessage,
            ErrorCode = statusResponse.ErrorCode,
            StackTrace = statusResponse.StackTrace
        };
    }

    //public static ActionResponse<TResult, Guid> GetResult<TResult>(this StatusResponse<Guid> statusResponse, TResult result)
    //{
    //    statusResponse ??= new StatusResponse<Guid>();

    //    return new ActionResponse<TResult, Guid>
    //    {
    //        Result = result,
    //        Status = statusResponse.Status,
    //        RecordId = statusResponse.RecordId,
    //        ErrorMessage = statusResponse.ErrorMessage,
    //        ErrorCode = statusResponse.ErrorCode
    //    };
    //}
}
