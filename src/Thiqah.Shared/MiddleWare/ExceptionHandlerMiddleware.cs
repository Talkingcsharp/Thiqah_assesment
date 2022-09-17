using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Thiqah.Shared.Context;
using Thiqah.Shared.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Thiqah.Shared.MiddleWare
{
    public sealed class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context, ActiveContext activeContext)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var response = context.Response;
                response.ContentType = "application/json";
                var model = GetExceptionModel(ex, activeContext);
                var modelSerialized = JsonSerializer.Serialize(model);
                response.StatusCode = model.Code;
                await response.WriteAsync(modelSerialized);
            }
        }

        private ExceptionViewModel GetExceptionModel(Exception ex, ActiveContext activeContext)
        {
            var model = new ExceptionViewModel
            {
                Message = ex.Message,
                RequestId = activeContext.RequestId,
                StackTrace = ex.StackTrace
            };

            if (ex is DataNotFoundException)
            {
                model.Code = (int)HttpStatusCode.NotFound;
                model.Status = HttpStatusCode.NotFound.ToString();
            }
            else if (ex is DataNotValidException exc)
            {
                model.ValidationResult = exc?.ValidationErrors;
                model.Code = (int)HttpStatusCode.BadRequest;
                model.Status = HttpStatusCode.BadRequest.ToString();
            }
            else if (ex is ArgumentException)
            {
                model.Code = (int)HttpStatusCode.NotAcceptable;
                model.Status = HttpStatusCode.NotAcceptable.ToString();
            }
            else if (ex is ArgumentNullException)
            {
                model.Code = (int)HttpStatusCode.BadRequest;
                model.Status = HttpStatusCode.BadRequest.ToString();
            }
            else
            {
                model.Code = (int)HttpStatusCode.InternalServerError;
                model.Status = HttpStatusCode.InternalServerError.ToString();
            }

            if (ex.InnerException is not null)
            {
                model.InnerException = GetExceptionModel(ex.InnerException, activeContext);
            }

            return model;
        }
    }
}
