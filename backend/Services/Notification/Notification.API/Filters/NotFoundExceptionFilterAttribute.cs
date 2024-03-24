using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace Notification.API.Filters;

public class NotFoundExceptionFilterAttribute : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {
        var result = new ObjectResult(new
        {
            context.Exception.Message,
            context.Exception.Source,
            ExceptionType = context.Exception.GetType().FullName,
        })
        {
            StatusCode = (int)HttpStatusCode.NotFound
        };

        Log.Error("Unhandled exception occurred while executing request: {ex}", context.Exception);

        context.Result = result;
    }
}