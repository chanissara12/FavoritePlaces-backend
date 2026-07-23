using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.Net;
using System.Runtime.InteropServices;
using Domain.Exceptions;
using Microsoft.Data.SqlClient;
using static Domain.Exceptions.ValidateException;
using Domain.Extensions;

namespace Web.Handlers
{
    public static class CustomExceptionHandler
    {
        public static void UseCustomExceptionHandler(this IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    if (context.Features.Get<IExceptionHandlerFeature>() != null)
                    {
                        var ex = context.Features.Get<IExceptionHandlerFeature>().Error;

                        var result = new GlobalExceptionResult();
                        if (ex is UnauthorizedAccessException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                            await context.Response.WriteAsync(ex.Message);
                            return;
                        }
                        else if (ex is ValidateException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            foreach (var exMsg in ((ValidateException)ex).Messages)
                            {
                                var error = new ExceptionViewModel();
                                error.ElementId = exMsg.ElementId;
                                error.Message = exMsg.Message;
                                result.Errors.Add(error);
                            }
                        }
                        //else if (ex is EmailException)
                        //{
                        //    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        //    var emailEx = (EmailException)ex;
                        //    var error = new ExceptionViewModel();
                        //    error.Message = emailEx.AllMessages();
                        //    result.Errors.Add(error);
                        //}
                        //else if (ex is ApiValidateException)
                        //{
                        //    await HandlerApiError(context, ex);
                        //    return;
                        //}
                        else
                        {
                            bool isSqlException = ex is SqlException;
                            //using var scope = serviceProvider.CreateScope();
                            //var appLogger = scope.ServiceProvider.GetRequiredService<IAppLogger>();
                            //var utils = scope.ServiceProvider.GetRequiredService<IUtilsService>();
                            //string userName = await utils.GetUserName();

                            //string errorCode = await appLogger.Error(ex, userName);

                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            var error = new ExceptionViewModel();
                            if (isSqlException)
                            {
                                error.Message = ex.AllMessages();
                            }
                            else
                            {
                                error.Message = $"ErrorCode [errorCode]\r\n{ex.AllMessages()}";
                                //error.Message = $"ErrorCode [{errorCode}]\r\n{ex.AllMessages()}";
                            }
                            result.Errors.Add(error);
                        }

                        context.Response.ContentType = "application/json";
                        var jsonSerializerSettings = new JsonSerializerSettings
                        {
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        };
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(result, jsonSerializerSettings));
                    }
                });
            });
        }

        //private static async Task HandlerApiError(HttpContext context, Exception ex)
        //{
        //    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        //    var listOfError = new List<ApiExceptionViewModel>();
        //    foreach (var exMsg in ((ApiValidateException)ex).MessagesDictionary)
        //    {
        //        listOfError.Add(new ApiExceptionViewModel()
        //        {
        //            FieldName = exMsg.Key.FieldName,
        //            Type = exMsg.Key.Type,
        //            Index = exMsg.Key.Index,
        //            Message = exMsg.Value
        //        });
        //    }
        //    context.Response.ContentType = "application/json";
        //    await context.Response.WriteAsync(JsonConvert.SerializeObject(listOfError, new JsonSerializerSettings
        //    {
        //        ContractResolver = new CamelCasePropertyNamesContractResolver()
        //    }));
        //}

        private class GlobalExceptionResult
        {
            public List<ExceptionViewModel> Errors { get; set; } = new List<ExceptionViewModel>();
        }
    }
}
