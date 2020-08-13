using FluentValidation;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace WebApi
{
    public class HandleValidationExceptionAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is ValidationException)
            {
                var apiException = context.Exception as ValidationException;
                context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(apiException.Message)
                };
            }
        }
    }
}