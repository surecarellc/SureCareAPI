using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using System.Threading.Tasks;

public class CorsMiddleware : IFunctionsWorkerMiddleware
{
    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        var httpContext = context.GetHttpContext();

        httpContext.Response.Headers["Access-Control-Allow-Origin"] = "*";
        httpContext.Response.Headers["Access-Control-Allow-Headers"] = "*";
        httpContext.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";

        if (httpContext.Request.Method == HttpMethods.Options)
        {
            httpContext.Response.StatusCode = 204;
            return;
        }

        await next(context);
    }
}
