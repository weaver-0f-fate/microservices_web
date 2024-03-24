using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;
using System.Net;

namespace Events.API.Filters;

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