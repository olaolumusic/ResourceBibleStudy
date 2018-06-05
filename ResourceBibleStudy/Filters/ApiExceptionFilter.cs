using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http.Filters;
using ResourceBibleStudy.Models;

namespace ResourceBibleStudy.Filters
{
    public class ApiExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            context.Response = new HttpResponseMessage(HttpStatusCode.BadRequest);
            context.Response.Content = CreateApiResponse(context);
        }

        private ObjectContent<ApiErrorMessage> CreateApiResponse(HttpActionExecutedContext context)
        {
            var apiErrorMessage = new ApiErrorMessage
            {
                ErrorMessage = context.Exception.Message,
                ErrorType = context.Exception.GetType().ToString()
            };

            if (context.Exception is AggregateException)
            {
                apiErrorMessage = new ApiErrorMessage
                {
                    ErrorMessage = context.Exception.InnerException.Message,
                    ErrorType = string.Format("{0}, {1}", context.Exception.GetType(), context.Exception.InnerException.GetType())
                };
            }

            return new ObjectContent<ApiErrorMessage>(apiErrorMessage, new JsonMediaTypeFormatter());
        }
    }
}