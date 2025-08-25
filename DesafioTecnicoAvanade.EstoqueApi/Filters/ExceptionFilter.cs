using DesafioTecnicoAvanade.EstoqueApi.Filters.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace DesafioTecnicoAvanade.EstoqueApi.Filters
{
    public class ExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            switch (context.Exception)
            {
                case InsufficientStockException:
                    context.Result = new BadRequestObjectResult(context.Exception.Message);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                case ProductNotFoundException:
                    context.Result = new NotFoundObjectResult(context.Exception.Message);
                    context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    context.Result = new ObjectResult("Ocorreu um erro interno no servidor.")
                    {
                        StatusCode = (int)HttpStatusCode.InternalServerError
                    };
                    break;
            }

            context.ExceptionHandled = true;
        }
    }
}
