using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace Hahn.ApplicationProcess.July2021.Web.Controllers
{
    struct StructResponse
    {
        public string Title { get; set; }
        public string Detail { get; set; }
        public int StatusCode { get; set; }
    }

    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : ControllerBase
    {
        private readonly ILogger<ErrorController> _logService;

        public ErrorController(ILogger<ErrorController> logService)
        {
            _logService = logService;
        }

        [Route("/error-local-development")]
        public IActionResult ErrorLocalDevelopment(
            [FromServices] IWebHostEnvironment webHostEnvironment)
        {
            if (webHostEnvironment.EnvironmentName != "Development")
            {
                throw new InvalidOperationException(
                    "This shouldn't be invoked in non-development environments.");
            }

            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;

            _logService.LogError(exception, "Uncaught Exception");

            return BuildResponseResult(exception);
        }

        [Route("/error")]
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>().Error;
            _logService.LogError(exception, "Uncaught Exception");

            return BuildResponseResult(exception);
        }

        private ObjectResult BuildResponseResult(Exception errorException)
        {
            var data = new StructResponse
            {
                Title = errorException.Message,
                Detail = errorException.InnerException != null ? errorException.InnerException.Message : errorException.Message,
                StatusCode = 500
            };

            return StatusCode(data.StatusCode, data);
        }
    }
}
