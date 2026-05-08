using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using ProjectBoard.Application.Services;

namespace ProjectBoard.Web.Filters;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
    {
        _logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        if (context.Exception is BusinessRuleException ex)
        {
            _logger.LogWarning(ex, "Business rule exception was handled by MVC filter.");
            context.Result = new BadRequestObjectResult(new
            {
                error = "Business rule violation",
                detail = ex.Message
            });
            context.ExceptionHandled = true;
        }
    }
}
