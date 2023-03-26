using System.Net;
using System.Text.RegularExpressions;

namespace Master
{
    public class InjectionMiddleware : IMiddleware
    {
        private readonly ILogger<InjectionMiddleware> logger;

        public InjectionMiddleware(
            ILogger<InjectionMiddleware> logger)
        {
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                context.Request.EnableBuffering();
                var bodyAsText = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Position = 0;
                Regex regex = new Regex(@"[;#]", RegexOptions.Multiline | RegexOptions.Singleline);
                Match match = regex.Match(bodyAsText);
                if (match.Success == false)
                {
                    regex = new Regex(@"^.*select.*$|^.*insert.*$|^.*delete.*$|^.*update.*$|^.*0x.*$",
                        RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
                    var s = regex.Match(bodyAsText);
                    if (s.Success == false)
                    {
                        await next(context);
                        return;
                    }
                }
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }           
            catch (Exception ex)
            {
                logger.LogCritical(ex, ex.Message);
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

    }
}
    }
}
