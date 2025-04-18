using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using App.Shared.Util.ExceptionHandling.Types;

namespace App.Client.Util.Filters;

public class ExceptionAndToastFilter(IToastNotification toastNotification
) : IActionFilter, //  action çalşırken veya çalıştıkran sonra neler yapılacağını belirtiyoruz
    IExceptionFilter //  action, exception throw ettiğinde neler yapılacağını belirtiyoruz
{
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Controller is Controller controller)
        {
            if (controller.TempData["ErrorMessage"] != null)
            {
                var errorMessage = controller.TempData["ErrorMessage"]!.ToString();
                toastNotification.AddErrorToastMessage(errorMessage, new ToastrOptions
                {
                    Title = "Hata"
                });
                controller.TempData.Remove("ErrorMessage");
            }

            if (controller.TempData["SuccessMessage"] != null)
            {
                var errorMessage = controller.TempData["SuccessMessage"]!.ToString();
                toastNotification.AddSuccessToastMessage(errorMessage, new ToastrOptions
                {
                    Title = "Başarılı"
                });
                controller.TempData.Remove("SuccessMessage");
            }
        }
    }

    public void OnActionExecuting(ActionExecutingContext context) { }

    public void OnException(ExceptionContext context)
    {
        var exception = context.Exception;

        switch (exception)
        {
            case ValidationException:
                toastNotification.AddWarningToastMessage(exception.Message);
                break;

            case NotFoundException:
                toastNotification.AddInfoToastMessage(exception.Message);
                break;

            case UnauthorizedException:
                toastNotification.AddErrorToastMessage(exception.Message);
                break;

            case BadRequestException:
                toastNotification.AddAlertToastMessage(exception.Message);
                break;

            case DeserializationException:
                toastNotification.AddErrorToastMessage(exception.Message);
                break;

            case ConflictException:
                toastNotification.AddErrorToastMessage(exception.Message);
                break;

            default:
                toastNotification.AddErrorToastMessage(exception.Message);
                break;
        }

        if (context.HttpContext.Request.Method == "GET")
        {
            context.Result = new RedirectToActionResult("Error", "Home", null);
        }
        else if (context.HttpContext.Request.Method == "POST")
        {
            var controller = context.RouteData.Values["controller"]?.ToString();
            var action = context.RouteData.Values["action"]?.ToString();

            if (!string.IsNullOrWhiteSpace(controller) && !string.IsNullOrWhiteSpace(action))
            {
                context.Result = new RedirectToActionResult(action, controller, null);
            }
        }

        context.ExceptionHandled = true;
    }
}