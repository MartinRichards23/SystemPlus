using Microsoft.AspNetCore.Http;
using System;

namespace SystemPlus.Web
{
    public static class HttpExtensions
    {
        public static string UserAgent(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Headers["User-Agent"];
        }

        public static string Referer(this HttpRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            return request.Headers["Referer"];
        }

        public static string? GetIpAddress(this HttpContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            return context.Connection.RemoteIpAddress.ToString();
        }
    }
}
