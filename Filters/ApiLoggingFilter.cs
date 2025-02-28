using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    public class ApiLoggingFilter : IActionFilter
    {
        private readonly ILogger<ApiLoggingFilter> _logger;

        public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
        {
            _logger = logger;
        }

        // After action
        public void OnActionExecuted(ActionExecutedContext context)
        {
            _logger.LogInformation("### Executing -> OnActionExecuted");
            _logger.LogInformation("########################################################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"Status Code : {context.HttpContext.Response.StatusCode}");
            _logger.LogInformation("########################################################################");
        }

        // Before action
        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInformation("### Executing -> OnActionExecuting");
            _logger.LogInformation("########################################################################");
            _logger.LogInformation($"{DateTime.Now.ToLongTimeString()}");
            _logger.LogInformation($"ModelState : {context.ModelState.IsValid}");
            _logger.LogInformation("########################################################################");
        }
    }
}
