using Microsoft.AspNetCore.Mvc.Filters;

namespace ProjectBoard.Web.Filters;

public class ApiActionLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiActionLoggingFilter> _logger;

    public ApiActionLoggingFilter(ILogger<ApiActionLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation(
            "Executing action {Action}. Arguments={ArgumentCount}",
            context.ActionDescriptor.DisplayName,
            context.ActionArguments.Count);
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("Executed action {Action}", context.ActionDescriptor.DisplayName);
    }
}
